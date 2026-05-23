using TennisAcademyApp.ViewModels.Admin.UserManagement;
using TennisAcademyApp.ViewModels.DropDown;

namespace TennisAcademyApp.Services.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserIndexViewModel>> GetUserManagementDataAsync(string userId);
        Task<bool> AssignUserToRoleAsync(string userId, string role);
        Task<bool> RemoveUserFromRoleAsync(string userId, string role);
        Task<bool> RemoveUserAsync(string userId);
    }
}
