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
namespace Talegen.AspNetCore.App.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using Talegen.Common.Models.Server;
    using Talegen.Common.Core.Errors;
    using Talegen.AspNetCore.App.Properties;

    /// <summary>
    /// This class is used to handle and model a response for unhandled API controller exceptions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The logger instance used for logging exceptions.
        /// </summary>
        private readonly ILogger<ApiExceptionFilterAttribute> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute" /> class.
        /// </summary>
        /// <param name="logger">Contains a filter logger.</param>
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// This filter method is used to handle exceptions.
        /// </summary>
        /// <param name="context">Contains the context.</param>
        public override void OnException(ExceptionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }

            // copy error messages into response model error messages list.
            ErrorResponseModel responseModel = new ErrorResponseModel();
            responseModel.Messages.Add(new ErrorModel(context.Exception.Message, ErrorType.Critical, DateTime.UtcNow, context.Exception?.StackTrace ?? string.Empty));
            context.HttpContext.Response.StatusCode = (int)code;
            context.ExceptionHandled = true;
            context.Result = new JsonResult(responseModel);

            // If error is due to access denial
            this.logger?.LogCritical(context.Exception, Resources.ErrorGeneralWebText);
        }
    }
}
