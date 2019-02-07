using System.Threading.Tasks;
using BookStore.Api.Identity.Models;

namespace BookStore.Api.Identity.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(long id);

        Task<User> GetUserAsync(string userName, string password);

        Task<User> GetUserAsync(string userName);

        Task<User> CreateUser(string userName, string password);
    }
}