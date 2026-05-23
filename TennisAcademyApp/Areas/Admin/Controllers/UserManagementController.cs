using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Services.Core.Contracts;
using static TennisAcademyApp.GCommon.Validations.SuccessfulMessages.UserManagement;
using static TennisAcademyApp.GCommon.Validations.ErrorMessages.UserManagement;
using static TennisAcademyApp.GCommon.Validations.ValidationConstants;
using TennisAcademyApp.ViewModels.Admin.UserManagement;

namespace TennisAcademyApp.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        private readonly IUserService userService;
        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await userService.GetUserManagementDataAsync(GetUserId()!);

                return View(users);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            try
            {
                bool result = await userService.AssignUserToRoleAsync(userId, role);
                if (result)
                {
                    TempData["SuccessMessage"] = UserAssignedToRoleSuccessfully;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("Failed to assign role. User or role may not exist.");
                }
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = UserAlreadyInRoleErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            try
            {
                bool result = await userService.RemoveUserFromRoleAsync(userId, role);
                if (result)
                {
                    TempData["SuccessMessage"] = UserRemovedFromRoleSuccessfully;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("Failed to remove role. User or role may not exist.");
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = UserFailedToRemoveFromRoleErrorMessage;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                bool result = await userService.RemoveUserAsync(userId);
                if (result)
                {
                    TempData["SuccessMessage"] = UserRemovedSuccessfully;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest("Failed to remove user. User may not exist.");
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
