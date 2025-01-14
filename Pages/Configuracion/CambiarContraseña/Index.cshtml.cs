using System.Text.RegularExpressions;

using laminas_calisa.Models;

using Microsoft.AspNetCore.Mvc;

using NToastNotify;

using Supabase.Gotrue;

namespace laminas_calisa.Pages.Configuracion.CambiarContraseña
{
    public class ChangePasswordIndexModel : BasePageModel
    {
        private readonly ILogger<ChangePasswordIndexModel> _logger;
        private readonly Supabase.Client _supabase;
        private readonly IToastNotification _toastNotification;
        public ChangePasswordIndexModel(ILogger<ChangePasswordIndexModel> logger, Supabase.Client client, IToastNotification toastNotification)
        {
            _logger = logger;
            _supabase = client;
            _toastNotification = toastNotification;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string password = Request.Form["newPwd"].ToString() ?? "";
            string checkPassword = Request.Form["checkPwd"].ToString() ?? "";

            if (!Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_&!#,;:]).{10,30}$").Success) return Page();
            if (!checkPassword.Equals(password)) return Page();

            try
            {
                await _supabase.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);

                if (_supabase.Auth.CurrentUser == null)
                {
                    _toastNotification.AddErrorToastMessage("No se pudo asegurar la sesión actual.");
                    return Page();
                }
                
                var attrs = new UserAttributes { Password = password };
                var user = await _supabase.Auth.Update(attrs);

                _logger.LogInformation($"Successful password change at {DateTime.UtcNow} with email {user!.Email}");
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed password change attempt at {DateTime.UtcNow} with error {ex.Message}");
                return Page();
            }
        }
    }
}