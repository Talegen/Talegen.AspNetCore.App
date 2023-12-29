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
    using Talegen.AspNetCore.App.Models.Audit;
    using Talegen.Common.Models.Server.Queries;

    /// <summary>
    /// This interface defines the audit service contract.
    /// </summary>
    /// <typeparam name="TAuditModel">Contains the model type for audit records.</typeparam>
    public interface IAuditService<TAuditModel> where TAuditModel : class, IAuditModel
    {
        /// <summary>
        /// This method is used to read audit logs.
        /// </summary>
        /// <param name="filters">Contains the query filters.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a <see cref="PaginatedQueryResultModel{TQueryModel}"/> model results.</returns>
        Task<PaginatedQueryResultModel<TAuditModel>> ReadAuditLogsAsync(AuditLogQueryFilterModel filters, CancellationToken cancellationToken);

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
        Task<bool> LogAsync(AuditEvent auditEvent, AuditResult eventResult, string? clientAddress = null, string? message = null, Guid? userId = null, string? location = null, CancellationToken cancellationToken = default);
    }
}
