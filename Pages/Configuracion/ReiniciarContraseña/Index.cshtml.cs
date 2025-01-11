using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;

using Models;

namespace laminas_calisa.Pages;

public class ResetPasswordIndexModel : BasePageModel
{
    private readonly ILogger<ResetPasswordIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public ResetPasswordIndexModel(ILogger<ResetPasswordIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        string email = Request.Form["email"].ToString() ?? "";

        if (!Regex.Match(email, @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$").Success) return Page();

        try
        {
            await _supabase.Auth.ResetPasswordForEmail(email);

            _logger.LogInformation($"Successful password reset request at {DateTime.UtcNow} with email {email}");
            return RedirectToPage("/IniciarSesion/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed password reset request at {DateTime.UtcNow} with error {ex.Message}");
            return Page();
        }
    }
}