using Autofac;
using Autofac.Extensions.DependencyInjection;
using BookingAPI.Producers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RawRabbit.DependencyInjection.Autofac;

namespace BookingAPI
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    private IConfigurationRoot Configuration { get; }
    private ILifetimeScope AutofacContainer { get; set; }
    
    public static void ConfigureServices(IServiceCollection services)
    {
      services.AddOptions();
      services.AddControllers();
      services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "BookingAPI", Version = "v1"}); });
    }
    
    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.RegisterType<BookingProducer>();
      builder.RegisterRawRabbit("guest:guest@localhost:5672/");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      AutofacContainer = app.ApplicationServices.GetAutofacRoot();
      
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingAPI v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}