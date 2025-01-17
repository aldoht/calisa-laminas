using laminas_calisa.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Client = laminas_calisa.Models.Client;

namespace laminas_calisa.Pages.Dashboard
{
    public class DashboardIndexModel(ILogger<DashboardIndexModel> logger, Supabase.Client client)
        : DashboardModel(logger, client)
    {
        public string? UserFullName { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public List<int> DashboardOrderIds { get; set; } = [];
        public List<string> OrderTypes { get; } = ["R-101", "R-85"];
        public List<string> OrderFinishes { get; } = ["Galvanizada", "Pintro Blanco", "Pintro Rojo", "Pintro Negro", "Pintro Gris"];
        [BindProperty]
        public Order NewOrder { get; set; } = new Order();
        
        public override async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await client.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
                if (client.Auth.CurrentSession == null)
                    throw new ArgumentNullException(nameof(client.Auth.CurrentSession));

                var results = await client.From<Profile>()
                    .Where(p => p.Id == client.Auth.CurrentUser!.Id)
                    .Get();

                UserFullName = results.Models
                    .Select(p => $"{p.FirstName} {p.LastName}")
                    .FirstOrDefault();

                Role = results.Models
                    .Select(p => p.Role)
                    .FirstOrDefault();

                var allRecords = await client.From<Order>().Get();
                DashboardOrderIds = allRecords.Models
                    .Select(o => o.Id)
                    .ToList();
                DashboardOrderIds.Sort();
                
                if (UserFullName == null || Role == null)
                    throw new ArgumentNullException("User's full name or role is null");
            
                logger.LogInformation($"User with email {client.Auth.CurrentUser!.Email} accessed dashboard correctly at {DateTime.UtcNow}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not validate user session in dashboard at {DateTime.UtcNow}.");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/IniciarSesion/Index");
            }

            await SetOrders();
            return Page();
        }
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

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await client.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
                if (client.Auth.CurrentSession == null)
                    throw new ArgumentNullException(nameof(client.Auth.CurrentSession));

                await client.From<Order>()
                    .Where(o => o.Id == id)
                    .Delete();
                logger.LogInformation($"Admin {client.Auth.CurrentUser!.Email} has deleted order {id}.");
                return RedirectToPage("/Dashboard/Index");
            }
            catch
            {
                logger.LogError($"Could not delete order at {DateTime.UtcNow}.");
                return RedirectToPage("/Dashboard/Index");
            }
        }
        
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                await client.Auth.SetSession(SupabaseAccessToken, SupabaseRefreshToken);
                if (client.Auth.CurrentSession == null)
                    throw new ArgumentNullException(nameof(client.Auth.CurrentSession));
                
                logger.LogInformation($"Es la id {NewOrder.Id}");
                var model = await client.From<Order>()
                    .Where(o => o.Id == NewOrder.Id)
                    .Single();
                if (model == null) throw new ArgumentNullException(nameof(model));

                if (!model.Paid && NewOrder.Paid) model.PaidAt = DateTimeOffset.UtcNow;
                if (!model.Done && NewOrder.Done) model.DoneAt = DateTimeOffset.UtcNow;
                foreach (var property in typeof(Order).GetProperties())
                {
                    if ((property.Name is "Id" or "ClientId" or "UserId" or "CreatedAt" or "ModifiedAt" or "PaidAt"
                            or "done_at") || UnwantedProperties.Contains(property.Name)) continue;
                    property.SetValue(model, property.GetValue(NewOrder));
                }
                model.ModifiedAt = DateTimeOffset.UtcNow;

                await model.Update<Order>();
                
                logger.LogInformation($"User {client.Auth.CurrentUser!.Email} has updated order {NewOrder.Id}.");
                return RedirectToPage("/Dashboard/Index");
            }
            catch (Exception e)
            {
                logger.LogError($"Could not update order at {DateTime.UtcNow} {e.Message}.");
                return RedirectToPage("/Dashboard/Index");
            }
        }
    }
}