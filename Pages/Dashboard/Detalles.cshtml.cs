using laminas_calisa.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Client = laminas_calisa.Models.Client;

namespace laminas_calisa.Pages.Dashboard
{
    public class DashboardDetailsModel : BasePageModel
    {
        private readonly ILogger<DashboardDetailsModel> _logger;
        private readonly Supabase.Client _supabase;
        public List<DashboardOrdersDetailed> DashboardOrdersDetailed { get; set; }
        public string[] UnwantedProperties = ["BaseUrl", "RequestClientOptions", "TableName", "PrimaryKey"];
        public DashboardDetailsModel(ILogger<DashboardDetailsModel> logger, Supabase.Client client)
        {
            _logger = logger;
            _supabase = client;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await _supabase.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
                if (_supabase.Auth.CurrentSession == null)
                    throw new ArgumentNullException(nameof(_supabase.Auth.CurrentSession));
                _logger.LogInformation($"User with email {_supabase.Auth.CurrentUser!.Email} accessed dashboard correctly at {DateTime.UtcNow}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not validate user session in dashboard at {DateTime.UtcNow}.");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/IniciarSesion/Index");
            }

            DashboardOrdersDetailed = await GetDetailedOrders();
            
            return Page();
        }
        
        public async Task<List<DashboardOrdersDetailed>> GetDetailedOrders()
        {
            try
            {
                var ordersResults = await _supabase.From<Order>().Get();
                var clientsResults = await _supabase.From<Client>().Get();
                var profilesResults = await _supabase.From<Profile>().Get();
                
                List<DashboardOrdersDetailed> dashboardOrders = new List<DashboardOrdersDetailed>();
                foreach (var order in ordersResults.Models)
                {
                    dashboardOrders.Add(new DashboardOrdersDetailed()
                    {
                        Id = order.Id,
                        Cliente = clientsResults.Models
                            .Where(c => c.Id == order.ClientId)
                            .Select(c => c.FirstName + " " + c.LastName)
                            .First(),
                        Alias = clientsResults.Models
                            .Where(c => c.Id == order.ClientId)
                            .Select(c => c.Alias)
                            .FirstOrDefault(),
                        Notas = clientsResults.Models
                            .Where(c => c.Id == order.ClientId)
                            .Select(c => c.Notes)
                            .FirstOrDefault(),
                        Email = clientsResults.Models
                            .Where(c => c.Id == order.ClientId)
                            .Select(c => c.Email)
                            .FirstOrDefault(),
                        Telefono = clientsResults.Models
                            .Where(c => c.Id == order.ClientId)
                            .Select(c => c.PhoneNumber)
                            .FirstOrDefault(),
                        Tipo = order.Type,
                        Acabado = order.Finish,
                        Calibre = order.Caliber,
                        Largo = order.AreFt ? $"{order.Length} ft" : $"{order.Length} ML",
                        Cantidad = order.Quantity,
                        Peso = $"{order.Kg} kg",
                        Descripcion = order.Description,
                        Terminado = order.Done ? "Sí" : "No",
                        FechaTerminado = order.DoneAt?.Date.ToString("YYYY-MM-dd"),
                        Pagado = order.Paid ? "Sí" : "No",
                        FechaPagado = order.PaidAt?.Date.ToString("YYYY-MM-dd"),
                        Usuario = profilesResults.Models
                            .Where(p => p.TableId == order.UserId)
                            .Select(p => p.FirstName + " " + p.LastName)
                            .First()
                    });
                }

                return dashboardOrders;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not get full orders from dashboard at {DateTime.UtcNow}.");
                return [];
            }
        }
    }
}