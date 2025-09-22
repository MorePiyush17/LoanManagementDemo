using LoanManagement.Models;

namespace LoanManagement.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<string> LoginUserAsync(string email, string password);
    }
}
