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
namespace Talegen.AspNetCore.App.Services.Audit
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.App.Models.Audit;
    using Talegen.AspNetCore.App.Properties;
    using Talegen.AspNetCore.App.Repository;
    using Talegen.Common.Core.Errors;
    using Talegen.Common.Core.Extensions;
    using Talegen.Common.Models.Server.Queries;

    /// <summary>
    /// This class implements the base audit service class.
    /// </summary>
    public class AuditServiceBase : IAuditService<AuditLogBase>
    {
        /// <summary>
        /// Contains a reference to the request context.
        /// </summary>
        private readonly BaseRequestContext requestContext;

        /// <summary>
        /// Contains a reference to the data repository.
        /// </summary>
        private readonly IDataRepository<AuditLogBase> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditServiceBase" /> class.
        /// </summary>
        /// <param name="requestContext">Contains a reference to the request context.</param>
        /// <param name="repository">Contains a reference to the data repository.</param>
        public AuditServiceBase(BaseRequestContext requestContext, IDataRepository<AuditLogBase> repository)
        {
            this.requestContext = requestContext;
            this.repository = repository;
        }

        /// <summary>
        /// This method is used to log an audit event.
        /// </summary>
        /// <param name="auditEvent">Contains the audit event.</param>
        /// <param name="eventResult">Contains the audit result.</param>
        /// <param name="clientAddress">Remote client address.</param>
        /// <param name="message">Optional event message.</param>
        /// <param name="userId">Optional user identity.</param>
        /// <param name="location">Optional location.</param>
        /// <param name="cancellationToken">Optional Cancellation token.</param>
        /// <returns>Returns a value indicatng success.</returns>
        public virtual async Task<bool> LogAsync(AuditEvent auditEvent, AuditResult eventResult, string? clientAddress = null, string? message = null, Guid? userId = null, string? location = null, CancellationToken cancellationToken = default)
        {
            var auditLog = new AuditLogBase
            {
                Event = auditEvent,
                Result = eventResult,
                Message = message,
                UserId = userId,
                ClientAddress = clientAddress,
                Location = location
            };

            var result = await this.repository.AddAsync(auditLog, cancellationToken);

            return result?.Success ?? false;
        }

        /// <summary>
        /// This method is used to read audit logs.
        /// </summary>
        /// <param name="filters">Contains the query filters.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a <see cref="PaginatedQueryResultModel{TQueryModel}"/> model results.</returns>
        public virtual async Task<PaginatedQueryResultModel<AuditLogBase>> ReadAuditLogsAsync(AuditLogQueryFilterModel filters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(filters);

            IQueryable<AuditLogBase> query = await this.BuildPagedResultsQueryAsync(filters, cancellationToken);
            return await this.ExecutePagedQueryAsync(query, filters, cancellationToken);
        }

        /// <summary>
        /// This method is used to build the query LINQ for reuse between retrieval methods in the service class.
        /// </summary>
        /// <param name="filters">Contains the event log filter parameters object.</param>
        /// <param name="cancellationToken">Contains the cancellation token.</param>
        /// <returns>Returns an <see cref="IQueryable" /> query class built using optional parameters.</returns>
        /// <exception cref="ArgumentNullException">The filters argument is null.</exception>
        /// <exception cref="Exception">An exception occurred while building the query.</exception>
        protected virtual async Task<IQueryable<AuditLogBase>> BuildPagedResultsQueryAsync(AuditLogQueryFilterModel filters, CancellationToken cancellationToken)
        {
            IQueryable<AuditLogBase>? query = null;

            if (filters.Limit <= 0)
            {
                // we cannot allow an unlimited amount of results, so defaulting to 25 if no or invalid value is passed in
                filters.Limit = 25;
            }

            if (filters.Page <= 0)
            {
                filters.Page = 1;
            }

            try
            {
                var results = await this.repository.GetAllAsync(cancellationToken);

                if (results.Success && results.Results != null)
                {
                    query = results.Results?.AsQueryable();

                    if (query != null)
                    {

                        filters.Events.ForEach(ev =>
                        {
                            query = query.Where(q => q.Event == ev);
                        });

                        filters.Results.ForEach(re =>
                        {
                            query = query.Where(r => r.Result == re);
                        });

                        if (filters.Sort.Any())
                        {
                            for (int i = 0; i < filters.Sort.Length; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(filters.Sort[i]))
                                {
                                    query = query.OrderByName(filters.Sort[i], filters.Direction[i]);
                                }
                            }
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.EventDateTime);
                        }

                        if (filters.Limit > 0)
                        {
                            // set paging selection
                            query = filters.Page > 1 ? query.Skip((filters.Page - 1) * filters.Limit).Take(filters.Limit) : query.Take(filters.Limit);
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(Resources.ErrorNoRepositoryResultFoundText, typeof(IDataRepository<AuditLogBase>).Name, nameof(this.BuildPagedResultsQueryAsync)));
                    }
                }
            }
            catch (Exception ex)
            {
                // log the error
                this.requestContext.ErrorManager.Critical(ex, ErrorCategory.Application);
            }

            if (query == null)
            {
                this.requestContext.ErrorManager.Critical(string.Format(Resources.ErrorNoQueryText, nameof(this.BuildPagedResultsQueryAsync)), ErrorCategory.Application);
            }

#pragma warning disable CS8603 // Possible null reference return.
            return query;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// This method is used to execute the passed query LINQ and return the results as an <see cref="AuditLogBase" /> list.
        /// </summary>
        /// <param name="query">Contains the LINQ statement to execute.</param>
        /// <param name="filters">Contains the filters used for the query</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns the results of the query in an <see cref="PaginatedQueryResultModel{AuditLogBase}" /> model.</returns>
        /// <exception cref="ArgumentNullException">The query argument is null.</exception>
        protected virtual async Task<PaginatedQueryResultModel<AuditLogBase>> ExecutePagedQueryAsync(IQueryable<AuditLogBase> query, AuditLogQueryFilterModel filters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            PaginatedQueryResultModel<AuditLogBase> result = new PaginatedQueryResultModel<AuditLogBase>
            {
                Results = query.ToList()
            };

            // if the query returned less than the limit, and we're on the first page, we can use that count for the component results otherwise, we must run a
            // separate query to determine the total count
            result.TotalCount = filters.Page == 1 && result.Results.Count <= filters.Limit ? result.Results.Count : query.Count();

            return await Task.FromResult(result);
        }
    }
}
