using TravelPlanner.Domain.DTOs.Auth;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
