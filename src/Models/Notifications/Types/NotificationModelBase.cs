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
namespace Talegen.AspNetCore.App.Models.Notifications.Types
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// This abstract class contains properties that are to be applied to all notification model classes.
    /// </summary>
    public abstract class NotificationModelBase
    {
        /// <summary>
        /// Gets or sets the metadata string and deserializes JSON formatted metadata into the Metadata dictionary.
        /// </summary>
        [JsonIgnore]
        public string MetadataModel
        {
            get
            {
                return JsonConvert.SerializeObject(this.Metadata);
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.Metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(value) ?? [];
                }
            }
        }

        /// <summary>
        /// Gets or sets an optional set of metadata for the notification message.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = [];
    }
}