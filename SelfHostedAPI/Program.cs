using System;
using System.Web.Http;
using System.Web.Http.SelfHost; 

namespace SelfHostedAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8082");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Server started....");
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
