using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("/Profile/ChangePicture")]
        public async Task<IActionResult> ChangePicture()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost("/Profile/UploadPicture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPicture(IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                if (profilePicture.Length > 2097152)
                {
                    ModelState.AddModelError(string.Empty, "Снимката не трябва да надвишава 2MB.");
                    var currentUser = await _userManager.GetUserAsync(User);
                    return View("ChangePicture", currentUser);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    string base64String = Convert.ToBase64String(imageBytes);

                    string contentType = profilePicture.ContentType;

                    user.ProfilePictureUrl = $"data:{contentType};base64,{base64String}";

                    await _userManager.UpdateAsync(user);
                }
            }

            return Redirect("/Profile/ChangePicture");
        }
    }
}