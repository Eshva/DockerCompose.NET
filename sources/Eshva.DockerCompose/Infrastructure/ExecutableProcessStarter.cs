#region Usings

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    /// <summary>
    /// Process starter for executables.
    /// </summary>
    public sealed class ExecutableProcessStarter : IProcessStarter
    {
        /// <summary>
        /// Creates process starter with path to <paramref name="executable"/>.
        /// </summary>
        /// <param name="executable">
        /// Path to the starting executable.
        /// </param>
        public ExecutableProcessStarter(string executable)
        {
            _executable = executable;
        }

        /// <inheritdoc cref="IProcessStarter.StandardOutput"/>
        public StringBuilder StandardOutput { get; } = new StringBuilder();

        /// <inheritdoc cref="IProcessStarter.StandardError"/>
        public StringBuilder StandardError { get; } = new StringBuilder();

        /// <inheritdoc cref="IProcessStarter.Start"/>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task<int> Start(string arguments, TimeSpan executionTimeout)
        {
            var outputTextWriter = new StringWriter(StandardOutput);
            var errorTextWriter = new StringWriter(StandardError);

            var processStartInfo = new ProcessStartInfo(_executable, arguments)
                                   {
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true,
                                       CreateNoWindow = true,
                                       UseShellExecute = false
                                   };
            Process process;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Exception exception)
            {
                throw new ProcessStartException($"An error occured during starting the executable '{_executable}'.", exception);
            }

            if (process == null)
            {
                throw new InvalidOperationException("Process not started.");
            }

            try
            {
                var timeoutTokenSource = new CancellationTokenSource(executionTimeout);
                await Task.WhenAll(
                    process.WaitForExitAsync(timeoutTokenSource.Token),
                    ReadAsync(
                        handler =>
                        {
                            process.OutputDataReceived += handler;
                            process.BeginOutputReadLine();
                        },
                        handler => process.OutputDataReceived -= handler,
                        outputTextWriter,
                        timeoutTokenSource.Token),
                    ReadAsync(
                        handler =>
                        {
                            process.ErrorDataReceived += handler;
                            process.BeginErrorReadLine();
                        },
                        handler => process.ErrorDataReceived -= handler,
                        errorTextWriter,
                        timeoutTokenSource.Token)
                );

                return process.ExitCode;
            }
            catch (TaskCanceledException exception)
            {
                throw new TimeoutException($"The process is not finished during {executionTimeout:g}", exception);
            }
            finally
            {
                process.Dispose();
            }
        }

        private static Task ReadAsync(
            Action<DataReceivedEventHandler> addHandler,
            Action<DataReceivedEventHandler> removeHandler,
            TextWriter textWriter,
            CancellationToken cancellationToken = default)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();

            addHandler(Handler);

            if (cancellationToken != default)
            {
                cancellationToken.Register(
                    () =>
                    {
                        removeHandler(Handler);
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;

            void Handler(object sender, DataReceivedEventArgs eventArgs)
            {
                if (eventArgs.Data == null)
                {
                    removeHandler(Handler);
                    taskCompletionSource.TrySetResult(null);
                }
                else
                {
                    textWriter.WriteLine(eventArgs.Data);
                }
            }
        }

        private readonly string _executable;
    }
}
