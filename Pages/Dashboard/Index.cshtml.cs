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
    }
}