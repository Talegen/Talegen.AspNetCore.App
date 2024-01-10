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
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// This class contains methods to help with streaming file data from a request.
    /// </summary>
    public static class FileStreamingHelper
    {
        /// <summary>
        /// Contains the default form options.
        /// </summary>
        private static readonly FormOptions DefaultFormOptions = new FormOptions();

        /// <summary>
        /// Contains the reader buffer size.
        /// </summary>
        private static int ReaderBufferSize = 4096;

        /// <summary>
        /// This method streams a file from a request to a target stream.
        /// </summary>
        /// <param name="request">Contains the <see cref="HttpRequest" /> object.</param>
        /// <param name="targetStream">Contains a target <see cref="Stream" /> object that will receive form data found in the request.</param>
        /// <returns>Returns a form value validator object for any multipart form data found in the request.</returns>
        public static async Task<FormValueProvider> StreamFile(this HttpRequest request, Stream targetStream)
        {
            ArgumentNullException.ThrowIfNull(request);
            
            if (request.ContentType == null)
            {
                Exception exception = new Exception("No content type was specified.");
                throw exception;
            }

            if (!MultipartRequestHelper.IsMultipartContentType(request.ContentType))
            {
                Exception exception = new Exception($"Expected a multipart request, but got {request.ContentType}");
                throw exception;
            }

            // Used to accumulate all the form url encoded key value pairs in the request.
            var formAccumulator = new KeyValueAccumulator();

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), DefaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, request.Body);
            var section = await reader.ReadNextSectionAsync().ConfigureAwait(false);
            
            while (section != null)
            {
                bool hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader && contentDisposition != null)
                {
                    // if we have a file, then copy it to the target stream
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        await section.Body.CopyToAsync(targetStream).ConfigureAwait(false);
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // else if we have a form data, then read the value, but
                        // do not limit the key name length here because the multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);

                        using (var streamReader = new StreamReader(section.Body, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: ReaderBufferSize, leaveOpen: true))
                        {
                            // the value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync().ConfigureAwait(false);

                            // check for undefined values
                            if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = string.Empty;
                            }

                            // if we can parse the content disposition header, then we can add it to the form accumulator
                            if (key.HasValue)
                            {
                                formAccumulator.Append(key.Value, value);
                            }

                            // if the key count is greater than the limit, then throw an exception
                            if (formAccumulator.ValueCount > DefaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {DefaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                // Drain any remaining section body that has not been consumed and read the headers for the next section.
                section = await reader.ReadNextSectionAsync().ConfigureAwait(false);
            }

            // Bind form data to a model
            var formValueProvider = new FormValueProvider(BindingSource.Form, new FormCollection(formAccumulator.GetResults()), CultureInfo.CurrentCulture);

            return formValueProvider;
        }

        /// <summary>
        /// This method gets the encoding from a multipart section.
        /// </summary>
        /// <param name="section">An <see cref="MultipartSection" /> object.</param>
        /// <returns>Returns the encoding object found in the multipart section if any, otherwise defaults to UTF8.</returns>
        private static Encoding GetEncoding(MultipartSection section)
        {
            ArgumentNullException.ThrowIfNull(section);
            Encoding encoding = Encoding.UTF8;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in most cases.
#pragma warning disable SYSLIB0001 // Type or member is obsolete
            if (hasMediaTypeHeader && mediaType != null && !Encoding.UTF7.Equals(mediaType.Encoding))
            {
                encoding = mediaType.Encoding ?? Encoding.UTF8;
            }
#pragma warning restore SYSLIB0001 // Type or member is obsolete

            return encoding;
        }
    }
}