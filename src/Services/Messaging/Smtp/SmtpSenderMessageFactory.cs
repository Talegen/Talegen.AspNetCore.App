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
    using System.Globalization;
    using System.Resources;


    /// <summary>
    /// This class is used to create a new <see cref="ISenderMessage" /> object for the SMTP messaging service.
    /// </summary>
    public class SmtpSenderMessageFactory : ISenderMessageFactory
    {
        /// <summary>
        /// Contains the resource manager used for retrieving template content.
        /// </summary>
        private ResourceManager resourceManager;

        /// <summary>
        /// Contains an optional locale string override for message body resource lookup.
        /// </summary>
        private CultureInfo cultureInfoOverride;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpSenderMessageFactory" /> class.
        /// </summary>
        /// <param name="resourceManager">Contains the resource manager used for retrieving template content.</param>
        /// <param name="cultureInfoOverride">Contains an optional locale string override for message body resource lookup.</param>
        public SmtpSenderMessageFactory(ResourceManager? resourceManager = default, CultureInfo? cultureInfoOverride = default)
        {
            this.resourceManager = resourceManager ?? Properties.Resources.ResourceManager;
            this.cultureInfoOverride = cultureInfoOverride ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// This method is used to create a new <see cref="ISenderAddress" /> object for the SMTP messaging service.
        /// </summary>
        /// <param name="address">Contains the address.</param>
        /// <param name="displayName">Contains an optional display name.</param>
        /// <returns>Returns the address.</returns>
        public ISenderAddress CreateAddress(string address, string displayName = "")
        {
            return new SmtpMailAddress(address, displayName);
        }

        /// <summary>
        /// This method is used to create a new <see cref="ISenderMessage" /> and populate the message content with optional token values.
        /// </summary>
        /// <param name="from">Contains the message sender address.</param>
        /// <param name="recipients">Contains a list of <see cref="ISenderAddress" /> recipient objects.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="templateName">Contains a message template name to retrieve and load into the message body.</param>
        /// <param name="tokensList">Contains an optional tokens list for populating message body with token values.</param>
        /// <param name="resourceManager">Contains the resource manager used for retrieving template content.</param>
        /// <param name="cultureInfoOverride">Contains an optional locale string override for message body resource lookup.</param>
        /// <returns>Returns a new <see cref="ISenderMessage" /> containing template message content.</returns>
        public ISenderMessage CreateSenderMessage(ISenderAddress from, List<ISenderAddress> recipients, string subject, string templateName, Dictionary<string, string>? tokensList = null, ResourceManager? resourceManager = null, CultureInfo? cultureInfoOverride = null)
        {
            ArgumentNullException.ThrowIfNull(from);

            ArgumentNullException.ThrowIfNull(recipients);

            if (recipients.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(recipients));
            }

            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            if (resourceManager != null)
            {
                this.resourceManager = resourceManager;
            }   

            if (cultureInfoOverride != null)
            {
                this.cultureInfoOverride = cultureInfoOverride;
            }
            
            // default to English if no language specified.
            string textContent = TemplateExtensions.LoadTemplate(templateName + TemplateExtensions.TemplateTextSuffix, this.resourceManager, this.cultureInfoOverride);
            string htmlContent = TemplateExtensions.LoadTemplate(templateName + TemplateExtensions.TemplateHtmlSuffix, this.resourceManager, this.cultureInfoOverride);

            return CreateSenderMessage(from, recipients, subject, textContent, htmlContent, false, tokensList);
        }

        /// <summary>
        /// This method is used to create a new <see cref="ISenderMessage" /> and populate the message content with optional token values.
        /// </summary>
        /// <param name="from">Contains the message sender address.</param>
        /// <param name="to">Contains the message recipient address.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="textBody">Contains the body of the message in text format.</param>
        /// <param name="htmlBody">Contains the body of the message in HTML format.</param>
        /// <param name="tokensList">Contains an optional tokens list for populating message body with token values.</param>
        /// <param name="textBodyContentType">Contains an optional text body content type.</param>
        /// <param name="htmlBodyContentType">Contains an optional HTML body content type.</param>
        /// <returns>Returns a new <see cref="ISenderMessage" /> containing message content.</returns>
        public ISenderMessage CreateSenderMessage(ISenderAddress from, ISenderAddress to, string subject, string textBody, string htmlBody = "", Dictionary<string, string>? tokensList = null, string textBodyContentType = "text/plain", string htmlBodyContentType = "text/html")
        {
            return CreateSenderMessage(from, new List<ISenderAddress>() { to }, subject, textBody, htmlBody, false, tokensList, textBodyContentType, htmlBodyContentType);
        }

        /// <summary>
        /// This method is used to create a new <see cref="ISenderMessage" /> and populate the message content with optional token values.
        /// </summary>
        /// <param name="from">Contains the message sender address.</param>
        /// <param name="recipients">Contains a list of <see cref="ISenderAddress" /> recipient objects.</param>
        /// <param name="subject">Contains the message subject.</param>
        /// <param name="textBody">Contains the body of the message in text format.</param>
        /// <param name="htmlBody">Contains the body of the message in HTML format.</param>
        /// <param name="recipientsVisible">
        /// Contains a value indicating whether the recipients are visible to each other. If false, each recipient shall receive their own message.
        /// </param>
        /// <param name="tokensList">Contains an optional tokens list for populating message body with token values.</param>
        /// <param name="textBodyContentType">Contains an optional text body content type.</param>
        /// <param name="htmlBodyContentType">Contains an optional HTML body content type.</param>
        /// <returns>Returns a new <see cref="ISenderMessage" /> containing message content.</returns>
        public ISenderMessage CreateSenderMessage(ISenderAddress from, List<ISenderAddress> recipients, string subject, string textBody, string htmlBody = "", bool recipientsVisible = false, Dictionary<string, string>? tokensList = null, string textBodyContentType = "text/plain", string htmlBodyContentType = "text/html")
        {
            if (tokensList != null)
            {

                if (!string.IsNullOrEmpty(textBody))
                {
                    textBody = TemplateExtensions.ReplaceTokens(textBody, tokensList);
                }

                if (!string.IsNullOrEmpty(htmlBody))
                {
                    htmlBody = TemplateExtensions.ReplaceTokens(htmlBody, tokensList);
                }
            }

            return new SmtpMessage(from, recipients, subject, textBody, htmlBody, recipientsVisible, textBodyContentType, htmlBodyContentType);
        }
    }
}
