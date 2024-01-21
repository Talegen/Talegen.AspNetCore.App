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
    /// This interface is used to interact with the queue service.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Gets or sets the queue processing interval in seconds.
        /// </summary>
        int QueueProcessingIntervalSeconds { get; }

        /// <summary>
        /// This method is used to execute and process the queue messaging.
        /// </summary>
        void Process();

        /// <summary>
        /// This method is used to store the remaining queue items to disk during a shutdown event.
        /// </summary>
        void StoreQueue();

        /// <summary>
        /// This method is used to restore queue items from the disk during a startup.
        /// </summary>
        void RestoreQueue();

        /// <summary>
        /// This method is used to check on the existence of the queue path, and if it doesn't exist, it is created.
        /// </summary>
        /// <returns>Returns a value indicating if the path exists.</returns>
        bool CheckQueuePath();
    }
}
