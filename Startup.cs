using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using depot;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace test {
    public class Startup {

        public IConfigurationRoot Configuration { get; set; }

        public Startup (IHostingEnvironment env) {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath)
                .AddJsonFile ("appsettings.json", optional : true, reloadOnChange : true);

            Configuration = builder.Build ();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            // Adds services required for using options.
            services.AddOptions ();
            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<AppOptions> (Configuration);

            services.AddDbContext<DepotContext> (options => {
                options.UseMySql (Configuration.GetConnectionString ("DefaultConnection"));
            });
            /*
                        services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer (options => {
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new TokenValidationParameters {
                                    // укaзывает, будет ли валидироваться издатель при валидации токена
                                    ValidateIssuer = true,
                                    // строка, представляющая издателя
                                    ValidIssuer = AuthOptions.ISSUER,

                                    // будет ли валидироваться потребитель токена
                                    ValidateAudience = false, //true,
                                    // установка потребителя токена
                                    ValidAudience = AuthOptions.AUDIENCE,
                                    // будет ли валидироваться время существования
                                    ValidateLifetime = true,
                                    LifetimeValidator = CustomLifetimeValidator,

                                    // установка ключа безопасности
                                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey (),
                                    // валидация ключа безопасности
                                    ValidateIssuerSigningKey = true,
                                };
                            });
             */
            services.AddAuthorization (options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder (JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser ()
                    .Build ();
            });

            services.AddIdentity<User, Role> ()
                .AddEntityFrameworkStores<DepotContext> ()
                .AddDefaultTokenProviders ();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear (); // => remove default claims
            services
                .AddAuthentication (options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer (cfg => {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters { // укaзывает, будет ли валидироваться издатель при валидации токена
                        ValidateIssuer = true,
                        // строка, представляющая издателя
                        ValidIssuer = AuthOptions.ISSUER,

                        // будет ли валидироваться потребитель токена
                        ValidateAudience = false, //true,
                        // установка потребителя токена
                        ValidAudience = AuthOptions.AUDIENCE,
                        // будет ли валидироваться время существования
                        ValidateLifetime = true,
                        LifetimeValidator = CustomLifetimeValidator,

                        // установка ключа безопасности
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey (),
                        // валидация ключа безопасности
                        ValidateIssuerSigningKey = true,
                    };
                });

            // Adds the services required for building
            services.AddMvc ();

            services.AddScoped<IPartsRepository, PartsRepository> ();
            services.AddScoped<IPartProducerRepository, PartProducerRepository> ();
            services.AddScoped<ICatalogItemRepository, CatalogItemRepository> ();
            services.AddScoped<IPartSupplierRepository, PartSupplierRepository> ();
            services.AddScoped<ISupplierPriceItemRepository, SupplierPriceItemRepository> ();
            services.AddScoped<ISupplierWarehouseRepository, SupplierWarehouseRepository> ();
            services.AddScoped<EnbsvParserService, EnbsvParserService> ();
            services.AddScoped<IModerationService, ModerationService> ();
            services.AddScoped<ISeoRepository, SeoRepository> ();
            services.AddScoped<GazServiceParserService, GazServiceParserService> ();
            services.AddScoped<IStatisticRepository, StatisticRepository> ();
            services.AddScoped<IModerationRepository, ModerationRepository> ();
            services.AddScoped<IProducerCodeService, ProducerCodeService> ();
            services.AddScoped<IPartProducerService, PartProducerService> ();
            services.AddScoped<IPartSupplierService, PartSupplierService> ();

            services.AddScoped<IGoogleDriveService, GoogleDriveService> ();
            services.AddScoped<ISeoService, SeoService> ();
            services.AddScoped<IStatisticService, StatisticService> ();
            services.AddScoped<ICatalogItemStatisticRepository, CatalogItemStatisticRepository> ();
            services.AddScoped<ISupplierOfferFilesRepository, SupplierOfferFilesRepository> ();

            services.AddScoped<ISchedulerWrapper, SchedulerWrapper> ();

            services.AddDirectoryBrowser ();

            /* 
                        services.Configure<FormOptions> (x => {
                            x.ValueLengthLimit = int.MaxValue;
                            x.MultipartBodyLengthLimit = int.MaxValue;
                            x.MultipartHeadersLengthLimit = int.MaxValue;
                        });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            // loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            // loggerFactory.AddDebug();

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseDatabaseErrorPage ();
                app.UseBrowserLink ();
            } else {
                app.UseExceptionHandler ("/Home/Error");
            }

            // Redirect any non-API calls to the Angular application
            // so our application can handle the routing
            //app.Use (async (context, next) => {
            //  await next ();
            /*
            if (context.Response.StatusCode == 404 &&
                !Path.HasExtension (context.Request.Path.Value)) {
                
                                    var isAdmin = context.Request.Path.Value.StartsWith ("/admin");
                                    var isApi = context.Request.Path.Value.StartsWith ("/api");

                                    if (isAdmin) {
                                        context.Request.Path = "/admin_module/index.html";
                                    } else if (!isApi) {
                                        context.Request.Path = "/search_module/index.html";
                                    }
                
                await next ();
            }
             */
            //});

            // Configures application for usage as API
            // with default route of '/api/[Controller]'
            app.UseMvcWithDefaultRoute ();

            // Configures applcation to serve the index.html file from /wwwroot/search_module
            // when you access the server from a web browser
            app.UseDefaultFiles ();
            app.UseStaticFiles ();

            app.UseFileServer (new FileServerOptions () {
                FileProvider = new PhysicalFileProvider (
                        Path.Combine (Directory.GetCurrentDirectory (), @"files")),
                    RequestPath = new PathString ("/files"),
                    EnableDirectoryBrowsing = true
            });

            app.UseForwardedHeaders (new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication ();
            app.UseMvc ();
        }

        public static bool CustomLifetimeValidator (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) {
            if (expires != null) {
                return DateTime.UtcNow < expires;
            }
            return false;
        }
    }
}