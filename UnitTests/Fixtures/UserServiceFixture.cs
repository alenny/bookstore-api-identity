using System.Threading.Tasks;
using System.Linq;
using BookStore.Api.Identity.Models;
using BookStore.Api.Identity.Data;
using System.Collections.Generic;
using BookStore.Api.Identity.Services;

namespace BookStore.Api.Identity.UnitTests.Fixtures
{
    public class UserServiceFixture : IUserService
    {
        private Dictionary<long, User> _users = new Dictionary<long, User>();

        private long _nextId = 1;

        public UserServiceFixture()
        {
            var user = new User { Id = _nextId++, Name = "dzy", Password = "dzy" };
            _users.Add(user.Id, user);
        }

        public Task<User> GetUserByIdAsync(long id)
        {
            return Task.FromResult(_users[id]);
        }

        public Task<User> GetUserAsync(string userName, string password)
        {
            return Task.FromResult(_users.Values.FirstOrDefault(u => u.Name == userName && u.Password == password));
        }

        public Task<User> GetUserAsync(string userName)
        {
            return Task.FromResult(_users.Values.FirstOrDefault(u => u.Name == userName));
        }

        public Task<User> CreateUser(string userName, string password)
        {
            var user = new User { Id = _nextId++, Name = userName, Password = password };
            _users.Add(user.Id, user);
            return Task.FromResult(user);
        }
    }
}