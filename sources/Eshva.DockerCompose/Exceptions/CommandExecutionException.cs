namespace Eshva.DockerCompose.Exceptions
{
    /// <summary>
    /// Represents an error occured during execution of a Docker Compose command.
    /// </summary>
    public sealed class CommandExecutionException : DockerComposeException
    {
        /// <inheritdoc cref="DockerComposeException"/>
        public CommandExecutionException(string message) : base(message)
        {
        }
    }
}
