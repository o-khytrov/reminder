using System;
using System.ComponentModel.DataAnnotations;

namespace reminder.Entities
{
    public class Note
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime RemindDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Place { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool Dismissed { get; set; }

        public bool Completed { get; set; }
    }
}