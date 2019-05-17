using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using WebApi.Domain;

namespace WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = BuildWebHost(args);

      // ensure seed data
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        try
        {
          var db = services.GetRequiredService<IEnsureDatabaseService>();
          db.EnsureSeedData();
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while seeding the database.");
        }
      }

      host.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      new WebHostBuilder()
        .UseConfiguration(new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile(
            "appsettings.json",
            optional: true,
            reloadOnChange: true)
          .Build())
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseKestrel()
        .UseShutdownTimeout(TimeSpan.FromSeconds(10))
        .UseStartup<Startup>()
        .UseSerilog()
        .Build();
  }
}
