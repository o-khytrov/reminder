using Microsoft.EntityFrameworkCore;
using reminder.Entities;
using System.Collections.Generic;
using System.Linq;

namespace reminder.Data
{
    public class NotesRepository
    {
        protected ApplicationDbContext _db;

        public string currentUserId { get; set; }
        public NotesRepository(ApplicationDbContext db)
        {
            _db = db;
            currentUserId = UnitOfWork.CurrentUser?.Id;
        }

        public void Add(Note note)
        {
            note.UserId = UnitOfWork.CurrentUser.Id;
            _db.Notes.Add(note);
        }

        public Note Get(int id)
        {
            var record = _db.Notes

                .SingleOrDefault(x => x.Id == id && x.UserId == currentUserId);

            return record;
        }

        public IEnumerable<Note> GetList()
        { 
            return _db.Notes.Where(x => x.UserId == currentUserId).ToList();
        }

        public void Update(Note record)
        {
            _db.Entry(record).State = EntityState.Modified;
        }

        public void Remove(Note record)
        {
            _db.Notes.Remove(record);
        }
    }
}