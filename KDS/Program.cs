using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KDS.Components;
using KDS.Components.Account;
using KDS.Data;
using MudBlazor.Services;
using KDS.Components.Donations;
using KDS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using TwitchLib.Api;

namespace KDS;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

        var twitchApi = new TwitchAPI
        {
            Settings =
            {
                ClientId = builder.Configuration["Authentication:Twitch:ClientId"] ??
                           throw new InvalidOperationException("Twitch ClientId not found.")
            }
        };

        //Add application services to the container.
        builder.Services.AddSingleton(twitchApi);
        builder.Services.AddSingleton<TwitchAuthService>();
        builder.Services.AddSingleton<DonationService>();
        builder.Services.AddSingleton<ApiAuthService>();
        
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        builder.Services.AddMudServices();
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddTwitch("Twitch",config =>
            {
                config.UsePkce = true;
                config.SaveTokens = true;
                config.AccessDeniedPath = "/AccessDenied";
                config.ClaimsIssuer = "Twitch";
                
                config.Scope.Add("channel:manage:redemptions");
                
                config.ClientId = builder.Configuration["Authentication:Twitch:ClientId"] ??
                                  throw new InvalidOperationException("Twitch ClientId not found.");
                
                config.ClientSecret = builder.Configuration["Authentication:Twitch:ClientSecret"] ?? 
                                      throw new InvalidOperationException("Twitch ClientSecret not found.");
            })
            .AddTwitch("TwitchElevated",config =>
            {
                config.UsePkce = true;
                config.SaveTokens = true;
                
                config.AccessDeniedPath = "/AccessDenied";
                config.CallbackPath = "/signin-twitch-elevated";
                config.ClaimsIssuer = "Twitch";
                
                config.Scope.Add("channel:manage:redemptions");
                config.Scope.Add("channel:read:vips");
                config.Scope.Add("moderation:read");
                
                config.ClientId = builder.Configuration["Authentication:Twitch:ClientId"] ??
                                  throw new InvalidOperationException("Twitch ClientId not found.");
                
                config.ClientSecret = builder.Configuration["Authentication:Twitch:ClientSecret"] ?? 
                                      throw new InvalidOperationException("Twitch ClientSecret not found.");
            })
            .AddIdentityCookies();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
            
            if (builder.Environment.IsDevelopment())
                options.EnableDetailedErrors();
        });
        
        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
            Console.WriteLine("Development mode");
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();
        app.MapAdditionalDonationEndpoints();

        app.Run();
    }
}