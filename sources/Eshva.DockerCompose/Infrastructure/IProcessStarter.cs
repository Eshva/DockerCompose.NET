#region Usings

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Infrastructure
{
    /// <summary>
    /// The contract for process starter.
    /// </summary>
    public interface IProcessStarter
    {
        /// <summary>
        /// Gets standard output stream of the started process.
        /// </summary>
        StringBuilder StandardOutput { get; }

        /// <summary>
        /// Gets standard error stream  of the started process.
        /// </summary>
        StringBuilder StandardError { get; }

        /// <summary>
        /// Starts the process chronologically with <paramref name="arguments"/>.
        /// It will be terminated if <paramref name="executionTimeout"/> will be overrun.
        /// </summary>
        /// <param name="arguments">
        /// Command-line arguments of the process to start.
        /// </param>
        /// <param name="executionTimeout">
        /// Timeout for the process execution.
        /// </param>
        /// <returns>
        /// Exit code of the process.
        /// </returns>
        /// <exception cref="ProcessStartException">
        /// An error occured during starting the executable.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Process not started.
        /// </exception>
        Task<int> Start(string arguments, TimeSpan executionTimeout);
    }
}
