#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    /// <summary>
    /// Represents an error occured during execution of a process.
    /// </summary>
    public sealed class ProcessStartException : DockerComposeException
    {
        /// <inheritdoc cref="DockerComposeException"/>
        public ProcessStartException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
