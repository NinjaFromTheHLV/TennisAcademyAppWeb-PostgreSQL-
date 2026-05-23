using TennisAcademyApp.Models;
using TennisAcademyApp.ViewModels.Wheel;

namespace TennisAcademyApp.Services.Contracts
{
    public interface IWheelService
    {
        Task<(string RacketName, string BallName, string BagName)> GetWheelItemsAsync();
        Task<WheelSpinResultViewModel> SpinWheelAsync(string userId);
        Task<(bool CanSpin, DateTime? NextSpinDate)> GetUserEligibilityAsync(string userId);
    }
}