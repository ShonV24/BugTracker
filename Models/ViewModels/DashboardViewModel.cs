﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<BTUser> Users { get; set; }
        public List<Company> Company { get; set; }
        //public List<Notification> Notifications { get; set; }
        //public Company CurrentCompany { get; set; }
    }
}
