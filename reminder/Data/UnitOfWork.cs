using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reminder.Entities;

namespace reminder.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        private NotesRepository _notesRepository;

        public NotesRepository Notes => _notesRepository ?? (_notesRepository = new NotesRepository(_db));

        public DbSet<ApplicationUser> Users => _db.Users;

        public async Task CompleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Complete()
        {
            _db.SaveChanges();
        }

    }
}
