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
    /// Contains application advanced settings.
    /// </summary>
    public class AppAdvancedSettings : AdvancedSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to enable the cookie settings.
        /// </summary>
        public bool CookieSettings { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum request body size.
        /// </summary>
        public int MaxRequestSize { get; set; } = 30000000;

        /// <summary>
        /// Gets or sets the maximum multipart body length limit.
        /// </summary>
        public int MultiPartBodyLengthLimit { get; set; } = 128000000;

        /// <summary>
        /// Gets or sets the maximum value length limit.
        /// </summary>
        public int ValueLengthLimit { get; set; } = 4000000;
    }
}
