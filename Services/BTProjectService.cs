using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services
{
    public class BTProjectService : IBTProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BTProjectService> _logger;
        private readonly IBTRolesService _roleService;
        private readonly UserManager<BTUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BTProjectService(ApplicationDbContext context,
                            UserManager<BTUser> userManager,
                            IBTCompanyInfoService BTCompanyInfoService,
                            IBTRolesService roleService,
                            ILogger<BTProjectService> logger,
                            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleService = roleService;
            _logger = logger;
            _roleManager = roleManager;
        }


        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            BTUser pm = await GetProjectManagerAsync(projectId);

            try
            {
                if (pm is not null)
                {
                    await RemoveProjectManagerAsync(projectId);
                }

            }
            catch
            {
                throw;
            }
            try
            {
                await AddUserToProjectAsync(userId, projectId);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BTUser>> DevelopersOnProjectAsync(int projectId)
        {
            Project project = await _context.Project
                                 .Include(p => p.Members)
                                 .FirstOrDefaultAsync(u => u.Id == projectId);

            List<BTUser> developers = new();

            foreach (var user in project.Members)
            {
                if (await _roleService.IsUserInRoleAsync(user, "Developer"))
                {
                    developers.Add(user);
                }
            }
            return developers;
        }



        public async Task<List<Project>> GetAllProjectsByCompany(int companyId)
        {
            List<Project> projects = new();

            projects = await _context.Project.Include(p => p.Members)
                                 .Include(p => p.ProjectPriority)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.OwnerUser)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.DeveloperUser)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.Comments)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.Attachments)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.History)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.TicketPriority)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.TicketStatus)
                                 .Include(p => p.Tickets)
                                    .ThenInclude(t => t.TicketType)
                                 .Where(p => p.CompanyId == companyId).ToListAsync();

            return projects;
        }

        public async Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            int priorityId = _context.ProjectPriority.FirstOrDefault(p => p.Name == priorityName).Id;

            List<Project> priorityProjects = await GetAllProjectsByCompany(companyId);

            return priorityProjects.Where(u => u.ProjectPriorityId == priorityId).ToList();
        }
        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            return (await _context.ProjectPriority.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;
        }
        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            return await _context.Project.Where(p => p.CompanyId == companyId && p.Archived == true).ToListAsync();
        }

        public async Task<List<BTUser>> GetMembersWithoutPMAsync(int projectId)
        {
            //List<BTUser> developers = await DevelopersOnProjectAsync(projectId);
            List<BTUser> developers = await GetProjectMembersByRoleAsync(projectId, "Developer");
            List<BTUser> submitters = await GetProjectMembersByRoleAsync(projectId, "Submitters");
            List<BTUser> administrators = await GetProjectMembersByRoleAsync(projectId, "Administrator");

            List<BTUser> teamMembers = developers.Concat(submitters).Concat(administrators).ToList();

            return teamMembers;
        }

        public async Task<BTUser> GetProjectManagerAsync(int projectId)
        {
            Project project = await _context.Project
                              .Include(p => p.Members)
                              .FirstOrDefaultAsync(u => u.Id == projectId);
            foreach (BTUser member in project?.Members)
            {
                if (await _roleService.IsUserInRoleAsync(member, "ProjectManager"))
                {
                    return member;
                }
            }
            return null;
        }

        public async Task<List<BTUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            //Get the project and members from the Db
            Project project = await _context.Project
                              .Include(p => p.Members)
                              .FirstOrDefaultAsync(m => m.Id == projectId);

            List<BTUser> users = new();
            //loop through the members and add them to project by passing in role. 
            foreach (BTUser user in project.Members)
            {
                if (await _roleService.IsUserInRoleAsync(user, role))
                {
                    users.Add(user);
                }
            }
            //List<BTUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id == projectId);

            return users;
        }

        public async Task<bool> IsUserOnProject(string userId, int projectId)
        {
            Project project = await _context.Project
                                     .FirstOrDefaultAsync(p => p.Id == projectId);

            bool result = project.Members.Any(u => u.Id == userId);

            return result;

        }

        public async Task<List<Project>> ListUserProjectsAsync(string userId)
        {
            try
            {
                List<Project> userProjects = (await _context.Users
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Company)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Members)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.DeveloperUser)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.OwnerUser)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.TicketPriority)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.TicketStatus)
                                              .Include(u => u.Projects)
                                                .ThenInclude(p => p.Tickets)
                                                    .ThenInclude(t => t.TicketType)
                                              .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch
            {
                throw;
            }


        }
        public Task RemoveProjectManagerAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                List<BTUser> members = await GetProjectMembersByRoleAsync(projectId, role);
                Project project = await _context.Project.FirstOrDefaultAsync(p => p.Id == projectId);

                foreach (BTUser btUser in members)
                {
                    try
                    {
                        project.Members.Remove(btUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"*** ERROR *** - Error Removing users from project. ==> {ex.Message}");
            }
        }
        public async Task<List<BTUser>> SubmittersOnProjectAsync(int projectId)
        {
            Project project = await _context.Project
                              .Include(p => p.Members)
                              .FirstOrDefaultAsync(u => u.Id == projectId);
            // instantiate a list of submitters
            List<BTUser> submitters = new();

            foreach (var user in project.Members)
            {
                if (await _roleService.IsUserInRoleAsync(user, "Submitter"))
                {
                    submitters.Add(user);
                }
            }
            return submitters;
        }

        public async Task<List<BTUser>> UsersNotOnProjectAsync(int projectId, int companyId)
        {
            List<BTUser> usersNotOnProject = new();
            //usersNotOnProject = _context.Users.Where(u => u.Projects.All(p => p.Id = projectId) && .CompanyId == companyId).ToListAsync();

            //return usersNotOnProject.ToList


            List<BTUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId) && u.CompanyId == companyId).ToListAsync();

            return users;
        }
    }

    //public async Task<List<BTUser>> UsersNotOnProjectAsync(int projectId, int companyId)
    //{
    //    List<BTUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId) && u.CompanyId == companyId).ToListAsync();

    //    return users;
    //}

}

            
