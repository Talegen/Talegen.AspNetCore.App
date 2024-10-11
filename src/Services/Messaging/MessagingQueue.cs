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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Talegen.AspNetCore.App.Shared.Services.Queue;
    using Talegen.AspNetCore.App.Shared.Services.Messaging;

    /// <summary>
    /// This class represents a queue for handling messages.
    /// </summary>
    public class MessagingQueue : IMessagingQueue
    {
        /// <summary>
        /// Gets the thread-safe queue for handling messages.
        /// </summary>
        public ConcurrentQueue<IQueueItem> Messages { get; } = new ConcurrentQueue<IQueueItem>();

        /// <summary>
        /// Adds a new sender message to the queue to be processed.
        /// </summary>
        /// <param name="message">Contains the message to process.</param>
        public void Add(IQueueItem message)
        {
            message.QueueDateTime = DateTime.UtcNow;
            message.State = QueueItemState.New;
            message.RetryCount = 0;

            Messages.Enqueue(message);
        }

        /// <summary>
        /// Clears the queue of all messages.
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<IQueueItem> Peek(int count = 0)
        {
            var results = Messages
                .Where(Messages => Messages.State != QueueItemState.Failed
                        && Messages.State != QueueItemState.Complete)
                .AsQueryable();

            if (count > 0)
            {
                results = results.Take(count);
            }

#pragma warning disable CS8604 // Possible null reference argument.
            return results.ToList();
#pragma warning restore CS8604 // Possible null reference argument.
        }

        /// <summary>
        /// Removes a message from the queue.
        /// </summary>
        /// <param name="message">Contains the message to remove.</param>
        public void Remove(IQueueItem message)
        {
            var item = Messages.FirstOrDefault(m => m.Id == message.Id);

            if (item != null)
            {
                Messages.TryDequeue(out item);
            }
        }
    }
}
