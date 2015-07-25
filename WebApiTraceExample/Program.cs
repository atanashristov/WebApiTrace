using System;

namespace WebApiTraceExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostUrl = "http://localhost:9000";

            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>(hostUrl))
            {
                Console.WriteLine(hostUrl);
                Console.WriteLine("Press [Enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}
