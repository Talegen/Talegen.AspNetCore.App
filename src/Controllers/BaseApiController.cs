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
    using System.Net;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Talegen.Common.Core.Errors;
    using Talegen.Common.Core.Extensions;
    using Talegen.Common.Models.Server;

    /// <summary>
    /// This class implements the base API controller class.
    /// </summary>
    [ApiController]
    public abstract class BaseApiController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController" /> class.
        /// </summary>
        /// <param name="requestContext">Contains an instance of request context.</param>
        public BaseApiController(BaseRequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        /// <summary>
        /// Gets the application request context.
        /// </summary>
        public BaseRequestContext RequestContext { get; private set; }

        /// <summary>
        /// This method is used to execute before the action method is executed.
        /// </summary>
        /// <param name="context">Contains the action context.</param>
        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // if the user is authenticated, then add the user identity to the request context.
            if (context.HttpContext != null)
            {
                // set the context http context
                this.RequestContext.HttpContext = context.HttpContext;

                if (context.HttpContext.User != null)
                {
                    // set the context user identity
                    this.RequestContext.Principal = context.HttpContext.User;
                }
            }

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="context">The action executed context.</param>
        [NonAction]
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing; this is a placeholder for derived classes.
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        /// <param name="next">
        /// The <see cref="ActionExecutionDelegate" /> to execute. Invoke this delegate in the body of <see cref="OnActionExecutionAsync" /> to continue
        /// execution of the action.
        /// </param>
        /// <returns>A <see cref="Task" /> instance.</returns>
        [NonAction]
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(next);

            OnActionExecuting(context);
            
            if (context.Result == null)
            {
                OnActionExecuted(await next());
            }
        }

        /// <summary>
        /// Overload the existing NotFound with a response that returns an action error type.
        /// </summary>
        /// <returns>Returns an error result containing a 404 message code.</returns>
        protected new IActionResult NotFound()
        {
            return this.CreateErrorResult(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// This method is used to return either an empty success (200) IHttpActionResult if no critical or validation errors are found within the context error
        /// manager class. If errors are found the method returns a bad request (400) response with a <see cref="ErrorResponseModel" /> result that overrides
        /// the success model.
        /// </summary>
        /// <returns>Returns either a empty OK result upon success or returns a bad request with an <see cref="ErrorResponseModel" /> result.</returns>
        protected IActionResult SuccessOrFailResult()
        {
            return this.SuccessOrFailResult<object>(null);
        }

        /// <summary>
        /// This method is used to return either a successful (200) IHttpActionResult if no critical or validation errors are found within the context error
        /// manager class. If errors are found the method returns a bad request (400) response with a <see cref="ErrorResponseModel" /> result that overrides
        /// the success model.
        /// </summary>
        /// <typeparam name="T">Contains the type of the data model to return on success.</typeparam>
        /// <param name="returnValue">Contains the data model to return on success.</param>
        /// <param name="routeName">Contains a a location route when the object was created, requiring a Location header response.</param>
        /// <param name="recordId">Contains an optional record identity value.</param>
        /// <returns>Returns either a successful model upon success or returns a bad request with an <see cref="ErrorResponseModel" /> result.</returns>
        protected IActionResult SuccessOrFailResult<T>(T? returnValue, string routeName = "", Guid recordId = default)
        {
            IActionResult result = this.NoContent();

            if (this.RequestContext.ErrorManager.HasCriticalErrors || this.RequestContext.ErrorManager.HasValidationErrors)
            {
                result = this.CreateErrorResult();
            }
            else if (returnValue != null)
            {
                result = string.IsNullOrWhiteSpace(routeName) ?
                    this.Ok(returnValue) : 
                    this.CreatedAtRoute(routeName, recordId != Guid.Empty ? new { id = recordId } : null, returnValue);
            }

            return result;
        }

        /// <summary>
        /// Adds the model state errors to the error manager.
        /// </summary>
        protected void AddModelErrors()
        {
            this.ModelState.Where(ms => ms.Value?.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).ToList().ForEach(ms =>
            {
                ms.Value?.Errors.ToList().ForEach(err =>
                {
                    this.RequestContext.ErrorManager.Validation(ms.Key, err.ErrorMessage, ErrorCategory.General);
                });
            });
        }

        /// <summary>
        /// This method is used to add identity result errors to the model error manager.
        /// </summary>
        /// <param name="result">Contains the identity result that contains errors.</param>
        protected void AddIdentityErrors(IdentityResult result)
        {
            ArgumentNullException.ThrowIfNull(result);

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            this.AddModelErrors();
        }

        /// <summary>
        /// This method is used to create the error response action result to return to the client.
        /// </summary>
        /// <param name="errorCode">Contains an optional status code to return in the response message. By default 400 Bad Request is returned.</param>
        /// <returns>Returns an action response containing an <see cref="ErrorResponseModel" /> object.</returns>
        private ObjectResult CreateErrorResult(HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            ErrorResponseModel responseModel = new ErrorResponseModel();

            // copy error messages into response model error messages list.
            responseModel.Messages.AddRange(this.RequestContext.ErrorManager.Messages.Select(m => new ErrorModel(m.Message, m.ErrorType, m.EventDate, m.PropertyName)));

            // determine if there is a suggested errorCode override
            var suggestedReturnCode = this.RequestContext.ErrorManager.Messages.OrderByDescending(m => m.SuggestedErrorCode).Select(m => m.SuggestedErrorCode).FirstOrDefault();

            // if a suggestedReturnCode is found and it is greater than the original error code...
            if (suggestedReturnCode > 0 && suggestedReturnCode > (int)errorCode)
            {
                // use the suggested return code.
                errorCode = (HttpStatusCode)suggestedReturnCode;
            }

            // create new response with the error response model in the Content.
            return this.StatusCode((int)errorCode, responseModel);
        }

        // <summary>
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
