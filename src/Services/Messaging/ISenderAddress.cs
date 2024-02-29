﻿/*
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
namespace Talegen.AspNetCore.App.Services.Messaging
{
    /// <summary>
    /// This interface defines a minimum implementation of a sender address.
    /// </summary>
    public interface ISenderAddress
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        string DisplayName { get; set; }
    }
}
