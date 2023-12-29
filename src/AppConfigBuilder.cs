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
    using System.Xml.Linq;
    using Serilog;
    using Talegen.AspNetCore.App.Properties;

    /// <summary>
    /// This class contains the application configuration builder.
    /// </summary>
    /// <typeparam name="TAppConfigType">Contains the type of application configuration class.</typeparam>
    /// <example>
    /// var c = new AppConfigBuilder<AppConfig>(new AppConfigBuilderSettings { UseSerilog = true });
    /// // c.AppConfig.SomeProperty
    /// </example>
    public class AppConfigBuilder<TAppConfigType> : ConfigurationBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigBuilder"/> class.
        /// </summary>
        /// <param name="appConfigBuilderSettings">Contains the app config builder settings.</param>
        public AppConfigBuilder(AppConfigBuilderSettings appConfigBuilderSettings)
        {
            // set the base path to the current directory
            this.SetBasePath(Directory.GetCurrentDirectory());

            // set the appsettings.json as the default, then development, then environment variables
            this.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

            // build the configuration
            Configuration = this.Build();

            if (appConfigBuilderSettings.UseSerilog)
            {
                // setup logging
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .ReadFrom.Configuration(Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();
            }

            // create the app config instance
            Configuration.GetSection(typeof(TAppConfigType).Name).Bind(this.AppConfig);

            if (this.AppConfig == null)
            {
                throw new Exception(string.Format(Resources.ErrorNoConfigurationText, typeof(TAppConfigType).Name));
            }
        }

        /// <summary>
        /// Gets or sets an instance of the application configuration.
        /// </summary>
        public static IConfiguration? Configuration { get; private set; }

        /// <summary>
        /// Gets an instance of the application configuration.
        /// </summary>
        public TAppConfigType AppConfig { get; private set; }
    }
}
