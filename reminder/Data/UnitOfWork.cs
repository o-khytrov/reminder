using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using reminder.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace reminder.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private NotesRepository _notesRepository;

        public NotesRepository Notes => _notesRepository ?? (_notesRepository = new NotesRepository(_db));

        public DbSet<ApplicationUser> Users => _db.Users;

        public static ApplicationUser CurrentUser { get; private set; }

        private static UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public UnitOfWork(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            CurrentUser = _db.Users.SingleOrDefault(u => u.UserName == _userManager.GetUserId(_httpContextAccessor.HttpContext.User));
        }

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