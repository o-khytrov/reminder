using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Models
{
    public class ApplicationUserDto 
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        [MaxLength(100)]
        public string ImageFile { get; set; }

        public byte[] ImageBinary { get; set; }

        public string ImageData { get; set; }
    }
}
