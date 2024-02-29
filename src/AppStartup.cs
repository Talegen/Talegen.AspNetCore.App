/*
 * Talegen ASP.net Core App Library
 * (c) Copyright Talegen, LLC.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/
namespace Talegen.AspNetCore.App
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Reflection;
    using System.Runtime.Serialization;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Serilog;
    using Talegen.AspNetCore.App.Converters;
    using Talegen.AspNetCore.App.Filters;
    using Talegen.AspNetCore.App.Models.Settings;
    using Talegen.AspNetCore.App.Services.Messaging;
    using Talegen.AspNetCore.App.Services.Messaging.Queue;
    using Talegen.AspNetCore.App.Services.Messaging.Smtp;
    using Talegen.AspNetCore.App.Services.Queue;
    using Talegen.AspNetCore.Web.Bindings;
    using Talegen.Common.Core.Errors;
    using Talegen.Common.Core.Extensions;
    using Talegen.Common.Models.Server.Configuration;

    /// <summary>
    /// This class defines the application startup logic for a core web API.
    /// </summary>
    /// <typeparam name="TConfigType">Contains the configuration data type.</typeparam>
    public class AppStartup<TConfigType> where TConfigType : class, IApplicationSettings, new()
    {
        /// <summary>
        /// Contains the value for max number of files to import. The default value is 1024.
        /// </summary>
        public const int MaxNumberOfFilesToImport = 8192;

        /// <summary>
        /// Contains the default scope profile.
        /// </summary>
        private const string ScopeProfile = "profile";

        /// <summary>
        /// Contains the default telemetry key.
        /// </summary>
        private const string DefaultTelemetryKey = "TelemetryConnection";

        /// <summary>
        /// Contains the cache connection string.
        /// </summary>
        private string cacheConnectionString = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppStartup{TConfigType}" /> class.
        /// </summary>
        /// <param name="configuration">Contains a configuration instance.</param>
        /// <param name="environment">Contains an environment instance.</param>
        /// <param name="appName">Contains the application name of the entry program.</param>
        public AppStartup(IConfiguration configuration, IWebHostEnvironment environment, string? appName = null)
        {
            this.Configuration = configuration;
            this.Environment = environment;
            this.AppName = appName ?? Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";

#if DEBUG
            Console.Title = this.AppName;
#endif
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the application environment.
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        public TConfigType AppSettings { get; private set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// Gets the Json settings.
        /// </summary>
        public JsonSerializerSettings JsonSettings => new JsonSerializerSettings
        {
            Formatting = this.Environment.IsDevelopment() ? Formatting.Indented : Formatting.None,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include & DefaultValueHandling.Populate,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter> { new DecimalConverter() }
        };

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <param name="iocInitialization">Contains an optional services initialization action.</param>
        /// <param name="postSecurityServiceInitialization">Contains an optional post security service intialization.</param>
        public void ConfigureServices(IServiceCollection services, Action<IServiceCollection>? iocInitialization = default, Action<IServiceCollection>? postSecurityServiceInitialization = default)
        {
            bool development = this.Environment.IsDevelopment();

            // load application settings.
            this.InitializeSettings(services);

            this.InitializeServices(services, development);

            // call the optional ioc initialization method for additional service configuration.
            iocInitialization?.Invoke(services);

            // initialize web api settings.
            this.InitializeWebApiSettings(services, development);

            // setup security if defined.
            this.InitializeSecurity(services, development);

            // initialize advanced settings.
            this.InitializeAdvancedSettings(services, development);

            // call the optional post security service initialization method for additional service configuration.
            postSecurityServiceInitialization?.Invoke(services);

            this.InitializeRequestSettings(services, development);

            this.InitializeBackgroundWorkers(services);
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Contains an instance of application builder.</param>
        /// <param name="optionalBuilder">Contains an optional action for app builder logic.</param>
        /// <param name="optionalRoutes">Contains an optional action for application route logic.</param>
        public void Configure(IApplicationBuilder app, Action<IApplicationBuilder>? optionalBuilder = default, Action<IEndpointRouteBuilder>? optionalRoutes = default)
        {
            bool development = this.Environment.IsDevelopment();

            // configure application security
            this.ConfigureApplicationSecurity(app, development);

            // invoke additional builder logic if defined.
            optionalBuilder?.Invoke(app);

            this.ConfigureApplicationRoutes(app, optionalRoutes);
        }

        /// <summary>
        /// This method is used to configure the application security.
        /// </summary>
        /// <param name="app">Contains an application builder instance.</param>
        /// <param name="development">Contains a value indicating if running in development environment.</param>
        private void ConfigureApplicationSecurity(IApplicationBuilder app, bool development)
        {
            if (development || this.AppSettings.Advanced.ShowDiagnostics)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();

            if (this.AppSettings.Security.EnableCors)
            {
                app.UseCors("CorsPolicy");
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles(this.CreateStaticFileSettings());
            app.UseRouting();

            if (this.AppSettings.Security != null && this.AppSettings.Security.AuthorityUri != null)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
        }

        /// <summary>
        /// This method is used to configure the application routes.
        /// </summary>
        /// <param name="app">Contains an application builder instance.</param>
        /// <param name="optionalRoutes">Contains an optional action for application route logic.</param>
        private void ConfigureApplicationRoutes(IApplicationBuilder app, Action<IEndpointRouteBuilder>? optionalRoutes = default)
        {
            ArgumentNullException.ThrowIfNull(app);

            app.UseEndpoints(endpoints =>
            {
                if (this.AppSettings.Security != null && this.AppSettings.Security.AuthorityUri != null)
                {
                    endpoints.MapDefaultControllerRoute()
                        .RequireAuthorization();
                }
                else
                {
                    endpoints.MapDefaultControllerRoute();
                }

                // invoke optional routes if defined.
                optionalRoutes?.Invoke(endpoints);
            });
        }

        /// <summary>
        /// This method is used to initialize the application settings.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <exception cref="Exception">Exception is thrown if no application settings section is found.</exception>
        private void InitializeSettings(IServiceCollection services)
        {
            // setup logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(this.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            IConfigurationSection settingsSection = this.Configuration.GetSection(typeof(TConfigType).Name);

            services.Configure<TConfigType>(settingsSection)
                .PostConfigure<TConfigType>(options =>
                {
                    // handle any post configuration settings here.
                    Log.Debug("Startup Initialize Settings - Post Configure for services.Configure<{0}>(settingsSection);", nameof(TConfigType));
                });

            this.AppSettings = settingsSection.Get<TConfigType>() ?? throw new AppServerException(string.Format(Properties.Resources.ErrorNoConfigurationText, typeof(TConfigType).Name));
            bool development = this.Environment.IsDevelopment();
            this.cacheConnectionString = this.Configuration.GetConnectionString(this.AppSettings.Cache.ConnectionStringName).ConvertToString();

            if (development || this.AppSettings.Advanced.ShowDiagnostics)
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;               
                Log.Debug("Cache Type: {0}", this.AppSettings.Cache.CacheType.ToString());
                Log.Debug("Cache Connection String: {0}", this.cacheConnectionString);
                Log.Debug("Cache Name: {0}", this.AppSettings.Cache.CacheName);
                Log.Debug("Application Settings:\n{0}", this.AppSettings);
            }
        }

        /// <summary>
        /// This method is used to initialize the application services.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <param name="development">Contains a value indicating if running in development environment.</param>
        private void InitializeServices(IServiceCollection services, bool development)
        {
            // initialize the error manager
            services.AddTransient<IErrorManager, ErrorManager>();

            // setup JSON settings
            // default JSON serializer settings
            services.AddSingleton((service) =>
            {
                return this.JsonSettings;
            });

            // initialize cache mechanisms
            this.InitializeCache(services);

            // initialize telemetry.
            this.InitializeTelemetry(services, development);

            // initialize the messaging service
            this.InitializeMessagingService(services);

            // setup unhandled APi error modeling.
            services.AddScoped<ApiExceptionFilterAttribute>();
        }

        /// <summary>
        /// Initializes the cache system.
        /// </summary>
        /// <param name="services">Contains the service collection</param>
        private void InitializeCache(IServiceCollection services)
        {
            if (this.AppSettings.Cache.CacheType == CacheType.Redis && !string.IsNullOrWhiteSpace(this.AppSettings.Cache.ConnectionStringName))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = this.cacheConnectionString;
                    options.InstanceName = this.AppSettings.Cache.CacheName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
        }

        /// <summary>
        /// This method is used to initialize the telemetry system.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <param name="development">Contains a value indicating if running in development environment.</param>
        /// <exception cref="NotImplementedException">Exception is thrown if attempting to use CloudWatch telemetry.</exception>
        private void InitializeTelemetry(IServiceCollection services, bool development)
        {
            string key = !string.IsNullOrWhiteSpace(this.AppSettings.Telemetry.InstrumentationKey) ? this.AppSettings.Telemetry.InstrumentationKey : DefaultTelemetryKey;
            string telemetryConnectionString = this.Configuration.GetConnectionString(key).ConvertToString();

            if (this.AppSettings.Telemetry.Enabled &&
                (!development || this.AppSettings.Advanced.ShowDiagnostics)
                && !string.IsNullOrWhiteSpace(telemetryConnectionString))
            {
                switch (this.AppSettings.Telemetry.TelemetryType)
                {
                    case TelemetryType.ApplicationInsights:
                        services.AddApplicationInsightsTelemetry();
                        break;
                    case TelemetryType.AwsCloudWatch:
                        throw new NotImplementedException();
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// This method is used to initialize the configured messaging service.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <exception cref="NotImplementedException">Thrown if a non-supported messaging type is specified in configuration.</exception>
        private void InitializeMessagingService(IServiceCollection services)
        {
            // initialize the messaging service
            if (this.AppSettings.Messaging != null && this.AppSettings.Messaging.MessagingType != MessagingType.None)
            {
                // add messaging queue service
                services.AddSingleton<IMessagingQueue, MessagingQueue>();

                // add queued message sender
                services.AddSingleton<IMessageSender, QueuedMessageSender>();

                Log.Information("Configuring Messaging Services ({0})", this.AppSettings.Messaging.MessagingType);

                switch (this.AppSettings.Messaging.MessagingType)
                {
                    case MessagingType.Smtp:
                        Log.Debug("Adding SMTP messaging");
                        // setup smtp messaging service
                        services.AddSingleton((service) =>
                        {
                            return new SmtpMessageContext
                            {
                                QueueProcessingIntervalSeconds = this.AppSettings.Messaging.QueueProcessingIntervalSeconds,
                                QueueProcessingMaxRetries = this.AppSettings.Messaging.QueueProcessingMaxRetries,
                                TokenValues = new Dictionary<string, string>(),
                                UseSsl = this.AppSettings.Messaging.UseSsl,
                                UserName = this.AppSettings.Messaging.UserName,
                                ReplyEmail = this.AppSettings.Messaging.ReplyEmail,
                                Port = this.AppSettings.Messaging.Port,
                                Password = this.AppSettings.Messaging.Password,
                                HostName = this.AppSettings.Messaging.HostName,
                            };
                        });

                        // add SMTP sender message factory
                        services.AddTransient<ISenderMessageFactory, SmtpSenderMessageFactory>();

                        // add a messaging processor
                        services.AddSingleton<IMessageProcessor, SmtpMessageProcessor>();
                        services.AddSingleton<IMessagingService, SmtpMessagingService>();
                        break;
                    case MessagingType.Memory:
                        throw new NotImplementedException();
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// This method is used to initialize the web api settings.
        /// </summary>
        /// <param name="services">Contains the service collection.</param>
        /// <param name="development">Contains a value indicating whether the environment is development.</param>
        private void InitializeWebApiSettings(IServiceCollection services, bool development)
        {
            bool authEnabled = this.AppSettings.Security != null && this.AppSettings.Security.AuthorityUri != null;

            services.AddControllers(options =>
            {
                // if security is enabled, add the authorization policy
                if (development || authEnabled)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                }
                
                // add custom bindings overrides to support empty Guids and other custom binding types.
                options.AddBindingOverrides();
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.Converters.Add(new StringEnumConverter());
                setup.SerializerSettings.Formatting = development ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;
                setup.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                setup.SerializerSettings.StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii;
                setup.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                setup.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                setup.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include & Newtonsoft.Json.DefaultValueHandling.Populate;
                setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }); 
        }

        /// <summary>
        /// This method is used to initialize advanced settings.
        /// </summary>
        /// <param name="services">Contains the services collection.</param>
        /// <param name="development">Contains a value indicating whether the enviromnet is development.</param>
        private void InitializeAdvancedSettings(IServiceCollection services, bool development)
        {
            if (this.AppSettings.Advanced != null)
            {

                // add health checks
                services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy());

                if (this.AppSettings.Advanced.MinimumCompletionPortThreads > 0)
                {
                    ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
                    ThreadPool.SetMinThreads(minWorkerThreads * 2, minCompletionPortThreads > this.AppSettings.Advanced.MinimumCompletionPortThreads ? minCompletionPortThreads : this.AppSettings.Advanced.MinimumCompletionPortThreads);
                }

                if (this.AppSettings.Advanced.CookieSettings)
                {
                    services.Configure<CookiePolicyOptions>(options =>
                    {
                        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                        options.Secure = development
                            ? CookieSecurePolicy.SameAsRequest
                            : CookieSecurePolicy.Always;
                    });
                }
            }
        }

        /// <summary>
        /// This method is used to initialize security.
        /// </summary>
        /// <param name="services">Contains the services collection.</param>
        /// <param name="development">Contains a value indicating whether the enviromnet is development.</param>
        private void InitializeSecurity(IServiceCollection services, bool development)
        {
            if (this.AppSettings.Security != null && this.AppSettings.Security.AuthorityUri != null)
            {
                if (this.AppSettings.Security.ClearClaimMapping)
                {
                    // clear default claim mapping
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                }

                // add discovery cache mechanism for openid connect
                services.AddSingleton<IDiscoveryCache>(serviceCollection =>
                {
                    var factory = serviceCollection.GetRequiredService<IHttpClientFactory>();
                    return new DiscoveryCache(this.AppSettings.Security.AuthorityUri.ToString(), () => factory.CreateClient());
                });

                // if we're forcing SSL...
                if (!development)
                {
                    services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
                }

                // setup HSTS settings
                services.AddHsts(options =>
                {
                    options.IncludeSubDomains = true;
                    options.MaxAge = development ? TimeSpan.FromMinutes(60) : TimeSpan.FromDays(365);
                });

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddOpenIdConnect(options =>
                    {
                        options.SignInScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.Authority = this.AppSettings.Security.AuthorityUri.ToString();
                        options.RequireHttpsMetadata = !development;
                        options.ClientId = this.AppSettings.Security.ClientId;
                        options.ClientSecret = this.AppSettings.Security.ClientSecret;
                        options.Resource = this.AppSettings.Security.ResourceName;
                        options.ResponseType = "code";
                        options.UsePkce = true;
                        options.Scope.Add(ScopeProfile);
                        options.SaveTokens = true;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.AutomaticRefreshInterval = TimeSpan.FromMinutes(5);
                        options.BackchannelTimeout = TimeSpan.FromMinutes(5);
                        options.RefreshInterval = TimeSpan.FromMinutes(5);
                        
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "name",
                            RoleClaimType = "role"
                        };
                    });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("ResourcePolicy", builder => 
                    {
                        builder.RequireClaim(this.AppSettings.Security.ResourceName);
                    });
                });

                if (this.AppSettings.Security.EnableCors)
                {
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", policy =>
                        {
                            policy.AllowAnyMethod();
                            policy.AllowAnyHeader();

                            // if origins defined, restrict them.
                            if (this.AppSettings.Security.AllowedOrigins != null && this.AppSettings.Security.AllowedOrigins.Count > 0)
                            {
                                // allow for the matching of a wildcard domain.
                                policy.WithOrigins(this.AppSettings.Security.AllowedOrigins.ToArray())
                                .SetIsOriginAllowedToAllowWildcardSubdomains()
                                .AllowCredentials();
                            }
                            else
                            {
                                policy.AllowAnyOrigin();
                            }

                            // For CSV or any file download need to expose the headers, otherwise in JavaScript
                            // response.getResponseHeader('Content-Disposition') retuns undefined
                            // See https://stackoverflow.com/questions/58452531/im-not-able-to-access-response-headerscontent-disposition-on-client-even-aft
                            policy.WithExposedHeaders("Content-Disposition");
                        });
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">Contains the services collection.</param>
        /// <param name="development">Contains a value indicating whether the enviromnet is development.</param>
        private void InitializeRequestSettings(IServiceCollection services, bool development)
        {
            // we need to set the max request body size, default is 30MB
            // Reference: https://github.com/dotnet/aspnetcore/issues/20369 
            if (development)
            {
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.Limits.MaxRequestBodySize = this.AppSettings.Advanced.MaxRequestSize < 0 ? int.MaxValue : this.AppSettings.Advanced.MaxRequestSize;
                });
            }
            else
            {
                services.Configure<IISServerOptions>(options =>
                {
                    options.MaxRequestBodySize = this.AppSettings.Advanced.MaxRequestSize < 0 ? int.MaxValue : this.AppSettings.Advanced.MaxRequestSize;
                });
            }

            // set request form limits
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = this.AppSettings.Advanced.ValueLengthLimit < 0 ? int.MaxValue : this.AppSettings.Advanced.ValueLengthLimit;
                options.MultipartBodyLengthLimit = this.AppSettings.Advanced.MultiPartBodyLengthLimit < 0 ? int.MaxValue : this.AppSettings.Advanced.MultiPartBodyLengthLimit; // if don't set default value is: 128 MB
                options.MultipartHeadersLengthLimit = MaxNumberOfFilesToImport;
                options.ValueCountLimit = MaxNumberOfFilesToImport;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.MaxModelBindingCollectionSize = MaxNumberOfFilesToImport;
            });
        }   

        /// <summary>
        /// This method is used to create the static file settings.
        /// </summary>
        /// <returns>Returns a new <see cref="StaticFileOptions"/> object with additional web-mappings.</returns>
        private StaticFileOptions CreateStaticFileSettings()
        {
            var extensionsProvider = new FileExtensionContentTypeProvider();

            if (!extensionsProvider.Mappings.ContainsKey(".json"))
            {
                extensionsProvider.Mappings.Add(".json", "application/json");
            }

            if (!extensionsProvider.Mappings.ContainsKey(".woff2"))
            {
                extensionsProvider.Mappings.Add(".woff2", "font/woff2");
            }

            if (!extensionsProvider.Mappings.ContainsKey(".appcache"))
            {
                extensionsProvider.Mappings.Add(".appcache", "text/cache-manifest");
            }

            if (!extensionsProvider.Mappings.ContainsKey(".properties"))
            {
                extensionsProvider.Mappings.Add(".properties", "text/properties");
            }

            return new StaticFileOptions { ContentTypeProvider = extensionsProvider };
        }

        /// <summary>
        /// Initializes the background workers.
        /// </summary>
        /// <param name="services">Contains the services collection.</param>
        private void InitializeBackgroundWorkers(IServiceCollection services)
        {
            try
            {
                // determine if messaging has been defined
                if (services.Select(x => x.ServiceType).Contains(typeof(IMessagingService)))
                {
                    // add the background messaging job
                    services.AddHostedService<BackgroundMessagingJob>();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Properties.Resources.StartupBackgroundWorkerErrorText);
            }
        }
    }
}
