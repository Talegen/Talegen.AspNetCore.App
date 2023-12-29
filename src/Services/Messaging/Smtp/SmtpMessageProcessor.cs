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
    using System.Net;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.App.Properties;
    using Talegen.AspNetCore.App.Services.Queue;
    using Talegen.Common.Core.Errors;

    /// <summary>
    /// This class contains the logic for processing queued messages using a common SMTP messaging service.
    /// </summary>
    public class SmtpMessageProcessor : IMessageProcessor
    {
        #region Private Fields

        /// <summary>
        /// Contains an instance of the error manager.
        /// </summary>
        private readonly IErrorManager errorManager;

        /// <summary>
        /// The messaging settings.
        /// </summary>
        private readonly SmtpMessageContext context;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMessageProcessor" /> class.
        /// </summary>
        /// <param name="context">Contains message settings.</param>
        /// <param name="errorManager">Contains an instance of the error manager.</param>
        public SmtpMessageProcessor(SmtpMessageContext context, IErrorManager errorManager)
        {
            this.context = context;
            this.errorManager = errorManager;
        }

        #endregion

        /// <summary>
        /// This method is used to send a message for the application.
        /// </summary>
        /// <param name="message">Contains the mail message to send.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns an async Task result.</returns>
        public async Task<bool> ProcessMessageAsync(IQueueItem message, CancellationToken cancellationToken = default)
        {
            bool result = true;

            // new up a new SMTP client
            using SmtpClient client = new SmtpClient(this.context.HostName, this.context.Port);
            
            try
            {
                // setup credentials if necessary
                if (!string.IsNullOrWhiteSpace(this.context.UserName))
                {
                    client.Credentials = new NetworkCredential(this.context.UserName, this.context.Password);
                    client.UseDefaultCredentials = false;
                }

                client.EnableSsl = this.context.UseSsl;

                var senderMessage = message as SmtpMessage 
                    ?? throw new ArgumentException(string.Format(Resources.ErrorSmtpSenderInvalidTypeText, typeof(IQueueItem).Name, typeof(SmtpMessage).Name), nameof(message));

                using var emailMessage = senderMessage.ToMailMessage();

                if (!cancellationToken.IsCancellationRequested)
                {
                    // send the message
                    await client.SendMailAsync(emailMessage, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                result = false;
                // increment retry count and log error
                if (message.RetryCount++ <= this.context.QueueProcessingMaxRetries)
                {
                    message.State = QueueItemState.CanRetry;
                }
                else
                {
                    message.State = QueueItemState.Failed;
                }

                Serilog.Log.Error(ex, Resources.ErrorSmtpSenderExceptionText);
                this.errorManager?.Critical(ex, ErrorCategory.Application);
                throw;
            }
            finally
            {
                client.Dispose();
            }

            return result;
        }

    }
}