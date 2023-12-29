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
namespace Talegen.AspNetCore.App.Models
{
    /// <summary>
    /// This interface defines the minimum implementation of a user information object.
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the user email address.
        /// </summary>
        string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets the full name of the user.
        /// </summary>
        string FullName { get; }
    }
}
