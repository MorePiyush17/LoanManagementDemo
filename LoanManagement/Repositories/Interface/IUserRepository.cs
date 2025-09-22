using System.Collections.Generic;
using System.Threading.Tasks;
using LoanManagement.Models;

namespace LoanManagement.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveAsync();
        Task<User?> GetUserWithDetailsAsync(int userId);
    }
}