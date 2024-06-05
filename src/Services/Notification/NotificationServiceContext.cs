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
    using Microsoft.Extensions.Caching.Distributed;
    using Talegen.AspNetCore.App.Services.Messaging;
    using Talegen.AspNetCore.App.Services.Security;
    using Talegen.Common.Core.Errors;

    /// <summary>
    /// This class contains the security service context to be used in the application.
    /// </summary>
    public class NotificationServiceContext : BaseRequestContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationServiceContext"/> class.
        /// </summary>
        /// <param name="context">Contains a base context to pass initialize the new context.</param>
        public NotificationServiceContext(BaseRequestContext context)
            : base(context.Cache, context.Security, context.Messaging, context.ErrorManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationServiceContext"/> class.
        /// </summary>
        /// <param name="cache">Contains a cache service.</param>
        /// <param name="security">Contains a security service.</param>
        /// <param name="messaging">Contains a messaging service.</param>
        /// <param name="errorManager">Contains an error manager service.</param>
        public NotificationServiceContext(IDistributedCache cache, ISecurityService security, IMessagingService messaging, IErrorManager errorManager)
            : base(cache, security, messaging, errorManager)
        {
        }
    }
}
