namespace Eshva.DockerCompose.Commands.DownProject
{
    /// <summary>
    /// The builder for <see cref="DownProjectCommand"/> command.
    /// </summary>
    public sealed class DownProjectCommandBuilder : CommandBuilderBase<DownProjectCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal DownProjectCommandBuilder(DownProjectCommand command) : base(command)
        {
        }

        /// <summary>
        /// Remove all images used by any service.
        /// </summary>
        /// <remarks>
        /// See --rmi all option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public DownProjectCommandBuilder RemoveAllImages()
        {
            Command.RemoveImages = DownProjectCommand.ImageRemovingType.All;
            return this;
        }

        /// <summary>
        /// Remove only images that don't have a custom tag set by the 'image' field.
        /// </summary>
        /// <remarks>
        /// See --rmi local option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public DownProjectCommandBuilder RemoveLocalImages()
        {
            Command.RemoveImages = DownProjectCommand.ImageRemovingType.Local;
            return this;
        }

        /// <summary>
        /// Remove named volumes declared in the 'volumes' section of the Compose file and anonymous volumes attached to containers.
        /// </summary>
        /// <remarks>
        /// See --volumes option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public DownProjectCommandBuilder RemoveVolumes()
        {
            Command.RemoveVolumes = true;
            return this;
        }

        /// <summary>
        /// Remove containers for services not defined in the Compose file.
        /// </summary>
        /// <remarks>
        /// See --remove-orphans option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public DownProjectCommandBuilder RemoveOrphanContainers()
        {
            Command.RemoveOrphanContainers = true;
            return this;
        }

        /// <summary>
        /// Specify a shutdown timeout in seconds. The default is 10 seconds.
        /// </summary>
        /// <param name="shutdownTimeoutSeconds">
        /// Timeout in seconds.
        /// </param>
        /// <remarks>
        /// See --timeout option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public DownProjectCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }
    }
}
