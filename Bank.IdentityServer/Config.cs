using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace Bank.IdentityServer
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                  SubjectId = "1" ,
                  Username = "John",
                  Password = "password",
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Bob",
                    Password = "password"
                }
            };
        }

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
                // Client credential based grant type
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankApi" }
                },

                // Resource owner password type grant
                 new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "bankApi" },
                }
            };
        }
    }
}
