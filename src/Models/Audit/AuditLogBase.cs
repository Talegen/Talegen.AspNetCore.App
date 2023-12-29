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
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using Talegen.AspNetCore.App.Services.Audit;

    /// <summary>
    /// This entity class represents a security log event record within the data store.
    /// </summary>
    public class AuditLogBase : IAuditModel
    {
        /// <summary>
        /// Gets or sets the unique identity of the security event log record.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditLogId { get; set; }

        /// <summary>
        /// Gets or sets the event date time the event occurred.
        /// </summary>
        [Required]
        public DateTime EventDateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the optional associated user that generated the event.
        /// </summary>
        [MaxLength(128)]
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public AuditEvent Event { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        [MaxLength(30)]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the event result.
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public AuditResult Result { get; set; }

        /// <summary>
        /// Gets or sets an additional event message.
        /// </summary>
        [MaxLength(1000)]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the client's IP address.
        /// </summary>
        [MaxLength(30)]
        public string? ClientAddress { get; set; }

        /// <summary>
        /// Gets or sets the client's location.
        /// </summary>
        [MaxLength(250)]
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>The request.</value>
        [MaxLength(250)]
        public string? Request { get; set; }
    }
}
