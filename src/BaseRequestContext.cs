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
namespace Talegen.AspNetCore.App
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Caching.Distributed;
    using Talegen.AspNetCore.App.Services.Audit;
    using Talegen.AspNetCore.App.Services.Messaging;
    using Talegen.AspNetCore.App.Services.Security;
    using Talegen.AspNetCore.Web.Extensions;
    using Talegen.Common.Core.Errors;
    using Talegen.Common.Core.Extensions;

    /// <summary>
    /// This class defines the application context to be used in the application.
    /// </summary>
    public class BaseRequestContext
    {
        /// <summary>
        /// Contains the current user's identity value.
        /// </summary>
        private Guid currentUserId;

        /// <summary>
        /// Contains the current user's name.
        /// </summary>
        private string currentUserName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequestContext"/> class.
        /// </summary>
        /// <param name="cache">Contains the cache service.</param>
        /// <param name="security">Contains the security service.</param>
        /// <param name="messaging">Contains the messaging service.</param>
        /// <param name="audit">Contains the audit log service.</param>
        /// <param name="errorManager">Contains the error manager.</param>
        public BaseRequestContext(IDistributedCache cache,
            ISecurityService security,
            IMessagingService messaging,
            IAuditService<IAuditModel> audit,
            IErrorManager errorManager)
        {
            this.Cache = cache;
            this.Security = security;
            this.Messaging = messaging;
            this.AuditLog = audit;
            this.ErrorManager = errorManager;
        }

        /// <summary>
        /// Gets the application cache service.
        /// </summary>
        public IDistributedCache Cache { get; private set; }

        /// <summary>
        /// Gets the application security service.
        /// </summary>
        public ISecurityService Security { get; private set; }

        /// <summary>
        /// Gets the application messaging service.
        /// </summary>
        public IMessagingService Messaging { get; private set; }

        /// <summary>
        /// Gets the application audit log service.
        /// </summary>
        public IAuditService<IAuditModel> AuditLog { get; private set; }

        /// <summary>
        /// Gets the application error manager.
        /// </summary>
        public IErrorManager ErrorManager { get; private set; }

        /// <summary>
        /// Gets or sets the claims principal model.
        /// </summary>
        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// Gets the current user identity value.
        /// </summary>
        public Guid UserId
        {
            get
            {
                if (this.currentUserId == Guid.Empty && this.Principal != null)
                {
                    // get subject
                    this.currentUserId = this.Principal.SubjectId().ToGuid();
                }

                return this.currentUserId;
            }
        }

        /// <summary>
        /// Gets the current user name.
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(this.currentUserName))
                {
                    this.currentUserName = this.Principal.UserName();

                    if (string.IsNullOrWhiteSpace(this.currentUserName))
                    {
                        this.currentUserName = this.Principal.Email();
                    }
                }

                return this.currentUserName;
            }
        }

        /// <summary>
        /// Gets the client address.
        /// </summary>
        /// <value>The client address.</value>
        public string ClientAddress => this.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
    }
}
