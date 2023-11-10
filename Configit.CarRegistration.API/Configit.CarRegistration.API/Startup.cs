using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configit.CarRegistration.API.Data;
using Configit.CarRegistration.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Configit.CarRegistration.API {
  public class Startup {
    public Startup( IConfiguration configuration ) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices( IServiceCollection services ) {
      services.AddControllers();

      services.AddDbContext<CarRegistrationDbContext>( options =>
        options.UseSqlServer( Configuration.GetConnectionString( "Dev" ) )
      );

      services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {
      app.UseCors( options =>
      options.WithOrigins( "http://localhost:3000" )
      .AllowAnyHeader()
      .AllowAnyMethod()
      );
      
      if ( env.IsDevelopment() ) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints( endpoints => {
        endpoints.MapControllers();
      } );

      using ( var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope() ) {
        var context = serviceScope.ServiceProvider.GetRequiredService<CarRegistrationDbContext>();
        context.Database.EnsureCreated();
      }
    }
  }
}
