using Microsoft.AspNetCore.Mvc;
using NToastNotify;

using laminas_calisa.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace laminas_calisa.Pages.Configuracion.CerrarSesion
{
    public class LogoutIndexModel : BasePageModel
    {
        private readonly ILogger<LogoutIndexModel> _logger;
        private readonly Supabase.Client _supabase;
        private readonly IToastNotification _toastNotification;
        public LogoutIndexModel(ILogger<LogoutIndexModel> logger, Supabase.Client client, IToastNotification toastNotification)
        {
            _logger = logger;
            _supabase = client;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await _supabase.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
                string? email = _supabase.Auth.CurrentSession.User.Email;
                if (email == null)
                {
                    _toastNotification.AddErrorToastMessage("No se ha podido cerrar la sesión. Intente de nuevo.");
                    return RedirectToPage("/IniciarSesion/Index");
                }
                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await _supabase.Auth.SignOut();

                _logger.LogInformation($"Successful sign out at {DateTime.UtcNow} with email {email}");
                _toastNotification.AddSuccessToastMessage("Ha cerrado sesión correctamente.");
                return RedirectToPage("/IniciarSesion/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed signed out attempt at {DateTime.UtcNow} with error {ex.Message}");
                _toastNotification.AddErrorToastMessage("Ha ocurrido un error.");
                return RedirectToPage("/Dashboard/Index");
            }
        }
    }
}