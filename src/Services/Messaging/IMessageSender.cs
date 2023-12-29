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
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.App.Services.Queue;

    /// <summary>
    /// This interface defines a minimum implementation of a message sender.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// This method is used to send a message for the application.
        /// </summary>
        /// <param name="message">Contains the mail message to send.</param>
        void SendMessage(IQueueItem message);

        /// <summary>
        /// This method is used to send a message for the application.
        /// </summary>
        /// <param name="message">Contains the mail message to send.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns an async Task result.</returns>
        Task SendMessageAsync(IQueueItem message, CancellationToken cancellationToken = default);
    }
}