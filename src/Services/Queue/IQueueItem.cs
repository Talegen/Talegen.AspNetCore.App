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
    /// <summary>
    /// Contains an enumeration of queue item states.
    /// </summary>
    public enum QueueItemState
    {
        /// <summary>
        /// Item is new and has not been processed.
        /// </summary>
        New,

        /// <summary>
        /// Item is currently being processed.
        /// </summary>
        Processing,

        /// <summary>
        /// Item is complete.
        /// </summary>
        Complete,

        /// <summary>
        /// Item is available to retry.
        /// </summary>
        CanRetry,

        /// <summary>
        /// Item has failed to process.
        /// </summary>
        Failed
    }

    /// <summary>
    /// This interface defines the minimum implementation of a queue item.
    /// </summary>
    public interface IQueueItem
    {
        /// <summary>
        /// Gets or sets the identity of the queue item.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the queue date time.
        /// </summary>
        DateTime QueueDateTime { get; set; }

        /// <summary>
        /// Gets or sets the state of the queue item.
        /// </summary>
        QueueItemState State { get; set; }

        /// <summary>
        /// Gets or sets the rety count of the queue item.
        /// </summary>
        int RetryCount { get; set; }
    }
}
