using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class CreditoIndexModel : PageModel
{
    private readonly ILogger<CreditoIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public CreditoIndexModel(ILogger<CreditoIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}