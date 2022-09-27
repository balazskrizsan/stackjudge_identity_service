using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Stackjudge_Identity_Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddControllersWithViews();
        services.AddCors();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseCors(x => x.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

        // Hack until com.auth0.jwk.UrlJwkProvider.WELL_KNOWN_JWKS_PATH hardcoded path won't be load from config

        app.UseIdentityServer();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        InitializeDatabase(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            endpoints.MapControllerRoute(
                name: "login",
                pattern: "{controller}/{action}"
            );
        });
    }

    private void InitializeDatabase(IApplicationBuilder app)
    {
        // Run only on db setup
        // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        // {
        //     serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        //     serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
        //     serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
        //
        //     var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        //     context.Database.Migrate();
        //     if (!context.Clients.Any())
        //     {
        //         foreach (var client in Config.Clients)
        //         {
        //             context.Clients.Add(client.ToEntity());
        //         }
        //     
        //         context.SaveChanges();
        //     }
        //
        //     if (!context.IdentityResources.Any())
        //     {
        //         foreach (var resource in Config.IdentityResources)
        //         {
        //             context.IdentityResources.Add(resource.ToEntity());
        //         }
        //         context.SaveChanges();
        //     }
        //
        //     if (!context.ApiResources.Any())
        //     {
        //         foreach (var resource in Config.ApiResources)
        //         {
        //             context.ApiResources.Add(resource.ToEntity());
        //         }
        //         context.SaveChanges();
        //     }
        // }
    }
}