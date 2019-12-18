#region Usings

using System;
using System.Diagnostics;
using System.IO;
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
        public Task<int> Start(string arguments, TimeSpan executionTimeout)
        {
            var processStartInfo = new ProcessStartInfo(_executable, arguments)
                                   {
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true,
                                       CreateNoWindow = false,
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
                var wasExitedNormally = process.WaitForExit((int)executionTimeout.TotalMilliseconds);
                if (!wasExitedNormally)
                {
                    throw new TimeoutException();
                }

                return Task.FromResult(process.ExitCode);
            }
            finally
            {
                StandardOutput = process.StandardOutput;
                StandardError = process.StandardError;
                process.Dispose();
            }
        }

        private readonly string _executable;
    }
}
