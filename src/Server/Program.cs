﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Synaplic.Inventory.Infrastructure.Contexts;
using Synaplic.Inventory.Server.Extensions;
using System;
using System.Threading.Tasks;
using Synaplic.Inventory.Server.Services;

namespace Synaplic.Inventory.Server
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                try
                {
                     

                    var context = services.GetRequiredService<UniContext>();
                    

                    if (context.Database.IsSqlServer())
                    {
                        context.Database.Migrate();
                    }
                    
                    //var bydContext = services.GetRequiredService<IBydesignService>();
                    //await bydContext.IntegrateFile(10, "PROJECTS_V0508 HackLab.xlsx");
                    //return;

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
    }
}