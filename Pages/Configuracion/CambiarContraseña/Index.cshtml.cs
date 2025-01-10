using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class ChangePasswordIndexModel : PageModel
{
    private readonly ILogger<ChangePasswordIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public ChangePasswordIndexModel(ILogger<ChangePasswordIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}