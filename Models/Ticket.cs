using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated")]
        public DateTime? Updated { get; set; }

        [Display(Name = "Archived")]
        public bool Archived { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Archived")]
        public DateTimeOffset ArchivedDate { get; set; }

        [DisplayName("Project")]
        public string ProjectId { get; set; }

        [DisplayName("Ticket Type")]
        public string TicketTypeId { get; set; }

        [DisplayName("Ticket Priority")]
        public string TicketPriorityId { get; set; }

        [DisplayName("Ticket Status")]
        public string TicketStatusId { get; set; }

        [DisplayName("Ticket Owner")]
        public string OwnerUserId { get; set; }

        [DisplayName("Ticket Developer")]
        public string DeveloperUserId { get; set; }


        // -- Navigational Properties --//
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual BTUser OwnerUser { get; set; }
        public virtual BTUser DeveloperUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();
    }
}
