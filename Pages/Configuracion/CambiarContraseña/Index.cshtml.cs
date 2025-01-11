using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;

using Models;

namespace laminas_calisa.Pages;

public class ChangePasswordIndexModel : BasePageModel
{
    private readonly ILogger<ChangePasswordIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public ChangePasswordIndexModel(ILogger<ChangePasswordIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try {
            var session = await _supabase.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed sign in attempt at {DateTime.UtcNow} with error {ex.Message}");
            return RedirectToPage("/");
        }

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