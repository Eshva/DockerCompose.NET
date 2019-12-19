#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.StopServices
{
    /// <summary>
    /// Stops running containers without removing them. They can be started again with <see cref="StartServicesCommand"/>.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/stop/
    /// </summary>
    public sealed class StopServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private StopServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private StopServicesCommand(params string[] files) : base(files)
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
        public static StopServicesCommandBuilder WithFiles(params string[] files) =>
            new StopServicesCommandBuilder(new StopServicesCommand(files));

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files with process <paramref name="starter"/>.
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
        public static StopServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new StopServicesCommandBuilder(new StopServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new StopServicesCommandValidator();

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "stop";

        /// <inheritdoc cref="ServicesCommandBase.PrepareOptions"/>
        protected override string[] PrepareOptions()
        {
            var options = new List<string>();
            options.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            return options.ToArray();
        }

        private static class Default
        {
            public const int ShutdownTimeoutSeconds = 10;
        }
    }
}
