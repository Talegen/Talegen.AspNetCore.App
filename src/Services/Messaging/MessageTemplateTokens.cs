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
        public const string FromUserName = "FROM_USERNAME";
        public const string FromFirstName = "FROM_FIRSTNAME";
        public const string FromFullName = "FROM_FULLNAME";
        public const string FromEmail = "FROM_EMAIL";
        public const string UserId = "USERID";
        public const string UserName = "USERNAME";
        public const string FirstName = "FIRSTNAME";
        public const string FullName = "FULLNAME";
        public const string UserEmail = "USEREMAIL";
        public const string CurrentUserName = "CURRENTUSERNAME";
        public const string RequestedUrl = "REQUESTEDURL";
        public const string Url = "URL";
        public const string UtcDateTime = "UTCDATETIME";
        public const string UtcDate = "UTCDATE";
        public const string DateTime = "DATETIME";
        public const string Date = "DATE";
        public const string Time = "TIME";
        public const string TimeZone = "TIMEZONE";
        public const string ErrorList = "ERRORLIST";
        public const string TextErrorList = "TEXTERRORLIST";
        public const string TenantName = "TENANTNAME";
        public const string TenantLevel = "TENANTLEVEL";
        public const string Email = "EMAIL";
        public const string Copyright = "COPYRIGHT";
        public const string Version = "VERSION";
    }
}
