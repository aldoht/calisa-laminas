using laminas_calisa.Models;

using Supabase.Postgrest.Responses;

namespace laminas_calisa.Pages.Dashboard
{
    public class DashboardDetailsModel(ILogger<DashboardDetailsModel> logger, Supabase.Client client)
        : DashboardModel(logger, client)
    {
        public List<DashboardOrdersDetailed> DashboardOrdersDetailed { get; set; } = [];
        public override async Task SetOrders()
        {
            try
            {
                var ordersResults = await client.From<Order>().Get();
                var clientsResults = await client.From<Client>().Get();
                var profilesResults = await client.From<Profile>().Get();

                CreateDashboardOrders(ordersResults, clientsResults, profilesResults);
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not get orders from dashboard at {DateTime.UtcNow}.");
                this.DashboardOrders = [];
            }
        }
        
        protected override void CreateDashboardOrders(ModeledResponse<Order> orders, ModeledResponse<Client> clients, ModeledResponse<Profile> profiles)
        {
            this.DashboardOrdersDetailed = orders.Models.Select(order => new DashboardOrdersDetailed()
                {
                    Id = order.Id,
                    Cliente = clients.Models
                        .Where(c => c.Id == order.ClientId)
                        .Select(c => c.FirstName + " " + c.LastName)
                        .First(),
                    Alias = clients.Models
                        .Where(c => c.Id == order.ClientId)
                        .Select(c => c.Alias)
                        .FirstOrDefault(),
                    Notas = clients.Models
                        .Where(c => c.Id == order.ClientId)
                        .Select(c => c.Notes)
                        .FirstOrDefault(),
                    Email = clients.Models
                        .Where(c => c.Id == order.ClientId)
                        .Select(c => c.Email)
                        .FirstOrDefault(),
                    Telefono = clients.Models
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
                    FechaTerminado = order.DoneAt?.Date.ToString("yyyy-MM-dd"),
                    Pagado = order.Paid ? "Sí" : "No",
                    FechaPagado = order.PaidAt?.Date.ToString("yyyy-MM-dd"),
                    Usuario = profiles.Models
                        .Where(p => p.TableId == order.UserId)
                        .Select(p => p.FirstName + " " + p.LastName)
                        .First(),
                    TipoDePago = order.IsCredit ? "Crédito" : "Contado"
                })
                .ToList();
        }
    }
}