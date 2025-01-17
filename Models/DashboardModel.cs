using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using Supabase.Gotrue;
using Supabase.Postgrest.Responses;

namespace laminas_calisa.Models;

public abstract class DashboardModel(ILogger<DashboardModel> logger, Supabase.Client client) : BasePageModel
{
    public List<DashboardOrders> DashboardOrders { get; set; } = [];
    public string[] UnwantedProperties = ["BaseUrl", "RequestClientOptions", "TableName", "PrimaryKey"];
    public string? UserFullName { get; set; } = string.Empty;
    public string? Role { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
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
    
    public abstract Task SetOrders();

    protected virtual void CreateDashboardOrders(ModeledResponse<Order> orders, ModeledResponse<Client> clients, ModeledResponse<Profile> profiles)
    {
        this.DashboardOrders = orders.Models.Select(order => new DashboardOrders()
            {
                Id = order.Id,
                Cliente = clients.Models.Where(c => c.Id == order.ClientId)
                    .Select(c => c.FirstName + " " + c.LastName)
                    .First(),
                Alias = clients.Models.Where(c => c.Id == order.ClientId)
                    .Select(c => c.Alias)
                    .FirstOrDefault(),
                Tipo = order.Type,
                Acabado = order.Finish,
                Calibre = order.Caliber,
                Largo = order.AreFt ? $"{order.Length} ft" : $"{order.Length} ML",
                Cantidad = order.Quantity,
                Peso = $"{order.Kg} kg",
                Descripcion = order.Description,
                Terminado = order.Done ? "Sí" : "No",
                Pagado = order.Paid ? "Sí" : "No",
                Usuario = profiles.Models.Where(p => p.TableId == order.UserId)
                    .Select(p => p.FirstName + " " + p.LastName)
                    .First(),
                TipoDePago = order.IsCredit ? "Crédito" : "Contado",
                Precio = order.Price
            })
            .ToList();
    }
}