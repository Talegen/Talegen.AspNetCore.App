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
namespace Talegen.AspNetCore.App.Repository
{
    /// <summary>
    /// This class implements the minimum implementation for a data connection info class.
    /// </summary>
    public class ApiDataConnectionInfo : IDataConnectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDataConnectionInfo" /> class.
        /// </summary>
        /// <param name="dataConnectionString">Contains the initialized connection string.</param>
        public ApiDataConnectionInfo(string dataConnectionString = "") 
        {
            this.ConnectionString = dataConnectionString;
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
