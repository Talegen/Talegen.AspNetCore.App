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
    using Serilog;
    using Talegen.AspNetCore.App.Properties;

    /// <summary>
    /// This class defines a background job that processes messages in the message queue.
    /// </summary>
    public class BackgroundMessagingJob : IHostedService, IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Contains the messaging service instance.
        /// </summary>
        private readonly IMessagingService service;

        /// <summary>
        /// Contains the execution count.
        /// </summary>
        private int executionCount;

        /// <summary>
        /// Contains a timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Contains a value indicating if dispose has been called.
        /// </summary>
        private bool disposed;

        #endregion

        #region Public Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundMessagingJob" /> class.
        /// </summary>
        /// <param name="service">Contains an instance of the messaging service.</param>
        public BackgroundMessagingJob(IMessagingService service)
        {
            ArgumentNullException.ThrowIfNull(service);
            this.service = service;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method is used to start executing the background job timer.
        /// </summary>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns the task to execute.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information(Properties.Resources.MessageQueueJobStartText);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(this.service.QueueProcessingIntervalSeconds));
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

            // restore any messages from queue folder.
            this.service.RestoreQueue();

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is used to stop the execution of a background job.
        /// </summary>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns the task result.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information(Resources.MessageQueueJobStopText);

            // store any messages in queue.
            this.service.StoreQueue();

            this.timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Disposes of internal disposable objects.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Disposes internal disposable objects.
        /// </summary>
        /// <param name="disposing">Contains a value indicating whether disposing is underway.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.timer?.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// This method is what executes the work to be performed.
        /// </summary>
        /// <param name="state">Contains thread state.</param>
        private void DoWork(object state)
        {
            Interlocked.Increment(ref this.executionCount);
            this.service.Process();
        }

        #endregion
    }
}
