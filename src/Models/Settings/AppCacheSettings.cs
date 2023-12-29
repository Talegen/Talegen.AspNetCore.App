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
    /// This class contains the cache settings.
    /// </summary>
    public class AppCacheSettings
    {
        /// <summary>
        /// Contains the default cache key name.
        /// </summary>
        public const string CacheKeyName = "Cache";

        /// <summary>
        /// Gets or sets the cache type.
        /// </summary>
        public CacheType CacheType { get; set; } = CacheType.Memory;

        /// <summary>
        /// Gets or sets the connection string name.
        /// </summary>
        public string CacheName { get; set; } = CacheKeyName;

        /// <summary>
        /// Gets or sets the connection string name.
        /// </summary>
        public string ConnectionStringName { get; set; } = CacheKeyName;
    }
}
