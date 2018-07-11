using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oboete.Database;
using Oboete.Logic;
using Oboete.Web.API;
using System.Text;

namespace Oboete.Web
{
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
            var connectionString = Configuration.GetConnectionString("OboeteDB");
            services.AddDbContext<OboeteContext>(options => options.UseSqlServer(connectionString));
            
            // Setup Logic Layer Config
            Config.ConnectionString = connectionString;
            Config.SecretKey = Configuration["SecretKey"];
            Config.Issuer = Configuration["Issuer"];
            Config.Audience = Configuration["Audience"];
            Config.GlobalHashSalt = Configuration["PasswordSalt"];

            services.AddAuthentication(JwtAuthenticationOptions.DefaultScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.SecretKey)),
                        RequireSignedTokens = true,

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true,
                        ValidAudience = Config.Audience,

                        ValidateIssuer = true,
                        ValidIssuer = Config.Issuer
                    };
                })
                .UseCustomJwtAuthentication(o => { });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Vue}/{action=Index}"
                );
                
                routes.MapSpaFallbackRoute(
                    name: "SPA-Fallback",
                    defaults: new { controller = "Vue", action = "Index" }
                );
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
                serviceScope.ServiceProvider.GetService<OboeteContext>().Database.Migrate();
            }
        }
    }
}
