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
namespace Talegen.AspNetCore.App
{
    /// <summary>
    /// This class defines the main Application Server Exception to be thrown within Applications. 
    /// </summary>
    public class AppServerException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServerException"/> class.
        /// </summary>
        public AppServerException()
            : base(string.Empty, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServerException"/> class.
        /// </summary>
        /// <param name="message">Contains the exception message.</param>
        public AppServerException(string? message)
            : base(message, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServerException"/> class.
        /// </summary>
        /// <param name="message">Contains the exception message.</param>
        /// <param name="innerException">Contains the inner exception.</param>
        public AppServerException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
