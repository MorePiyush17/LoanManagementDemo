namespace LoanManagement.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<Models.User> GetUserByEmailAsync(string email);
        Task<bool> AddUserAsync(Models.User user);
        Task<bool> EmailExistsAsync(string email);
    }

}
