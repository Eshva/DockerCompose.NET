#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.RestartServices
{
    /// <summary>
    /// Restarts all stopped and running services.
    /// </summary>
    /// <remarks>
    /// <para>If you make changes to your docker-compose.yml configuration these
    /// changes are not reflected after running this command.</para>
    /// <para>For example, changes to environment variables(which are added after
    /// a container is built, but before the container’s command is executed) are not
    /// updated after restarting.</para>
    /// <para>If you are looking to configure a service’s restart policy, please
    /// refer to restart in Compose file v3 and restart in Compose v2.Note that
    /// if you are deploying a stack in swarm mode, you should use restart_policy,
    /// instead.</para>
    /// More info about this command find is here: https://docs.docker.com/compose/reference/restart/
    /// </remarks>
    public sealed class RestartServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private RestartServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private RestartServicesCommand(params string[] files) : base(files)
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
        public static RestartServicesCommandBuilder WithFiles(params string[] files) =>
            new RestartServicesCommandBuilder(new RestartServicesCommand(files));

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
        public static RestartServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new RestartServicesCommandBuilder(new RestartServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new RestartServicesCommandValidator();

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "restart";

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
