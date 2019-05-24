using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace reminder.Entities
{
    public class Note
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Palce { get; set; }
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool Dismissed { get; set; }

        public bool Completed { get; set; }

    }
}
