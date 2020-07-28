using System.Security.Claims;
using System.Security.Principal;

namespace ML.Extensions
{
    
    public static class IdentityExtensions
    {
        public static string Getusern(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("usern");
            
            return (claim != null) ? claim.Value : string.Empty;
        }
        
    }
}