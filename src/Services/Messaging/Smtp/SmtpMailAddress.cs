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
namespace Talegen.AspNetCore.App.Services.Messaging.Smtp
{
    using Talegen.AspNetCore.App.Shared.Services.Messaging;

    /// <summary>
    /// This class represents a recipient address.
    /// </summary>
    public class SmtpMailAddress : ISenderAddress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMailAddress" /> class.
        /// </summary>
        public SmtpMailAddress()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMailAddress" /> class.
        /// </summary>
        /// <param name="address">Contains the address.</param>
        /// <param name="displayName">Contains an optional display name.</param>
        public SmtpMailAddress(string address, string displayName = "")
        {
            this.Address = address;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the address display name.
        /// </summary>
        public string DisplayName { get; set; }
    }
}