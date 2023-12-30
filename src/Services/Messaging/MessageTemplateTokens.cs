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
    /// <summary>
    /// Contains token constants for messaging templates.
    /// </summary>
    public static class MessageTemplateTokens
    {
        /// <summary>
        /// Contains the token for the from user name.
        /// </summary>
        public const string FromUserName = "FROM_USERNAME";

        /// <summary>
        /// Contains the token for the from first name.
        /// </summary>
        public const string FromFirstName = "FROM_FIRSTNAME";

        /// <summary>
        /// Contains the token for the from full name.
        /// </summary>
        public const string FromFullName = "FROM_FULLNAME";

        /// <summary>
        /// Contains the token for the from email address.
        /// </summary>
        public const string FromEmail = "FROM_EMAIL";

        /// <summary>
        /// Contains the token for the user identity.
        /// </summary>
        public const string UserId = "USERID";

        /// <summary>
        /// Contains the token for the user name.
        /// </summary>
        public const string UserName = "USERNAME";

        /// <summary>
        /// Contains the token for the first name.
        /// </summary>
        public const string FirstName = "FIRSTNAME";

        /// <summary>
        /// Contains the token for the full name.
        /// </summary>
        public const string FullName = "FULLNAME";

        /// <summary>
        /// Contains the token for the user email address.
        /// </summary>
        public const string UserEmail = "USEREMAIL";

        /// <summary>
        /// Contains the token for the current user name.
        /// </summary>
        public const string CurrentUserName = "CURRENTUSERNAME";

        /// <summary>
        /// Contains the token for the request URL.
        /// </summary>
        public const string RequestedUrl = "REQUESTEDURL";

        /// <summary>
        /// Contains a token for a URL.
        /// </summary>
        public const string Url = "URL";

        /// <summary>
        /// Contains a token for UTC Date Time.
        /// </summary>
        public const string UtcDateTime = "UTCDATETIME";

        /// <summary>
        /// Contains a token for the UTC date.
        /// </summary>
        public const string UtcDate = "UTCDATE";

        /// <summary>
        /// Contains a token for local date time.
        /// </summary>
        public const string DateTime = "DATETIME";

        /// <summary>
        /// Contains a token for the local date.
        /// </summary>
        public const string Date = "DATE";

        /// <summary>
        /// Contains a token for the local time.
        /// </summary>
        public const string Time = "TIME";

        /// <summary>
        /// Contains a token for the time zone.
        /// </summary>
        public const string TimeZone = "TIMEZONE";

        /// <summary>
        /// Contains a token for a list of errors.
        /// </summary>
        public const string ErrorList = "ERRORLIST";

        /// <summary>
        /// Contains a token for a list of text errors.
        /// </summary>
        public const string TextErrorList = "TEXTERRORLIST";

        /// <summary>
        /// Contains a token for a tenant name.
        /// </summary>
        public const string TenantName = "TENANTNAME";

        /// <summary>
        /// Contains a token for a tenant level.
        /// </summary>
        public const string TenantLevel = "TENANTLEVEL";

        /// <summary>
        /// Contains a token for an email address.
        /// </summary>
        public const string Email = "EMAIL";

        /// <summary>
        /// Contains a token for a copyright.
        /// </summary>
        public const string Copyright = "COPYRIGHT";

        /// <summary>
        /// Contains a token for an app version.
        /// </summary>
        public const string Version = "VERSION";
    }
}
