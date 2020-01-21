using IdentityServer4.Models;
using System.Collections.Generic;

namespace Bank.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResourses()
        {
            return new List<ApiResource>
            {
                new ApiResource("bankApi", "Customer Api for Bank")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankApi" }
                }
            };
        }
    }
}
