using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Identity
{
    public class Global
    {
        public static async Task SetExternalProperties(ClaimsIdentity identity, ClaimsIdentity ext)
        {
            if (ext != null)
            {
                const string ignoreClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";

                // Adicionando Claims Externos no Identity
                foreach (var c in ext.Claims)
                {
                    if (!c.Type.StartsWith(ignoreClaim))
                        if (!identity.HasClaim(c.Type, c.Value))
                            identity.AddClaim(c);
                }
            }
        }
    }
}
