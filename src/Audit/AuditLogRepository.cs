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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Talegen.AspNetCore.App.Models.Audit;
    using Talegen.AspNetCore.App.Repository;

    /// <summary>
    /// This class implements the audit log service.
    /// </summary>
    public class AuditLogRepository : IDataRepository<AuditLogBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditLogRepository"/> class.
        /// </summary>
        public AuditLogRepository()
        {
        }

        /// <summary>
        /// This method is used to add a new audit log entry.
        /// </summary>
        /// <param name="entity">Contains the entry to log.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<AuditLogBase>> AddAsync(AuditLogBase entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<AuditLogBase>() { Success = true, Results = entity });
        }

        /// <summary>
        /// This method is used to delete an audit log entry.
        /// </summary>
        /// <param name="id">Contains the identity of the log to delete.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<AuditLogBase>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<AuditLogBase>() { Success = true });
        }

        /// <summary>
        /// This method is used to delete an audit log entry.
        /// </summary>
        /// <param name="entity">Contains the entity to delete.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<AuditLogBase>> DeleteAsync(AuditLogBase entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<AuditLogBase>() { Success = true });
        }

        /// <summary>
        /// This method is used to get a list of all audit log entries.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<IEnumerable<AuditLogBase>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<IEnumerable<AuditLogBase>>() { Success = true, Results = new List<AuditLogBase>() });
        }

        /// <summary>
        /// This method is used to get an audit log entry by id.
        /// </summary>
        /// <param name="id">Contains the identity of the record to find.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<AuditLogBase>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<AuditLogBase>() { Success = true, Results = new AuditLogBase { UserId = id } });
        }

        /// <summary>
        /// This method is used to update an audit log entry.
        /// </summary>
        /// <param name="entity">Contains the entity to update.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        public Task<RepositoryResult<AuditLogBase>> UpdateAsync(AuditLogBase entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new RepositoryResult<AuditLogBase>() { Success = true });
        }

        /// <summary>
        /// This method is used to get an audit log entry by id.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns the repository result.</returns>
        /// <exception cref="NotImplementedException">This method is not implemented for this repository.</exception>
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
