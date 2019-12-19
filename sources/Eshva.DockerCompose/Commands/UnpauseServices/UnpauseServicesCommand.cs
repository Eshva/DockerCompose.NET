#region Usings

using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.UnpauseServices
{
    /// <summary>
    /// Unpauses running containers of a service.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/unpause/
    /// </summary>
    public sealed class UnpauseServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private UnpauseServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private UnpauseServicesCommand(params string[] files) : base(files)
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
        public static UnpauseServicesCommandBuilder WithFiles(params string[] files) =>
            new UnpauseServicesCommandBuilder(new UnpauseServicesCommand(files));

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
        public static UnpauseServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new UnpauseServicesCommandBuilder(new UnpauseServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new UnpauseServicesCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "unpause";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareOptions() => new string[0];
    }
}
