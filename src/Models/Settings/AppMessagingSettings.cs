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
    /// Contains an enumerated list of messaging types.
    /// </summary>
    public enum MessagingType
    {
        /// <summary>
        /// No messaging.
        /// </summary>
        None,

        /// <summary>
        /// Memory messaging (Testing).
        /// </summary>
        Memory,

        /// <summary>
        /// SMTP messaging.
        /// </summary>
        Smtp
    }

    /// <summary>
    /// This class contains the messaging settings.
    /// </summary>
    public class AppMessagingSettings : EmailSettings
    {
        /// <summary>
        /// Gets or sets the messaging type.
        /// </summary>
        public MessagingType MessagingType { get; set; } = MessagingType.None;

        /// <summary>
        /// Gets or sets the queue processing max retries.
        /// </summary>
        public int QueueProcessingMaxRetries { get; set; } = 5;

        /// <summary>
        /// Gets or sets the queue processing interval in seconds.
        /// </summary>
        public int QueueProcessingIntervalSeconds { get; set; } = 60;

        /// <summary>
        /// Gets or sets a message template path.
        /// </summary>
        public string TemplatePath { get; set; } = "Templates";
    }
}
