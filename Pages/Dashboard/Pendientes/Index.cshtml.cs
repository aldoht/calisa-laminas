using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class PendientesIndexModel : PageModel
{
    private readonly ILogger<PendientesIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public PendientesIndexModel(ILogger<PendientesIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}