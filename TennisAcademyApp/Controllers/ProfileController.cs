using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TennisAcademyApp.Data.Models;

namespace TennisAcademyApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }

                user.ProfilePictureUrl = $"/images/users/{uniqueFileName}";
                await _userManager.UpdateAsync(user);
            }

            return Redirect("/Profile/ChangePicture");
        }
    }
}