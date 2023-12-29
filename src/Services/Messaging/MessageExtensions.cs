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
namespace Talegen.AspNetCore.App.Services.Messaging
{
    using System.Globalization;
    using System.Linq;
    using Talegen.AspNetCore.App.Models;

    /// <summary>
    /// This class contains extensions for the message service.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// This method is used to determine if a message contains an HTML body.
        /// </summary>
        /// <param name="message">The message to evaluate.</param>
        /// <returns>Returns a value indicating if an HTML body was found.</returns>
        public static bool IsHtml(this ISenderMessage message)
        {
            return message.Bodies.Any(kvp => kvp.Key == MessageBodyType.Html);
        }

        /// <summary>
        /// This extension method is used to populate a message tokens list with commonly used values for an e-mail message.
        /// </summary>
        /// <param name="tokenValues">Contains the token values to add to.</param>
        /// <param name="requestedUri">Contains the requested Uri if any to use and base URL token value.</param>
        /// <param name="recipientUser">Contains the user to which the email is being sent.</param>
        /// <param name="senderUser">Contains the user to which the email is being sent from.</param>
        public static void InitializeBaseTokens(this Dictionary<string, string> tokenValues, Uri? requestedUri = default, IUserInfo? recipientUser = null, IUserInfo? senderUser = null)
        {
            ArgumentNullException.ThrowIfNull(tokenValues);

            if (senderUser != null)
            {
                if (!tokenValues.ContainsKey(MessageTemplateTokens.FromUserName))
                {
                    tokenValues.Add(MessageTemplateTokens.FromUserName, senderUser.UserName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.FromFirstName))
                {
                    tokenValues.Add(MessageTemplateTokens.FromFirstName, senderUser.FirstName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.FromFullName))
                {
                    tokenValues.Add(MessageTemplateTokens.FromFullName, senderUser.FullName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.FromEmail))
                {
                    tokenValues.Add(MessageTemplateTokens.FromEmail, senderUser.Email);
                }
            }

            if (recipientUser != null)
            {
                if (!tokenValues.ContainsKey(MessageTemplateTokens.UserId))
                {
                    tokenValues.Add(MessageTemplateTokens.UserId, recipientUser.UserId.ToString());
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.UserName))
                {
                    tokenValues.Add(MessageTemplateTokens.UserName, recipientUser.UserName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.FirstName))
                {
                    tokenValues.Add(MessageTemplateTokens.FirstName, recipientUser.FirstName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.FullName))
                {
                    tokenValues.Add(MessageTemplateTokens.FullName, recipientUser.FullName);
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.UserEmail))
                {
                    tokenValues.Add(MessageTemplateTokens.UserEmail, recipientUser.Email);
                }
            }

            if (requestedUri != null)
            {
                if (!tokenValues.ContainsKey(MessageTemplateTokens.RequestedUrl))
                {
                    tokenValues.Add(MessageTemplateTokens.RequestedUrl, requestedUri.ToString());
                }

                if (!tokenValues.ContainsKey(MessageTemplateTokens.Url))
                {
                    tokenValues.Add(MessageTemplateTokens.Url, requestedUri.GetLeftPart(UriPartial.Authority));
                }
            }
        }

        /// <summary>
        /// This method is used to render dates and times to the token values list for a recipient's specific culture.
        /// </summary>
        /// <param name="tokenValues">Contains the token values to add to.</param>
        /// <param name="recipientLanguageCode">Contains the recipient user locale code.</param>
        public static void InitializeDateTimeTokens(this Dictionary<string, string> tokenValues, string recipientLanguageCode = "")
        {
            ArgumentNullException.ThrowIfNull(tokenValues);

            CultureInfo recipientCulture = CultureInfo.CreateSpecificCulture(recipientLanguageCode);

            DateTime currentUtcNow = DateTime.UtcNow;
            DateTime currentNow = DateTime.Now;

            if (!tokenValues.ContainsKey(MessageTemplateTokens.UtcDateTime))
            {
                tokenValues.Add(MessageTemplateTokens.UtcDateTime, currentUtcNow.ToString(recipientCulture));
            }

            if (!tokenValues.ContainsKey(MessageTemplateTokens.UtcDate))
            {
                tokenValues.Add(MessageTemplateTokens.UtcDate, currentUtcNow.ToShortDateString());
            }

            if (!tokenValues.ContainsKey(MessageTemplateTokens.DateTime))
            {
                tokenValues.Add(MessageTemplateTokens.DateTime, currentNow.ToString(recipientCulture));
            }

            if (!tokenValues.ContainsKey(MessageTemplateTokens.Date))
            {
                tokenValues.Add(MessageTemplateTokens.Date, currentNow.ToShortDateString());
            }

            if (!tokenValues.ContainsKey(MessageTemplateTokens.Time))
            {
                tokenValues.Add(MessageTemplateTokens.Time, currentNow.ToShortTimeString());
            }
        }

    }
}
