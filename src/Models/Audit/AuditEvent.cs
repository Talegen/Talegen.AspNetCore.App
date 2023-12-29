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
    using System.Text.Json.Serialization;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Contains an enumerated list of audit event types.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AuditEvent
    {
        /// <summary>
        /// Login event.
        /// </summary>
        Login,

        /// <summary>
        /// Logout event.
        /// </summary>
        Logout,

        /// <summary>
        /// Read event.
        /// </summary>
        Read,

        /// <summary>
        /// Create event.
        /// </summary>
        Create,

        /// <summary>
        /// Update event.
        /// </summary>
        Update, 
        
        /// <summary>
        /// Delete event.
        /// </summary>
        Delete,

        /// <summary>
        /// Execute event.
        /// </summary>
        Execute
    }

    /// <summary>
    /// Contains an enumerated list of audit event results.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuditResult
    {
        /// <summary>
        /// Successful event.
        /// </summary>
        Success,

        /// <summary>
        /// Unsuccessful event.
        /// </summary>
        Fail,

        /// <summary>
        /// Informational event.
        /// </summary>
        Info
    }
}
