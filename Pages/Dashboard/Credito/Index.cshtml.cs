using laminas_calisa.Models;

namespace laminas_calisa.Pages.Dashboard.Credito
{
    public class CreditoIndexModel : BasePageModel
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
}