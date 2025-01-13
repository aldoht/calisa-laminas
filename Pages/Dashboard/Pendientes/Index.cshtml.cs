using laminas_calisa.Models;

namespace laminas_calisa.Pages.Dashboard.Pendientes
{
    public class PendientesIndexModel : BasePageModel
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
}