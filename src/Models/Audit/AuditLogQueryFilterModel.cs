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
namespace Talegen.AspNetCore.App.Models.Audit
{
    using System.Collections.Generic;
    using Talegen.Common.Models.Server.Queries;

    /// <summary>
    /// This class implements the paging query request model for browsing audit logs.
    /// </summary>
    /// <seealso cref="Talegen.Common.Models.Server.Queries.PaginatedQueryRequestModel" />
    public class AuditLogQueryFilterModel : PaginatedQueryRequestModel
    {
        /// <summary>
        /// The events
        /// </summary>
        public List<AuditEvent> Events = new List<AuditEvent>();

        /// <summary>
        /// The results
        /// </summary>
        public List<AuditResult> Results = new List<AuditResult>();

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText { get; set; }
    }
}
