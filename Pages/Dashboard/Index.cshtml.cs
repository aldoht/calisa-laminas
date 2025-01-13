using laminas_calisa.Models;

namespace laminas_calisa.Pages.Dashboard
{
    public class DashboardIndexModel : BasePageModel
    {
        private readonly ILogger<DashboardIndexModel> _logger;
        private readonly Supabase.Client _supabase;
        public DashboardIndexModel(ILogger<DashboardIndexModel> logger, Supabase.Client client)
        {
            _logger = logger;
            _supabase = client;
        }

        public void OnGet()
        {
            Console.WriteLine("se procesó un get a la vista");
        }
    }
}