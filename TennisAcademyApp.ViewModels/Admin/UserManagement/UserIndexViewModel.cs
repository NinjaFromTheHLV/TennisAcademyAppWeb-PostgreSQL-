namespace TennisAcademyApp.ViewModels.Admin.UserManagement
{
    public class UserIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public IEnumerable<string> Roles { get; set; } = null!;
        public IEnumerable<string> AllExistingRoles { get; set; } = new List<string>();
    }
}
