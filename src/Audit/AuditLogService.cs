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
namespace Talegen.AspNetCore.App.Audit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.App.Models.Audit;
    using Talegen.AspNetCore.App.Repository;
    using Talegen.AspNetCore.App.Services.Audit;
    using Talegen.Common.Models.Server.Queries;

    /// <summary>
    /// This class implements the audit log service.
    /// </summary>
    public class AuditLogService : IAuditService<IAuditModel>
    {
        /// <summary>
        /// Contains an instance of the audit data repository.
        /// </summary>
        private readonly IDataRepository<AuditLogBase, Guid> dataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogService"/> class.
        /// </summary>
        /// <param name="dataRepository">Contains an instance of the audit data repository.</param>
        public AuditLogService(IDataRepository<AuditLogBase, Guid> dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        /// <summary>
        /// This method is used to log an audit event.
        /// </summary>
        /// <param name="auditEvent">Contains the audit event.</param>
        /// <param name="eventResult">Contains the audit result.</param>
        /// <param name="clientAddress">Contains the client IP address.</param>
        /// <param name="message">Contains the message.</param>
        /// <param name="userId">Contains the user identity.</param>
        /// <param name="location">Contains the request location.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a result indicating whether the log was successful.</returns>
        public async Task<bool> LogAsync(AuditEvent auditEvent, AuditResult eventResult, string? clientAddress = null, string? message = null, Guid? userId = null, string? location = null, CancellationToken cancellationToken = default)
        {
            AuditLogBase auditLog = new AuditLogBase
            {
                Event  = auditEvent,
                Result = eventResult,
                ClientAddress = clientAddress,
                Message = message,
                UserId = userId,
                Location = location
            };

            var result = await this.dataRepository.AddAsync(auditLog, cancellationToken);

            return result?.Success ?? false;
        }

        /// <summary>
        /// This method is used to read audit logs.
        /// </summary>
        /// <param name="filters">Contains query filters for filtering results.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a result.</returns>
        /// <exception cref="NotImplementedException">This method is currently not implemented.</exception>
        public Task<PaginatedQueryResultModel<IAuditModel>> ReadAuditLogsAsync(AuditLogQueryFilterModel filters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
