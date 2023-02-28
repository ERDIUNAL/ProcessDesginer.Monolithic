using System.Security.Claims;

namespace Crea.Core.Security.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal?.FindAll(claimType).Select(x => x.Value).ToList();
    }

    public static List<string> ClaimsRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Role);
    }

    public static int ClaimsNameIdentifier(this ClaimsPrincipal claimsPrincipal)
    {
        return Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier).FirstOrDefault());
    }

    public static string ClaimsEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Email).FirstOrDefault().ToString();
    }

    public static string ClaimsName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal?.Claims(ClaimTypes.Name).FirstOrDefault().ToString();
    }
}