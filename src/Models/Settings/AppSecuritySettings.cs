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
    /// This class contains the application security settings.
    /// </summary>
    public class AppSecuritySettings : SecuritySettings
    {
        /// <summary>
        /// Gets or sets the resource name.
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }  = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the default JWT claim mapping is cleared.
        /// </summary>
        public bool ClearClaimMapping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the CORS settings are defined.
        /// </summary>
        public bool EnableCors { get; set; } = true;

        /// <summary>
        /// Gets or sets the resource scope.
        /// </summary>
        public string ResourceScope { get; set; }
    }
}
