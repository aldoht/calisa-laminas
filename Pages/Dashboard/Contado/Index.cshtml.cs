using laminas_calisa.Models;

namespace laminas_calisa.Pages.Dashboard.Contado
{
    public class ContadoIndexModel : BasePageModel
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
}