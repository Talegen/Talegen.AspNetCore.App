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
    using System;
    using System.IO;
    using Microsoft.Net.Http.Headers;
    using Properties;

    /// <summary>
    /// This helper class is used to assist with Multipart Requests.
    /// </summary>
    public static class MultipartRequestHelper
    {
        /// <summary>
        /// This method is used to determine the boundaries of request content types.
        /// </summary>
        /// <param name="contentType">An object of <see cref="MediaTypeHeaderValue" />.</param>
        /// <param name="lengthLimit">A value indicating max length.</param>
        /// <returns>Returns a string containing the boundary value if found.</returns>
        /// <remarks>
        /// The boundary is defined using a unique code. E.g., Content-Type: multipart/form-data; boundary="----WebKitFormBoundaryxyz123" 
        /// The specification says 70 characters is a reasonable limit.
        /// </remarks>
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit = 70)
        {
            ArgumentNullException.ThrowIfNull(contentType);

            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException(Resources.ErrorContentTypeBoundryNotFoundText);
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(string.Format(Resources.ErrorContentTypeBoundaryLimitExceededText, lengthLimit));
            }

            return boundary;
        }

        /// <summary>
        /// This method is used to determine if a request content type is multipart data.
        /// </summary>
        /// <param name="contentType">String value to be evaluated.</param>
        /// <returns>Returns a value indicating whether or not the content type is multipart.</returns>
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) && contentType.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// This method is used to determine if a request contains form data.
        /// </summary>
        /// <param name="contentDisposition">Contains an <see cref="ContentDispositionHeaderValue" /> object.</param>
        /// <returns>Returns a value indicating whether the content disposition has a form data type.</returns>
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data", StringComparison.OrdinalIgnoreCase)
                   && string.IsNullOrEmpty(contentDisposition.FileName.Value) 
                   && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        /// <summary>
        /// This method is used to determine if a request contains file data.
        /// </summary>
        /// <param name="contentDisposition">Contains an <see cref="ContentDispositionHeaderValue" /> object.</param>
        /// <returns>Returns a value indicating whether the content disposition has a file type.</returns>
        /// <remarks>
        /// E.g., Content-Disposition: form-data; name="profilePhoto1"; filename="profile-photo.jpg"
        /// </remarks>
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data", StringComparison.OrdinalIgnoreCase)
                   && (!string.IsNullOrEmpty(contentDisposition.FileName.Value) || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}