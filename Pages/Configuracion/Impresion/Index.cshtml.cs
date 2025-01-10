using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class ImpresionIndexModel : PageModel
{
    private readonly ILogger<ImpresionIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public ImpresionIndexModel(ILogger<ImpresionIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}