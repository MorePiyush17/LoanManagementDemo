using LoanManagement.Models;
using LoanManagement.Repositories.Implementation;
using LoanManagement.Repositories.Interface;
using LoanManagement.Services.Interface;

namespace LoanManagement.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly ILoanAdminRepository _adminRepo;
        private readonly ILoanOfficerRepository _officerRepo;

        public UserService(IUserRepository userRepo, ICustomerRepository customerRepo,
                             ILoanAdminRepository adminRepo, ILoanOfficerRepository officerRepo)
        {
            _userRepo = userRepo;
            _customerRepo = customerRepo;
            _adminRepo = adminRepo;
            _officerRepo = officerRepo;
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null || user.Password != password)
                return null;

            return user;
        }

        public async Task<bool> RegisterUserAsync(User user, string role)
        {
            try
            {
                if (await _userRepo.EmailExistsAsync(user.Email))
                    return false;

                await _userRepo.AddUserAsync(user);
                if (!await _userRepo.SaveAsync())
                    return false;

                switch (user.Role)
                {
                    case UserRole.Customer:
                        var customer = new Customer
                        {
                            UserId = user.UserId,
                            City = "Not Specified",
                            ContactNumber = "0000000000"
                        };
                        await _customerRepo.AddCustomerAsync(customer);
                        return await _customerRepo.SaveAsync();

                    case UserRole.LoanOfficer:
                        var officer = new LoanOfficer
                        {
                            UserId = user.UserId,
                            City = "Head Office"
                        };
                        await _officerRepo.AddOfficerAsync(officer);
                        return await _officerRepo.SaveAsync();

                    case UserRole.LoanAdmin:
                        var admin = new LoanAdmin
                        {
                            UserId = user.UserId
                        };
                        await _adminRepo.AddAdminAsync(admin);
                        return await _adminRepo.SaveAsync();

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserProfileAsync(int userId)
        {
            return await _userRepo.GetUserWithDetailsAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _userRepo.GetUsersByRoleAsync(role);
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.UserId);
            if (existingUser == null) return false;

            if (existingUser.Email != user.Email)
            {
                if (await _userRepo.EmailExistsAsync(user.Email))
                    return false;
            }

            _userRepo.UpdateUser(user);
            return await _userRepo.SaveAsync();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null || user.Password != oldPassword)
                return false;

            user.Password = newPassword;
            _userRepo.UpdateUser(user);
            return await _userRepo.SaveAsync();
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            _userRepo.DeleteUser(user);
            return await _userRepo.SaveAsync();
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _userRepo.EmailExistsAsync(email);
        }
    }
}