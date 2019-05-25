using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        [MaxLength(100)]
        public string ImageFile { get; set; }

        public byte[] ImageBinary { get; set; }


    }
}
