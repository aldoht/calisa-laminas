using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Models;

namespace laminas_calisa.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Supabase.Client _supabase;
    public IndexModel(ILogger<IndexModel> logger, Supabase.Client client)
    {
        _logger = logger;
        _supabase = client;
    }

    public async void OnGet()
    {
        //
    }
}
