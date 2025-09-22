using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface IUserService
    {
        Task<User?> AuthenticateUserAsync(string email, string password);
        Task<bool> RegisterUserAsync(User user, string role);
        Task<User?> GetUserProfileAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> UpdateUserProfileAsync(User user);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> IsEmailAvailableAsync(string email);
    }
}