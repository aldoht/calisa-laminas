using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages;

public class ContadoIndexModel : PageModel
{
    private readonly ILogger<ContadoIndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public ContadoIndexModel(ILogger<ContadoIndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public void OnGet()
    {
        //
    }
}