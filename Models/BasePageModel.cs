using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Models;

public abstract class BasePageModel : PageModel
{
    public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string UserEmail => User.FindFirstValue(ClaimTypes.Email);
    public string SupabaseAccessToken => User.FindFirstValue("SupabaseAccessToken");
    public string SupabaseRefreshToken => User.FindFirstValue("SupabaseRefreshToken");
    
    public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
}