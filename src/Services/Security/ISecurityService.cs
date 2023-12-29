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
namespace Talegen.AspNetCore.App.Services.Security
{
    /// <summary>
    /// Contains an enumeration of permission types.
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// Create permission type.
        /// </summary>
        Create = 1,

        /// <summary>
        /// Read permission type.
        /// </summary>
        Read = 2,

        /// <summary>
        /// Update permission type.
        /// </summary>
        Update = 4,

        /// <summary>
        /// Delete permission type.
        /// </summary>
        Delete = 8,

        /// <summary>
        /// Delete all permission type.
        /// </summary>
        DeleteAll = 16,

        /// <summary>
        /// Execute permission type.
        /// </summary>
        Execute = 32,

        /// <summary>
        /// All permission type.
        /// </summary>
        All = 256
    }

    /// <summary>
    /// This interface defines a minimal role-based security service implementation.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// This method is used to determine if the current user has the specified permission.
        /// </summary>
        /// <param name="userId">Contains the user identity.</param>
        /// <param name="permissionId">Contains the permission identity.</param>
        /// <param name="permissionType">Contains the permission type to check.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a value indicating whether the user has the specified permission type for the given permission.</returns>
        public Task<bool> HasPermissionAsync(Guid userId, Guid permissionId, PermissionType permissionType, CancellationToken cancellationToken = default);
    }
}
