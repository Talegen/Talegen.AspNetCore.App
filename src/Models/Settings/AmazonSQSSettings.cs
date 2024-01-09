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
namespace Talegen.AspNetCore.App.Models.Settings
{
    using Amazon;
    using Amazon.Runtime;

    /// <summary>
    /// This class contains the Amazon SQS settings.
    /// </summary>
    public class AmazonSQSSettings
    {
        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the service URL.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; } = "us-east-2";
        
        /// <summary>
        /// This method is used to get the AWS credentials.
        /// </summary>
        /// <returns>Returns new Basic credentials.</returns>
        public AWSCredentials GetCredentials()
        {
            AWSCredentials? credentials;
            if (string.IsNullOrWhiteSpace(this.AccessKey) || string.IsNullOrWhiteSpace(this.SecretKey))
            {
                credentials = FallbackCredentialsFactory.GetCredentials();
            }
            else
            {
                credentials = new BasicAWSCredentials(this.AccessKey, this.SecretKey);
            }

            return credentials;
        }

        /// <summary>
        /// This method is used to get the region endpoint.
        /// </summary>
        /// <returns>Returns the region endpoint</returns>
        public RegionEndpoint GetRegionEndpoint()
        {
            return !string.IsNullOrWhiteSpace(this.Region) ? RegionEndpoint.GetBySystemName(this.Region) : RegionEndpoint.USEast2;
        }
    }
}
