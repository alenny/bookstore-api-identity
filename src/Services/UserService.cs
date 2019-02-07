using System.Threading.Tasks;
using System.Linq;
using BookStore.Api.Identity.Models;
using BookStore.Api.Identity.Data;

namespace BookStore.Api.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _context.User.FindAsync(id);
        }

        public Task<User> GetUserAsync(string userName, string password)
        {
            return Task.FromResult(_context.User.FirstOrDefault(u => u.Name == userName && u.Password == password));
        }

        public Task<User> GetUserAsync(string userName)
        {
            return Task.FromResult(_context.User.FirstOrDefault(u => u.Name == userName));
        }

        public async Task<User> CreateUser(string userName, string password)
        {
            var user = new User { Name = userName, Password = password };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}