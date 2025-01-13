using laminas_calisa.Models;

namespace laminas_calisa.Pages.Configuracion.Impresion
{
    public class ImpresionIndexModel : BasePageModel
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
}