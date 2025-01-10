using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class IniciarSesionIndexModel : PageModel
{
    private readonly ILogger<IniciarSesionIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public IniciarSesionIndexModel(ILogger<IniciarSesionIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}