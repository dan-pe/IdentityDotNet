using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Bank.ConsoleClient
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            await ResourcePassowrdAuth();

            await ClientCredentialsAuth();

            Console.ReadKey();
        }

        private static async Task ResourcePassowrdAuth()
        {
            var client = new HttpClient();
            var discoveryRO = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            var response = await client.RequestTokenAsync(new TokenRequest
            {
                Address = discoveryRO.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                Parameters =
                {
                    { "scope", "bankApi" },
                    { "grant_type", "password" },
                    { "username", "John" },
                    { "password", "password" }
                }
            });

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                return;
            }

            Console.WriteLine($"Password based token: {response.AccessToken}");
        }

        private static async Task ClientCredentialsAuth()
        {
            // Discover endpoints using metadata of Identity Server
            var discoveryClient = new HttpClient();

            var discovery = await discoveryClient.GetDiscoveryDocumentAsync("http://localhost:5000");

            if (discovery.IsError)
            {
                throw new HttpRequestException($"Identity Server discovery failed: {discovery.Error}");
            }

            var tokenResponse = await discoveryClient.RequestTokenAsync(new TokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Parameters =
                {
                    { "scope", "bankApi" },
                    { "grant_type", "client_credentials" }
                }
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // Consumer Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                        new { Id = 1, FirstName = "ApiClientName", LastName = "LastNameOfApiClient" }),
                        Encoding.UTF8, "application/json");

            var createCustomerReponse = await client.PostAsync("https://localhost:44379/api/customers", customerInfo);

            if (!createCustomerReponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerReponse.StatusCode);
            }

            var getAllCustomersReponse = await client.GetAsync("https://localhost:44379/api/customers");
            var content = await getAllCustomersReponse.Content.ReadAsStringAsync();

            Console.WriteLine(JArray.Parse(content));
        }
    }
}
