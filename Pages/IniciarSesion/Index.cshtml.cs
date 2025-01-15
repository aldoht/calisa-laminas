using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using laminas_calisa.Models;

using NToastNotify;

namespace laminas_calisa.Pages.IniciarSesion
{
    public class IniciarSesionIndexModel : BasePageModel
    {
        private readonly ILogger<IniciarSesionIndexModel> _logger;
        private readonly Supabase.Client _supabase;
        private readonly IToastNotification _toastNotification;
        public IniciarSesionIndexModel(ILogger<IniciarSesionIndexModel> logger, Supabase.Client client, IToastNotification toastNotification)
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
                if (_supabase.Auth.CurrentSession == null)
                    throw new ArgumentNullException();
                _logger.LogInformation($"User is logged in.");
                _toastNotification.AddAlertToastMessage("Cierre la sesión antes de acceder al formulario de inicio de sesión.");
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An unauthenticated user has made a get request to the login form.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string email = Request.Form["email"].ToString().ToLower() ?? "";
            string password = Request.Form["pwd"].ToString() ?? "";

            if (!Regex.Match(email, @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$").Success)
            {
                _toastNotification.AddErrorToastMessage("Correo no válido. Intente de nuevo.");
                return Page();
            }
            if (!Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_&!#,;:]).{10,30}$").Success)
            {
                _toastNotification.AddErrorToastMessage("Contraseña no válida. Intente de nuevo.");
                return Page();
            }

            try
            {
                var session = await _supabase.Auth.SignIn(Request.Form["email"]!, Request.Form["pwd"]!);
            
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, session!.User!.Id!),
                    new Claim(ClaimTypes.Email, session.User.Email!),
                    new Claim("SupabaseAccessToken", session.AccessToken!),
                    new Claim("SupabaseRefreshToken", session.RefreshToken!)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"Successful sign in at {DateTime.UtcNow} with email {session.User.Email}");
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed login attempt at {DateTime.UtcNow} with error {ex.Message}");
                _toastNotification.AddErrorToastMessage("El correo o contraseña son incorrectos. Intente de nuevo.");
                return Page();
            }
        }
    }
}