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
    using System.Net.Mail;
    using System.Net.Mime;
    using Talegen.AspNetCore.App.Shared.Services.Messaging;

    /// <summary>
    /// This class contains extension methods for the <see cref="SmtpMessage" /> class.
    /// </summary>
    public static class SmtpMessageExtensions
    {
        /// <summary>
        /// This message is used to convert a <see cref="SmtpMessage" /> to a <see cref="MailMessage" /> object.
        /// </summary>
        /// <param name="message">Contains the sender message to convert.</param>
        /// <returns>Returns a new <see cref="MailMessage" /> object.</returns>
        public static MailMessage ToMailMessage(this SmtpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            MailMessage result = new MailMessage
            {
                From = new MailAddress(message.From.Address)
            };

            message.Recipients.ForEach(rcpt =>
            {
                result.To.Add(rcpt.Address);
            });

            result.Subject = message.Subject;
            bool hasTextBody = message.Bodies.Any(kvp => kvp.Key == MessageBodyType.Text);

            if (hasTextBody)
            {
                result.Body = message.Bodies.First(kvp => kvp.Key == MessageBodyType.Text).Value;
                result.IsBodyHtml = false;
            }

            if (message.IsHtml())
            {
                string htmlBody = message.Bodies.First(kvp => kvp.Key == MessageBodyType.Html).Value;

                // if there is only an HTML body, use it as the body. Otherwise, add it as an alternate view.
                if (!hasTextBody)
                {
                    result.Body = htmlBody;    
                    result.IsBodyHtml = true;
                }
                else
                {
                    result.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, new ContentType(message.HtmlContentType)));
                }
            }

            return result;
        }

        /// <summary>
        /// This extension is used to determine if a specified e-mail address string is within a MailAddressCollection of MailAddress objects.
        /// </summary>
        /// <param name="collection">Contains the collection of mail address objects to search.</param>
        /// <param name="emailAddress">Contains the e-mail address to find.</param>
        /// <returns>Returns a value indicating whether the e-mail address is a member of the specified mail address collection.</returns>
        public static bool Contains(this MailAddressCollection collection, string emailAddress)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                throw new ArgumentNullException(nameof(emailAddress));
            }

            return collection.Contains(new MailAddress(emailAddress));
        }
    }
}
