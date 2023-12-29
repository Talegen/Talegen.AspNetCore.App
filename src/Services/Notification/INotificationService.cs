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
namespace Talegen.AspNetCore.App.Services.Notification
{
    using Talegen.AspNetCore.App.Models.Notifications;

    /// <summary>
    /// This interface defines the notification service contract.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// This method is used to send a notification message to the specific connected user.
        /// </summary>
        /// <typeparam name="TModel">Contains the model data type included in the package.</typeparam>
        /// <param name="notification">Contains the notification model to send.</param>
        /// <param name="notificationContext">Contains an application. context instance.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a task.</returns>
        Task NotifyUserAsync<TModel>(NotificationModel<TModel, string> notification, NotificationServiceContext? notificationContext = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method is used to send a notification message to all the connected users.
        /// </summary>
        /// <typeparam name="TModel">Contains the model data type included in the package.</typeparam>
        /// <param name="notification">Contains the notification model to send.</param>
        /// <param name="notificationContext">Contains an Application context instance.</param>
        /// <param name="excludeCurrentUser">Contains a value indicating whether the current user is excluded from receiving the signal message.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a task.</returns>
        Task NotifyAllAsync<TModel>(NotificationModel<TModel, string> notification, NotificationServiceContext? notificationContext = null, bool excludeCurrentUser = false, CancellationToken cancellationToken = default);

    }
}
