using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Infrastructure.Data;
using Ardalis.SampleApp.Infrastructure.DataAccess;
using Ardalis.SampleApp.Web.Interfaces;
using Ardalis.SampleApp.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ardalis.SampleApp.Web;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddDbContext<SampleDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));

    services.AddAutoMapper(typeof(AutomapperMaps));

    services.AddScoped(typeof(IReadRepository<>), typeof(CachedRepository<>));
    services.AddScoped(typeof(MyRepository<>));
    services.AddScoped<ICustomerUiService, CustomerUiService>();

    //            services.AddControllers();
    services.AddMvc();
    services.AddLogging();
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
      endpoints.MapRazorPages();
    });
  }
}
