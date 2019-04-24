using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client1 = new HttpClient();
            var disco1 = await client1.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco1.IsError)
            {
                Console.WriteLine(disco1.Error);
                return;
            }
            var tokenResponse1 = await client1.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco1.TokenEndpoint,
                ClientId = "js",

                UserName = "alice",
                Password = "password",
                Scope = "api1"
            });

            if (tokenResponse1.IsError)
            {
                Console.WriteLine(tokenResponse1.Error);
                return;
            }

            Console.WriteLine(tokenResponse1.Json);
            client1.SetBearerToken(tokenResponse1.AccessToken);

            var response1 = await client1.GetAsync("http://localhost:5001/identity");
            if (!response1.IsSuccessStatusCode)
            {
                Console.WriteLine(response1.StatusCode);
            }
            else
            {
                var content = await response1.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            if (disco1.IsError)
            {
                Console.WriteLine(disco1.Error);
                return;
            }

        }
    }
}
