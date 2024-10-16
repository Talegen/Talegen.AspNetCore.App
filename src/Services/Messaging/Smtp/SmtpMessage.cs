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
namespace Talegen.AspNetCore.App.Services.Messaging.Smtp
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Talegen.AspNetCore.App.Shared.Services.Queue;
    using Talegen.AspNetCore.App.Shared.Services.Messaging;

    /// <summary>
    /// This class represents a message to be sent via the sender messaging system.
    /// </summary>
    public sealed class SmtpMessage : ISenderMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMessage" /> class.
        /// </summary>
        /// <param name="from">Contains the sender address.</param>
        /// <param name="to">Contains a recipient address.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="textBody">Contains the text message body.</param>
        /// <param name="htmlBody">Contains the HTML message body.</param>
        /// <param name="textBodyContentType">Contains the optional text body content type value.</param>
        /// <param name="htmlBodyContentType">Contains the optional HTML body content type value.</param>
        public SmtpMessage(string from, string to = "", string subject = "", string textBody = "", string htmlBody = "", string textBodyContentType = "text/plain", string htmlBodyContentType = "text/html")
            : this(new SmtpMailAddress(from), new SmtpMailAddress(to), subject, textBody, htmlBody, textBodyContentType, htmlBodyContentType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMessage" /> class.
        /// </summary>
        /// <param name="from">Contains the sender address.</param>
        /// <param name="to">Contains a recipient address.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="textBody">Contains the text message body.</param>
        /// <param name="htmlBody">Contains the HTML message body.</param>
        /// <param name="textBodyContentType">Contains the optional text body content type value.</param>
        /// <param name="htmlBodyContentType">Contains the optional HTML body content type value.</param>
        public SmtpMessage(ISenderAddress from, ISenderAddress to, string subject = "", string textBody = "", string htmlBody = "", string textBodyContentType = "text/plain", string htmlBodyContentType = "text/html")
            : this(from, new List<ISenderAddress>() { to }, subject, textBody, htmlBody, false, textBodyContentType, htmlBodyContentType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMessage" /> class.
        /// </summary>
        /// <param name="from">Contains the sender address.</param>
        /// <param name="recipients">Contains a list of recipient mail addresses.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="textBody">Contains the text message body.</param>
        /// <param name="htmlBody">Contains the HTML message body.</param>
        /// <param name="recipientsVisible">
        /// Contains a value indicating whether the recipients are visible to each other. If false, each recipient will receive a single copy of the message.
        /// </param>
        /// <param name="textBodyContentType">Contains the optional text body content type value.</param>
        /// <param name="htmlBodyContentType">Contains the optional HTML body content type value.</param>
        public SmtpMessage(ISenderAddress from, List<ISenderAddress> recipients, string subject = "", string textBody = "", string htmlBody = "", bool recipientsVisible = false, string textBodyContentType = "text/plain", string htmlBodyContentType = "text/html")
        {
            this.From = from ?? throw new ArgumentNullException(nameof(from));
            this.Recipients = recipients ?? throw new ArgumentNullException(nameof(recipients));

            this.RecipientsVisible = recipientsVisible;
            this.Subject = subject;
            this.TextBody = textBody;
            this.HtmlBody = htmlBody;
            this.TextContentType = textBodyContentType;
            this.HtmlContentType = htmlBodyContentType;
        }

        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the queue date time.
        /// </summary>
        public DateTime QueueDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the state of the queue item.
        /// </summary>
        public QueueItemState State { get; set; } = QueueItemState.New;

        /// <summary>
        /// Gets or sets the retry count of the queue item.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Gets or sets a priority.
        /// </summary>
        public MessagePriority Priority { get; set;} = MessagePriority.Normal;

        /// <summary>
        /// Gets or sets the sender address.
        /// </summary>
        public ISenderAddress From { get; set; }

        /// <summary>
        /// Gets or sets a list of recipient addresses.
        /// </summary>
        public List<ISenderAddress> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the text body of the message.
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// Gets or sets the HTML body of the message.
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the recipients are all included in a single message.
        /// </summary>
        public bool RecipientsVisible { get; set; }

        /// <summary>
        /// Gets or sets the subject of the message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the optional text body content type.
        /// </summary>
        public string TextContentType { get; set; } = "text/plain";

        /// <summary>
        /// Gets or sets the optional HTML body content type.
        /// </summary>
        public string HtmlContentType { get; set; } = "text/html";
        
        /// <summary>
        /// This method is used to convert the message class into a string for reporting.
        /// </summary>
        /// <returns>Returns a JSON formatted string.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}