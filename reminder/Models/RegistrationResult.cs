using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Models
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string[] Messages { get; set; }
    }
}
