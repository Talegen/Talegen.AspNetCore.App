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
    /// <summary>
    /// This class represents a message.
    /// </summary>
    public class NotificationMessageModel : NotificationModelBase
    {
        /// <summary>
        /// Gets or sets the message subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the summary message to display to the user in notification toasts.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the message content body in optional HTML format.
        /// </summary>
        public string HtmlBody { get; set; }
    }
}