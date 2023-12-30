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
    using Talegen.AspNetCore.App.Services.Queue;


    /// <summary>
    /// Contains an enumeration of message body types.
    /// </summary>
    public enum MessageBodyType
    {
        /// <summary>
        /// Message body is plain text.
        /// </summary>
        Text = 0,

        /// <summary>
        /// Message body is HTML.
        /// </summary>
        Html = 1,

        /// <summary>
        /// Message body is other.
        /// </summary>
        Other = 2
    }

    /// <summary>
    /// Contains an enumeration of message priorities.
    /// </summary>
    public enum MessagePriority
    {
        /// <summary>
        /// Low priority.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Normal priority.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 2
    }

    /// <summary>
    /// This interface defines a minimum implementation of a sender message.
    /// </summary>
    public interface ISenderMessage : IQueueItem
    {
        /// <summary>
        /// Gets or sets the sender address.
        /// </summary>
        ISenderAddress From { get; set; }

        /// <summary>
        /// Gets or sets the recipient addresses.
        /// </summary>
        List<ISenderAddress> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the message priority.
        /// </summary>
        MessagePriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the message subject.
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets a dictionary of message bodies.
        /// </summary>
        Dictionary<MessageBodyType, string> Bodies { get; set; }
    }
}
