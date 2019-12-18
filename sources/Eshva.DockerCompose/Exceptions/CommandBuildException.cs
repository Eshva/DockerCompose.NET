#region Usings

using System;

#endregion


namespace Eshva.DockerCompose.Exceptions
{
    /// <summary>
    /// Some errors occurred during building of a Docker Compose command.
    /// </summary>
    public sealed class CommandBuildException : DockerComposeException
    {
        /// <summary>
        /// Creates an exception with <paramref name="message"/> and list of <paramref name="errors"/>.
        /// </summary>
        /// <param name="message">
        /// Message about exception.
        /// </param>
        /// <param name="errors">
        /// Array of errors occurred during building.
        /// </param>
        private CommandBuildException(string message, string[] errors) : base(message)
        {
            Errors = errors;
        }

        /// <summary>
        /// Array of errors occurred during building.
        /// </summary>
        public string[] Errors { get; }

        /// <summary>
        /// Creates an exception with a list of <paramref name="errors"/>.
        /// </summary>
        /// <param name="command">
        /// Docker Compose command.
        /// </param>
        /// <param name="errors">
        /// Array of errors occurred during building.
        /// </param>
        public static CommandBuildException Create(string command, string[] errors)
        {
            var message = $"During building {command} following errors occured:{Environment.NewLine}" +
                          $"{string.Join(Environment.NewLine, errors)}";
            return new CommandBuildException(message, errors);
        }
    }
}
