using System;
using System.Threading.Tasks;
using Ardalis.SampleApp.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ardalis.SampleApp.Web;

public class Program
{
  public static async Task Main(string[] args)
  {
    var host = CreateHostBuilder(args).Build();

    using (var scope = host.Services.CreateScope())
    {
      var services = scope.ServiceProvider;
      try
      {
        var dbContext = services.GetRequiredService<SampleDbContext>();

        await new SampleDbContextSeed(dbContext).SeedAsync();
      }
      catch (Exception)
      {
      }
    }

    host.Run();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
            webBuilder.ConfigureLogging(config =>
                  config.AddConsole());
            webBuilder.UseStartup<Startup>();
          });
}
