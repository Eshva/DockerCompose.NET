#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands.DownProject
{
    /// <summary>
    /// Stops containers and removes containers, networks, volumes, and images created by <see cref="UpProjectCommand"/>.
    /// By default, the only things removed are:
    ///   * Containers for services defined in the Compose file.
    ///   * Networks defined in the networks section of the Compose file.
    ///   * The default network, if one is used.
    /// Networks and volumes defined as external are never removed.
    /// More info about this command find here: https://docs.docker.com/compose/reference/down/
    /// </summary>
    public sealed class DownProjectCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private DownProjectCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private DownProjectCommand(params string[] files) : base(files)
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
        public static DownProjectCommandBuilder WithFiles(params string[] files) =>
            new DownProjectCommandBuilder(new DownProjectCommand(files));

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
        public static DownProjectCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new DownProjectCommandBuilder(new DownProjectCommand(starter, files));

        internal ImageRemovingType RemoveImages { get; set; } = Default.RemoveImages;

        internal bool RemoveVolumes { get; set; } = Default.RemoveVolumes;

        internal bool RemoveOrphanContainers { get; set; } = Default.RemoveOrphanContainers;

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command => "down";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(
                RemoveImages == ImageRemovingType.All,
                "--rmi all");
            arguments.AddConditionally(
                RemoveImages == ImageRemovingType.Local,
                "--rmi local");
            arguments.AddConditionally(
                RemoveVolumes != Default.RemoveVolumes,
                "--volumes");
            arguments.AddConditionally(
                RemoveOrphanContainers != Default.RemoveOrphanContainers,
                "--remove-orphans");
            arguments.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            return arguments.ToArray();
        }

        private static class Default
        {
            public const ImageRemovingType RemoveImages = ImageRemovingType.None;
            public const bool RemoveVolumes = false;
            public const bool RemoveOrphanContainers = false;
            public const int ShutdownTimeoutSeconds = 10;
        }

        internal enum ImageRemovingType
        {
            None,
            All,
            Local
        }
    }
}
