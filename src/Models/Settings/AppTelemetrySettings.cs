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
    using Talegen.Common.Models.Server.Configuration;

    /// <summary>
    /// An enumerated list of telemetry types.
    /// </summary>
    public enum TelemetryType
    {
        /// <summary>
        /// No telemetry.
        /// </summary>
        None,

        /// <summary>
        /// Azure Application Insights.
        /// </summary>
        ApplicationInsights,

        /// <summary>
        /// Amazon CloudWatch.
        /// </summary>
        AwsCloudWatch
    }

    /// <summary>
    /// This class contains the telemetry settings.
    /// </summary>
    public class AppTelemetrySettings : MetricSettingsBase
    {
        /// <summary>
        /// Contains the default telemetry key name.
        /// </summary>
        public const string DefaultTelemetryKeyName = "ApplicationInsights";

        /// <summary>
        /// Gets or sets the instrumentation key.
        /// </summary>
        public string InstrumentationKey { get; set; } = DefaultTelemetryKeyName;

        /// <summary>
        /// Gets or sets the telemetry type.
        /// </summary>
        public TelemetryType TelemetryType { get; set; } = TelemetryType.None;
    }
}
