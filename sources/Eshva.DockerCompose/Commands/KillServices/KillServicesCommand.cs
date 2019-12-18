#region Usings

using System;
using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.KillServices
{
    /// <summary>
    /// Forces running containers to stop by sending a SIGKILL signal. Optionally the signal can be passed.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/kill/
    /// </summary>
    public sealed class KillServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private KillServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private KillServicesCommand(params string[] files) : base(files)
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
        public static KillServicesCommandBuilder WithFiles(params string[] files) =>
            new KillServicesCommandBuilder(new KillServicesCommand(files));

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
        public static KillServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new KillServicesCommandBuilder(new KillServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new KillServicesCommandValidator();

        internal string WithSignal { get; set; } = Default.WithSignal;

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "kill";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareOptions()
        {
            var options = new List<string>();
            options.AddConditionally(
                !WithSignal.Equals(Default.WithSignal, StringComparison.OrdinalIgnoreCase),
                $"-s {WithSignal}");
            return options.ToArray();
        }

        private static class Default
        {
            public const string WithSignal = "SIGKILL";
        }
    }
}
