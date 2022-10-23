using System.Security.Claims;
using System.Security.Principal;

namespace Server.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetId(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return 0;

            return int.Parse(claim.Value);
        }
    }
}