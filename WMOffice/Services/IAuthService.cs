using System.Threading.Tasks;
using Supabase.Gotrue;

namespace WMOffice.Services
{
    public interface IAuthService
    {
        Task<Session?> SignInAsync(string email, string password);
        Task SignOutAsync();
        User? GetCurrentUser();
        bool IsAuthenticated { get; }
    }
}
