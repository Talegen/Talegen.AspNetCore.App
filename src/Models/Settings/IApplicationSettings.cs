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
    /// Contains an interface for application settings.
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// Gets or sets the cache related settings.
        /// </summary>
        AppCacheSettings Cache { get; set; }


        /// <summary>
        /// Gets or sets security settings.
        /// </summary>
        AppSecuritySettings Security { get; set; }

        /// <summary>
        /// Gets or sets the telemetry settings.
        /// </summary>
        AppTelemetrySettings Telemetry { get; set; }

        /// <summary>
        /// Gets or sets advanced settings.
        /// </summary>
        AppAdvancedSettings Advanced { get; set; }

        /// <summary>
        /// Gets or sets the email settings.
        /// </summary>
        AppMessagingSettings Messaging { get; set; }
    }
}
