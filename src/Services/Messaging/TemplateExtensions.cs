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
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using Talegen.AspNetCore.App.Properties;

    /// <summary>
    /// This class contains template rendering methods for messages and text within the application.
    /// </summary>
    public static class TemplateExtensions
    {
        /// <summary>
        /// Contains the default language code.
        /// </summary>
        public const string DefaultLanguageCode = "en-US";

        /// <summary>
        /// Contains the suffix for HTML templates.
        /// </summary>
        public const string TemplateHtmlSuffix = "Html";

        /// <summary>
        /// Contains the suffix for text templates.
        /// </summary>
        public const string TemplateTextSuffix = "Text";

        /// <summary>
        /// This method is used to load a template message body from a template file.
        /// </summary>
        /// <param name="templateName">Contains the file name of the template to load.</param>
        /// <param name="resourceManager">Contains the resource manager containing the template contents.</param>
        /// <param name="cultureInfoOverride">Contains a culture info override to render a specific template language.</param>
        /// <returns>Returns the message body of the template.</returns>
        public static string LoadTemplate(string templateName, ResourceManager resourceManager, CultureInfo? cultureInfoOverride = null)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentException(Resources.TemplateFileNameRequired, nameof(templateName));
            }

            ArgumentNullException.ThrowIfNull(resourceManager);

            string content = string.Empty;
            cultureInfoOverride ??= CultureInfo.CurrentCulture;

            try
            {
                content = resourceManager.GetString(templateName, cultureInfoOverride) ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new AppServerException(string.Format(Resources.ErrorMessageTemplateMissingText, templateName), ex);
            }
            
            return content;
        }

        /// <summary>
        /// This method is used to replace the template tokens with the specified token values.
        /// </summary>
        /// <param name="content">Contains the content within to find and replace tokens.</param>
        /// <param name="tokensList">Contains the tokens used for replacement.</param>
        /// <returns>Returns the new content with found tokens replaced.</returns>
        public static string ReplaceTokens(string content, Dictionary<string, string> tokensList)
        {
            ArgumentNullException.ThrowIfNull(content);
            ArgumentNullException.ThrowIfNull(tokensList);

            string result = content;

            foreach (var kvp in tokensList)
            {
                result = result.Replace($"${kvp.Key.ToUpperInvariant()}$", kvp.Value);
            }

            return result;
        }
    }
}