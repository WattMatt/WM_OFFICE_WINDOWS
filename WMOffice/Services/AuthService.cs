using System;
using System.Threading.Tasks;
using Supabase.Gotrue;
using Supabase.Client;

namespace WMOffice.Services
{
    public class AuthService : IAuthService
    {
        private static AuthService? _instance;
        private Supabase.Client _client;

        public static AuthService Instance => _instance ??= new AuthService();

        // TODO: Replace with your actual Supabase credentials
        private const string SUPABASE_URL = "https://xyzcompany.supabase.co";
        private const string SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

        private AuthService()
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
            };
            
            // Note: InitializeAsync is usually async, so calling Wait() here is a bit risky in UI thread constructor.
            // A better approach would be to have an Initialize method.
            _client = new Supabase.Client(SUPABASE_URL, SUPABASE_KEY, options);
        }

        private bool _initialized = false;

        public async Task InitializeAsync()
        {
            if (!_initialized)
            {
                await _client.InitializeAsync();
                _initialized = true;
            }
        }

        public async Task<Session?> SignInAsync(string email, string password)
        {
            try
            {
                await InitializeAsync();
                var session = await _client.Auth.SignIn(email, password);
                return session;
            }
            catch (Exception ex)
            {
                // Handle or log exception
                System.Diagnostics.Debug.WriteLine($"Sign in failed: {ex.Message}");
                throw;
            }
        }

        public async Task SignOutAsync()
        {
            await _client.Auth.SignOut();
        }

        public User? GetCurrentUser()
        {
            return _client.Auth.CurrentUser;
        }

        public bool IsAuthenticated => _client.Auth.CurrentUser != null;
    }
}
