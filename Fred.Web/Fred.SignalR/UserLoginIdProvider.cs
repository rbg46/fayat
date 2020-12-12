using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNet.SignalR;

namespace Fred.SignalR
{
    public class UserLoginIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            Claim identifierClaim = GetIdentifierClaim();
            string userId = identifierClaim?.Value;

            return userId;

            Claim GetIdentifierClaim()
            {
                var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
                var nameIdentifierClaim = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                return nameIdentifierClaim;
            }
        }
    }
}
