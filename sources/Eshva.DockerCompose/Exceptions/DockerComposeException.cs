#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    /// <summary>
    /// The base class for all exceptions of this library.
    /// </summary>
    public abstract class DockerComposeException : Exception
    {
        /// <inheritdoc cref="Exception"/>
        protected DockerComposeException(string message) : base(message)
        {
        }

        /// <inheritdoc cref="Exception"/>
        protected DockerComposeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
