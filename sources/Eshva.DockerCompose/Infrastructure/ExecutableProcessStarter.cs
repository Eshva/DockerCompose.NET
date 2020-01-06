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
        public TextReader StandardOutput { get; private set; } = new StringReader(string.Empty);

        /// <inheritdoc cref="IProcessStarter.StandardError"/>
        public TextReader StandardError { get; private set; } = new StringReader(string.Empty);

        /// <inheritdoc cref="IProcessStarter.Start"/>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task<int> Start(string arguments, TimeSpan executionTimeout)
        {
            var outputTextWriter = new StringWriter(_outputBuilder);
            var errorTextWriter = new StringWriter(_errorBuilder);

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
                    ProcessExtensions.ReadAsync(
                        handler =>
                        {
                            process.OutputDataReceived += handler;
                            process.BeginOutputReadLine();
                        },
                        handler => process.OutputDataReceived -= handler,
                        outputTextWriter,
                        timeoutTokenSource.Token),
                    ProcessExtensions.ReadAsync(
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
                StandardOutput = new StringReader(_outputBuilder.ToString());
                StandardError = new StringReader(_errorBuilder.ToString());
                process.Dispose();
            }
        }

        private readonly StringBuilder _errorBuilder = new StringBuilder();

        private readonly string _executable;
        private readonly StringBuilder _outputBuilder = new StringBuilder();
    }
}
