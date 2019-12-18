#region Usings

using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.PauseServices
{
    /// <summary>
    /// Pauses running containers of a service.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/pause/
    /// </summary>
    public sealed class PauseServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private PauseServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private PauseServicesCommand(params string[] files) : base(files)
        {
        }

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files.
        /// </summary>
        /// <param name="files">
        /// Project files.
        /// </param>
        /// <returns>
        /// Command builder.
        /// </returns>
        public static PauseServicesCommandBuilder WithFiles(params string[] files) =>
            new PauseServicesCommandBuilder(new PauseServicesCommand(files));

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files with process starter <paramref name="starter"/>.
        /// </summary>
        /// <param name="starter">
        /// Process starter that will be used to start docker-compose executable.
        /// </param>
        /// <param name="files">
        /// Project files.
        /// </param>
        /// <returns>
        /// Command builder.
        /// </returns>
        public static PauseServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new PauseServicesCommandBuilder(new PauseServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new PauseServicesCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "pause";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareOptions() => new string[0];
    }
}
