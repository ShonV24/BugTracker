﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTracker.Data;
using BugTracker.Models;
using BugTracker.Extensions;
using BugTracker.Services.Interfaces;
using BugTracker.Services;
using Microsoft.AspNetCore.Identity;
using BugTracker.Models.ViewModels;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBTCompanyInfoService _companyInfoService;
        private readonly IBTTicketService _bTTicketService;
        private readonly IBTProjectService _projectService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IBTHistoryService _historyService;
        private readonly IBTNotificationService _notificationService;

        public TicketsController(ApplicationDbContext context,
                                 IBTCompanyInfoService companyInfoService,
                                 IBTTicketService bTTicketService,
                                 UserManager<BTUser> userManager, IBTProjectService projectService,
                                 IBTHistoryService historyService,
                                 IBTNotificationService notificationService)
        {
            _context = context;
            _companyInfoService = companyInfoService;
            _bTTicketService = bTTicketService;
            _userManager = userManager;
            _projectService = projectService;
            _historyService = historyService;
            _notificationService = notificationService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ticket.Include(t => t.DeveloperUser).Include(t => t.OwnerUser);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> AllTickets()
        {

            // Get the company tickets
            int companyId = User.Identity.GetCompanyId().Value;
            List<Ticket> tickets = await _companyInfoService.GetAllTicketsAsync(companyId);


            return View(tickets);
        }

        public async Task<IActionResult> MyTickets()
        {
            // Get current userId
            string userId = (await _userManager.GetUserAsync(User)).Id;

            // Get my Tickets
            int companyId = User.Identity.GetCompanyId().Value;

            List<Ticket> tickets = await _bTTicketService.GetAllTicketsByCompanyAsync(companyId);

            List<Ticket> myDevTickets = tickets.Where(t => t.DeveloperUserId == userId).ToList();
            List<Ticket> mySubTickets = tickets.Where(t => t.OwnerUserId == userId).ToList();

            List<Ticket> myTickets = myDevTickets.Concat(mySubTickets).ToList();


            return View(myTickets);
        }

        [HttpGet]
        public async Task<IActionResult> AssignTicket(int? ticketId)
        {
            if (!ticketId.HasValue)
            {
                return NotFound();
            }

            AssignDeveloperViewModel model = new();
            int companyId = User.Identity.GetCompanyId().Value;

            model.Ticket = (await _bTTicketService.GetAllTicketsByCompanyAsync(companyId)).FirstOrDefault(t => t.Id == ticketId);
            model.Developers = new SelectList(await _projectService.DevelopersOnProjectAsync(model.Ticket.ProjectId), "Id", "FullName");            
            
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AssignTicket(AssignDeveloperViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.DeveloperId))
            {
                int companyId = User.Identity.GetCompanyId().Value;

                BTUser btUser = await _userManager.GetUserAsync(User);
                BTUser developer = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(m => m.Id == viewModel.DeveloperId);
                BTUser projectManager = await _projectService.GetProjectManagerAsync(viewModel.Ticket.ProjectId);

                Ticket oldTicket = await _context.Ticket
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.DeveloperUser)
                                                 .AsNoTracking().FirstOrDefaultAsync(t => t.Id == viewModel.Ticket.Id);

                await _bTTicketService.AssignTicketAsync(viewModel.Ticket.Id, viewModel.DeveloperId);

                Ticket newTicket = await _context.Ticket
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.DeveloperUser)
                                                 .AsNoTracking().FirstOrDefaultAsync(t => t.Id == viewModel.Ticket.Id);

                await _historyService.AddHistoryAsync(oldTicket, newTicket, btUser.Id);
            }
            return RedirectToAction("Details", new { id = viewModel.Ticket.Id });
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .Include(t => t.Project)
                .Include(t => t.TicketPriority)
                .Include(t => t.TicketStatus)
                .Include(t => t.TicketType)
                .Include(t => t.Comments)
                    .ThenInclude(t => t.User)
                .Include(t => t.Attachments)
                    .ThenInclude(t => t.User)
                .Include(t => t.History)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            //get current user
            BTUser btUser = await _userManager.GetUserAsync(User);

            //get current user's company Id
            int companyId = User.Identity.GetCompanyId().Value;

            //TODO: Filter List
            if (User.IsInRole("Administrator"))
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompany(companyId), "Id", "Name");
            }
            else
            {
                ViewData["ProjectId"] = new SelectList(await _projectService.ListUserProjectsAsync(btUser.Id), "Id", "Name");
            }

            ViewData["TicketPriorityId"] = new SelectList(_context.Set<TicketPriority>(), "Id", "Name");



            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,Title,Description,ProjectId,TicketTypeId,TicketPriorityId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                BTUser btUser = await _userManager.GetUserAsync(User);


                ticket.Created = DateTimeOffset.Now;

                string userId = _userManager.GetUserId(User);
                ticket.OwnerUserId = userId;

                ticket.TicketStatusId = (await _bTTicketService.LookupTicketStatusIdAsync("New")).Value;

                _context.Add(ticket);
                await _context.SaveChangesAsync();

                #region Add History
                //Add history'
                Ticket newTicket = await _context.Ticket
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.TicketType)
                    .Include(t => t.Project)
                    .Include(t => t.DeveloperUser)
                    .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);

                await _historyService.AddHistoryAsync(null, newTicket, btUser.Id);
                #endregion

                #region Notification
                BTUser projectManager = await _projectService.GetProjectManagerAsync(ticket.ProjectId);
                int companyId = User.Identity.GetCompanyId().Value;

                if (projectManager != null)
                {


                    Notification notification = new()
                    {
                        TicketId = ticket.Id,
                        Title = "New Ticket",
                        Message = $"New Ticket: {ticket?.Title}, was created by {btUser?.FullName}",
                        Created = DateTimeOffset.Now,
                        SenderId = btUser?.Id,
                        RecipientId = projectManager?.Id
                    };

                    if (projectManager != null)
                    {
                        await _notificationService.SaveNotificationAsync(notification);
                    }
                    else
                    {
                        //Admin notification
                        await _notificationService.AdminsNotificationAsync(notification, companyId);
                    }

                }
                #endregion
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            //ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            //ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", ticket.ProjectId);
            //ViewData["TicketsPriorityId"] = new SelectList(_context.Set<TicketPriority>(), "Id", "Name", ticket.TicketPriorityId);
            //ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Name", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.Set<TicketPriority>(), "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,Archived,ArchivedDate,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,DeveloperUserId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            Notification notification;

            if (ModelState.IsValid)
            {

                int companyId = User.Identity.GetCompanyId().Value;
                BTUser btUser = await _userManager.GetUserAsync(User);
                BTUser projectManager = await _projectService.GetProjectManagerAsync(ticket.ProjectId);

                Ticket oldTicket = await _context.Ticket
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.DeveloperUser)
                                                 .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);

                try
                {
                    ticket.Updated = DateTimeOffset.Now;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();


                    //Create and Save a Notification
                    notification = new()
                    {
                        TicketId = ticket.Id,
                        Title = $"Ticket modified on project - {oldTicket.Project.Name}",
                        Message = $"Ticket: [{ticket.Id}]:{ticket.Title} updated by {btUser?.FullName}",
                        Created = DateTimeOffset.Now,
                        SenderId = btUser?.Id,
                        RecipientId = projectManager?.Id
                    };

                    if (projectManager != null)
                    {
                        await _notificationService.SaveNotificationAsync(notification);
                    }
                    else
                    {
                        await _notificationService.AdminsNotificationAsync(notification, companyId);
                    }

                    if(ticket.DeveloperUserId != null)
                    {
                        notification = new()
                        {
                            TicketId = ticket.Id,
                            Title = "A ticket assigned to you has been modified",
                            Message = $"Ticket: [{ticket.Id}]:{ticket.Title} updated by {btUser?.FullName}",
                            Created = DateTimeOffset.Now,
                            SenderId = btUser?.Id,
                            RecipientId = ticket.DeveloperUserId
                        };
                        await _notificationService.SaveNotificationAsync(notification);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                //Add History
                Ticket newTicket = await _context.Ticket
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.DeveloperUser)
                                                 .AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticket.Id);

                await _historyService.AddHistoryAsync(oldTicket, newTicket, btUser.Id);

                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["OwnerUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.OwnerUserId);
            ViewData["Projectd"] = new SelectList(_context.Project, "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.Set<TicketPriority>(), "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.DeveloperUser)
                .Include(t => t.OwnerUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
