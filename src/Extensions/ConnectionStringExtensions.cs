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
namespace Talegen.AspNetCore.App.Extensions
{
    using System.Reflection;

    /// <summary>
    /// This class contains extension methods for connection strings.
    /// </summary>
    public static class ConnectionStringExtensions
    {
        /// <summary>
        /// This method is used to parse a connection string into an object.
        /// </summary>
        /// <typeparam name="T">Contains the object type to return.</typeparam>
        /// <param name="connectionString">Contains the connection string to parse.</param>
        /// <returns>Returns a new instance of the object.</returns>
        public static T ParseToObject<T>(this string connectionString) where T : class, new()
        {
            T result = new();
            string[] parts = connectionString.Split(';');
            
            foreach (string part in parts)
            {
                string[] keyValuePair = part.Split('=');

                if (keyValuePair.Length == 2)
                {
                    PropertyInfo property = typeof(T).GetProperty(keyValuePair[0].Trim()) ?? throw new Exception("Invalid parameter");

                    property?.SetValue(result, keyValuePair[1].Trim());
                }
            }

            return result;
        }
    }
}
