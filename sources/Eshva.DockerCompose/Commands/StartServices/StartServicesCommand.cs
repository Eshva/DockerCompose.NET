#region Usings

using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// Starts existing containers for services.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/start/
    /// </summary>
    public sealed class StartServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private StartServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private StartServicesCommand(params string[] files) : base(files)
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
        public static StartServicesCommandBuilder WithFiles(params string[] files) =>
            new StartServicesCommandBuilder(new StartServicesCommand(files));

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
        public static StartServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new StartServicesCommandBuilder(new StartServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new StartServicesCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "start";

        /// <inheritdoc cref="ServicesCommandBase.PrepareOptions"/>
        protected override string[] PrepareOptions()
        {
            return new string[0];
        }
    }
}
