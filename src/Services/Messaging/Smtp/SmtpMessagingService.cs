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
    using Serilog;
    using Talegen.AspNetCore.App.Properties;
    using Talegen.AspNetCore.App.Shared.Services.Messaging;
    using Talegen.AspNetCore.App.Shared.Services.Queue;
    using Talegen.Common.Core.Extensions;

    /// <summary>
    /// This class contains the logic for processing queued messages using a common SMTP messaging service.
    /// </summary>
    public class SmtpMessagingService : IMessagingService
    {
        /// <summary>
        /// Contains an instance of the SMTP messaging context.
        /// </summary>
        private readonly SmtpMessageContext context;

        /// <summary>
        /// Contains an instance of the queue service.
        /// </summary>
        private readonly IMessagingQueue queueService;

        /// <summary>
        /// Contains an instance of the message processor.
        /// </summary>
        private readonly IMessageProcessor messageProcessor;

        /// <summary>
        /// Contains a value indicating whether the queue should be processing.
        /// </summary>
        private bool processing = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMessagingService" /> class.
        /// </summary>
        /// <param name="context">Contains an instance of SMTP messaging context.</param>
        /// <param name="queueService">Contains an instance of queue service.</param>
        /// <param name="messageProcessor">Contains an instance of message processor.</param>
        public SmtpMessagingService(SmtpMessageContext context, IMessagingQueue queueService, IMessageProcessor messageProcessor)
        {
            this.context = context;
            this.queueService = queueService;
            this.messageProcessor = messageProcessor;
        }

        /// <summary>
        /// Gets or sets the queue processing interval in seconds.
        /// </summary>
        public int QueueProcessingIntervalSeconds => this.context.QueueProcessingIntervalSeconds;

        /// <summary>
        /// This method is used to check the message queue folder availability.
        /// </summary>
        /// <returns>Returns a value indicating whether the message queue folder exists.</returns>
        public bool CheckQueuePath()
        {
            bool result = !string.IsNullOrWhiteSpace(this.context.MessageQueueFolder);

            // if the directory does not exist
            if (result && !Directory.Exists(this.context.MessageQueueFolder))
            {
                Directory.CreateDirectory(this.context.MessageQueueFolder);
            }

            result = result && Directory.Exists(this.context.MessageQueueFolder);

            return result;
        }

        /// <summary>
        /// This method is used to send a message for the application.
        /// </summary>
        public void Process()
        {
            Log.Debug(Resources.MessagingServiceProcessQueueText, this.messageProcessor.ToString());

            // process the queue
            var queueItems = this.queueService.Peek();

            // execute queue processing
            while (this.processing && queueItems.Any())
            {
                foreach (var queueItem in queueItems)
                {
                    if (queueItem is SmtpMessage smtpMessage)
                    {
                        try
                        {
                            if (AsyncHelper.RunSync(() => this.messageProcessor.ProcessMessageAsync(smtpMessage)))
                            {
                                this.queueService.Remove(queueItem);
                            }
                            else
                            {
                                queueItem.RetryCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, Resources.ErrorMessageSenderText);
                        }   
                    }
                    else
                    {
                        Log.Error(Resources.ErrorInvalidMessageTypeText, typeof(SmtpMessage).Name);
                    }
                }
                

                queueItems = this.queueService.Peek();
            }
        }

        /// <summary>
        /// This method is used to restore messages from the message queue folder.
        /// </summary>
        public void RestoreQueue()
        {
            Log.Debug(Resources.MessagingServiceRestoreQueueText, this.messageProcessor.ToString());

            if (this.CheckQueuePath())
            {
                string[] files = Directory.GetFiles(this.context.MessageQueueFolder);

                // for each file file in the message queue path.
                foreach (string filePath in files)
                {
                    try
                    {
                        FileInfo file = new FileInfo(filePath);

                        if (file.Exists)
                        {
                            SmtpMessage message = file.Deserialize<SmtpMessage>();

                            if (message != null)
                            {
                                // add message to queue
                                this.queueService.Add(message);

                                // remove the file from disk storage
                                file.Delete();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Resources.ErrorRestoreMessageQueueText, this.context.MessageQueueFolder, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to store messages in the message queue folder.
        /// </summary>
        public void StoreQueue()
        {
            Log.Debug(Resources.MessagingServiceStoreQueueText, this.messageProcessor.ToString());

            // if the queue message path exists...
            if (this.CheckQueuePath())
            {
                // stop processing....
                this.processing = false;
                
                // execute queue storage
                while (this.queueService.Messages.TryDequeue(out var message))
                {
                    if (message != null)
                    {
                        try
                        {
                            if (message is SmtpMessage smtpMessage)
                            {
                                smtpMessage.QueueDateTime = DateTime.UtcNow;

                                string tempFilePath = Path.Combine(this.context.MessageQueueFolder, $"{message.Id}.msg");

                                // the file should never exist, but check anyway...
                                if (!File.Exists(tempFilePath))
                                {
                                    smtpMessage.Serialize(tempFilePath);
                                }
                            }
                            else
                            {
                                Log.Error(Resources.ErrorInvalidMessageTypeText, typeof(SmtpMessage).Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, Resources.ErrorStoreMessageQueueText, this.context.MessageQueueFolder, ex.Message);
                        }
                    }
                    else
                    {
                        Log.Error(Resources.ErrorInvalidMessageTypeText, typeof(IQueueItem).Name);
                    }
                }
            }
        }
    }
}
