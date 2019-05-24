using Microsoft.EntityFrameworkCore;
using reminder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Data
{
    public interface IUnitOfWork
    {
        DbSet<ApplicationUser> Users { get; }
        Task CompleteAsync();
        void Complete();
        NotesRepository Notes { get; }
    }
}
