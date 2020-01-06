#region Usings

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    /// <summary>
    /// Utilities for process executing.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return
        /// immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(
            this Process process,
            CancellationToken cancellationToken = default)
        {
            process.EnableRaisingEvents = true;

            var taskCompletionSource = new TaskCompletionSource<object>();

            EventHandler handler = null;
            handler = (sender, args) =>
                      {
                          process.Exited -= handler;
                          taskCompletionSource.TrySetResult(null);
                      };
            process.Exited += handler;

            if (cancellationToken != default)
            {
                cancellationToken.Register(
                    () =>
                    {
                        process.Exited -= handler;
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }
    }
}
