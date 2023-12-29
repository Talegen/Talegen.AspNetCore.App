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
namespace Talegen.AspNetCore.App.Models.Settings
{
    using Talegen.Common.Models.Server.Configuration;
    
    /// <summary>
    /// This class contains the base application settings.
    /// </summary>
    public class AppSettingsBase : IApplicationSettings
    {
        /// <summary>
        /// Gets or sets the cache related settings.
        /// </summary>
        public AppCacheSettings Cache { get; set; } = new AppCacheSettings();

        /// <summary>
        /// Gets or sets security settings.
        /// </summary>
        public AppSecuritySettings Security {get; set; } = new AppSecuritySettings();

        /// <summary>
        /// Gets or sets the telemetry settings.
        /// </summary>
        public AppTelemetrySettings Telemetry { get; set; } = new AppTelemetrySettings();

        /// <summary>
        /// Gets or sets the email settings.
        /// </summary>
        public AppMessagingSettings Messaging { get; set; } = new AppMessagingSettings();

        /// <summary>
        /// Gets or sets advanced settings.
        /// </summary>
        public AppAdvancedSettings Advanced { get; set; } = new AppAdvancedSettings();    
    }
}
