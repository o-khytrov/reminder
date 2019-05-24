using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Data
{
    public class NotesRepository
    {
        protected ApplicationDbContext _db;

        public NotesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
