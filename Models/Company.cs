﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        //MaxFileSize(1024 * 1024)
        //[AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile ImageFormFile { get; set; }

        [DisplayName("File Name")]
        public string ImageFileName { get; set; }
        public byte[] ImageFileData { get; set; }

        [DisplayName("FileExtension")]
        public string ImageContentType { get; set; }

        public virtual ICollection<BTUser> Members { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }
}