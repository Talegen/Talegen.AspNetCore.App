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
namespace Talegen.AspNetCore.App.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Talegen.Common.Core.Extensions;

    /// <summary>
    /// This class contains the base controller class to be inherited in ASP.net Core applications.
    /// </summary>
    /// <typeparam name="TILogger">Contains the logger type</typeparam>
    public abstract class BaseController<TILogger> : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{TILogger}" /> class.
        /// </summary>
        /// <param name="logger">Contains an instance of the default logger.</param>
        public BaseController(TILogger logger)
        {
            this.Logger = logger;
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        protected TILogger Logger { get; private set; }

        /// <summary>
        /// Gets the access token if available in the request header.
        /// </summary>
        public string AccessToken
        {
            get
            {
                string? result = string.Empty;
                HttpContext context = this.HttpContext;

                if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
                {
                    result = this.User.HasClaim(c => c.Type == "access_token") ?
                        this.User.FindFirstValue("access_token") :
                        AsyncHelper.RunSync(() => context.GetTokenAsync("access_token"));
                }

                return result ?? GetToken(this.Request);
            }
        }

        /// <summary>
        /// This method is used to retrieve the authentication token from the request header and/or the url if not in the header.
        /// </summary>
        /// <param name="request">Contains the request to search for an access token.</param>
        /// <returns>Returns the token from the request header.</returns>
        private static string GetToken(HttpRequest request)
        {
            string? result;

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string auth = request.Headers.Authorization.ConvertToString();

            if (string.IsNullOrWhiteSpace(auth))
            {
                result = request.Form?["token"];

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = request.Query?["token"];
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    // http://docs.identityserver.io/en/latest/topics/add_apis.html
                    result = request.Form?["access_token"];
                }
            }
            else
            {
                result = auth.Split(' ')[1];
            }

            return result.ConvertToString();
        }
    }
}
