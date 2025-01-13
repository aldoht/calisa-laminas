using Microsoft.AspNetCore.Mvc.RazorPages;

namespace laminas_calisa.Pages.Configuracion.CerrarSesion
{
    public class LogoutIndexModel : PageModel
    {
        private readonly ILogger<LogoutIndexModel> _logger;
        private readonly Supabase.Client _supabase;
        public LogoutIndexModel(ILogger<LogoutIndexModel> logger, Supabase.Client client)
        {
            _logger = logger;
            _supabase = client;
        }

        public void OnGet()
        {
            //
        }
    }
}