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
namespace Talegen.AspNetCore.App.Services
{
    using System.ComponentModel.DataAnnotations;
    using Talegen.AspNetCore.App.Repository;
    using Talegen.Common.Core.Errors;

    /// <summary>
    /// This class implements the base service class for all services.
    /// </summary>
    /// <typeparam name="T">Contains the type of data repository model.</typeparam>
    /// <typeparam name="K">Contains the type of the data repository model key.</typeparam>
    public abstract class ServiceBase<T, K> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase{T, K}" /> class.
        /// </summary>
        /// <param name="requestContext">Contains a request context.</param>
        /// <param name="dataRepository">Contains a data repository.</param>
        protected ServiceBase(BaseRequestContext requestContext, IDataRepository<T, K> dataRepository)
        {
            this.RequestContext = requestContext;
            this.DataRepository = dataRepository;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        protected BaseRequestContext RequestContext { get; private set; }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        protected IDataRepository<T, K> DataRepository { get; private set; }

        /// <summary>
        /// This method is used to execute the data context save changes and catch all validation errors.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the number of rows updated on success.</returns>
        protected virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            bool resultValue = false;

            try
            {
                resultValue = await this.DataRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (InvalidOperationException invalidEx)
            {
                this.RequestContext.ErrorManager.Critical(invalidEx, ErrorCategory.Application);
            }
            catch (ValidationException validateEx)
            {
                this.RequestContext.ErrorManager.Critical(validateEx, ErrorCategory.Application);
            }
            catch (Exception otherEx)
            {
                this.RequestContext.ErrorManager.Critical(otherEx, ErrorCategory.Application);
            }

            return resultValue;
        }
    }
}
