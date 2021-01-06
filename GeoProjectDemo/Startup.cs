using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using GeoProjectDemo.Services;
using Microsoft.JSInterop;
using GeoProjectDemo.Helpers;
using BaseClasses;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Linq;

namespace GeoProjectDemo
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllers( );

            services.AddRazorPages( );
            services.AddTelerikBlazor( );
            services.AddServerSideBlazor( );
            services.AddSingleton<SessionService>( );
            services.AddSingleton<KompetenciaService>( );
            services.AddSingleton<FoglalasService>( );

            services.AddHttpContextAccessor( );
            services.AddAuthorization( );
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>( );

            Globals.Globals.SaveServiceOptions(
                File.ReadAllText( Path.Combine( Environment.CurrentDirectory, "appsettings.json" ) ) );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {

            if ( env.IsDevelopment( ) )
            {
                app.UseDeveloperExceptionPage( );
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts( );
            }

            app.UseHttpsRedirection( );
            app.UseStaticFiles( );

            app.UseRouting( );

            app.UseAuthentication( );
            app.UseAuthorization( );

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllers( );
                 endpoints.MapBlazorHub( );
                 endpoints.MapFallbackToPage( "/_Host" );
             } );

        }
    }
}
