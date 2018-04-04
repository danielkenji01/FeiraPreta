using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FeiraPreta
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();

            Thread thread = new Thread(search);

            thread.Start();

            Thread.Sleep(TimeSpan.FromMinutes(30));
        }

        static void search()
        {
            new Features.Publication.AutomaticSearch.Command();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
