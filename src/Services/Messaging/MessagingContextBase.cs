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
namespace Talegen.AspNetCore.App.Services.Messaging
{
    using System.Reflection;

    /// <summary>
    /// This class contains the base messaging context properties.
    /// </summary>
    public class MessagingContextBase 
    {

        /// <summary>
        /// Contains the message template sub-folder.
        /// </summary>
        private string messageTemplateFolder;

        /// <summary>
        /// Contains the message queue folder.
        /// </summary>
        private string messageQueueFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingContextBase" /> class.
        /// </summary>
        /// <param name="messageTemplateFolder">Contains an optional message template folder.</param>
        /// <param name="messageQueueFolder">Contains an optional message queue folder.</param>
        public MessagingContextBase(string messageTemplateFolder = "Templates", string messageQueueFolder = "Queue")
        {
            this.MessageTemplateFolder = messageTemplateFolder;
            this.MessageQueueFolder = messageQueueFolder;
        }

        /// <summary>
        /// Gets or sets a dictionary of token values to be inserted into the body of messages.
        /// </summary>
        public Dictionary<string, string> TokenValues { get; set; } = [];

        /// <summary>
        /// Gets or sets the message template sub-folder.
        /// </summary>
        public string MessageTemplateFolder
        {
            get
            {
                return this.messageTemplateFolder;
            }

            set
            {
                if (!Path.IsPathRooted(value))
                {
                    this.messageTemplateFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, value);
                }
                else
                {
                    this.messageTemplateFolder = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the message queue folder.
        /// </summary>
        public string MessageQueueFolder
        {
            get
            {
                return this.messageQueueFolder;
            }

            set
            {
                if (!Path.IsPathRooted(value))
                {
                    this.messageQueueFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, value);
                }
                else
                {
                    this.messageQueueFolder = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the queue processing max retries.
        /// </summary>
        public int QueueProcessingMaxRetries { get; set; } = 5;

        /// <summary>
        /// Gets or sets the interval in which queued messages are processed.
        /// </summary>
        public int QueueProcessingIntervalSeconds { get; set; } = 5;
    }
}
