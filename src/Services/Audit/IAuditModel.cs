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

    /// <summary>
    /// This interface defines the properties and methods for an audit model.
    /// </summary>
    public interface IAuditModel
    {
        /// <summary>
        /// Gets or sets the unique identity of the security event log record.
        /// </summary>
        long AuditLogId { get; set; }

        /// <summary>
        /// Gets or sets the event date time the event occurred.
        /// </summary>
        DateTime EventDateTime { get; set; } 

        /// <summary>
        /// Gets or sets the optional associated user that generated the event.
        /// </summary>
        Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        AuditEvent Event { get; set; }

        /// <summary>
        /// Gets or sets the event action.
        /// </summary>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the event result.
        /// </summary>
        AuditResult Result { get; set; }

        /// <summary>
        /// Gets or sets an additional event message.
        /// </summary>
        string? Message { get; set; }

        /// <summary>
        /// Gets or sets the client address.
        /// </summary>
        string? ClientAddress { get; set; }

        /// <summary>
        /// Gets or sets the client location.
        /// </summary>
        string? Location { get; set; }

        /// <summary>
        /// Gets or sets the request data.
        /// </summary>
        string? Request { get; set; }
    }
}
