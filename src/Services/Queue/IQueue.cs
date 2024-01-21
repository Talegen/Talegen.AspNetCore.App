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
namespace Talegen.AspNetCore.App.Services.Queue
{
    using System.Collections.Concurrent;

    /// <summary>
    /// This interface defines the minimum implementation of a queue service.
    /// </summary>
    /// <typeparam name="IQueueItem">Contains the message implementation to interact with.</typeparam>
    public interface IQueue<IQueueItem>
    {
        /// <summary>
        /// Gets the thread-safe queue for handling messages.
        /// </summary>
        ConcurrentQueue<IQueueItem> Messages { get; }

        /// <summary>
        /// Adds a new sender message to the queue to be processed.
        /// </summary>
        /// <param name="message">Contains the message to process.</param>
        void Add(IQueueItem message);

        /// <summary>
        /// Removes the specified message.
        /// </summary>
        /// <param name="message">Contains the message to remove</param>
        void Remove(IQueueItem message);

        /// <summary>
        /// Clears the queue.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the next available messages in the queue to process.
        /// </summary>
        /// <param name="count">Contains the number of messages to process. Default is 0, indicating all available messages to process.</param>
        /// <returns>Returns an enumerable list of queue items to process.</returns>
        IEnumerable<IQueueItem> Peek(int count = 0);
    }
}
