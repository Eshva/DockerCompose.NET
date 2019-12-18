#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    /// <summary>
    /// Builds, (re)creates, starts, and attaches to containers for a service.
    /// Unless they are already running, this command also starts any linked services.
    /// More info about this command find here: https://docs.docker.com/compose/reference/up/
    /// </summary>
    /// <remarks>
    /// By default the command executed in detached mode as it's more useful for testing purposes.
    /// </remarks>
    public sealed class UpProjectCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private UpProjectCommand(IProcessStarter processStarter, params string[] files) : base(processStarter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private UpProjectCommand(params string[] files) : base(files)
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
        public static UpProjectCommandBuilder WithFiles(params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(files));

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
        public static UpProjectCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new UpProjectCommandBuilder(new UpProjectCommand(starter, files));

        internal bool Attached { get; set; } = Default.Attached;

        internal bool WithQuietPull { get; set; } = Default.WithQuietPull;

        internal bool DoNotStartLinkedServices { get; set; } = Default.DoNotStartLinkedServices;

        internal bool ForceRecreateContainers { get; set; } = Default.ForceRecreateContainers;

        internal bool RecreateDependedContainers { get; set; } = Default.RecreateDependedContainers;

        internal bool DoNotRecreateExistingContainers { get; set; } = Default.DoNotRecreateExistingContainers;

        internal bool DoNotBuildMissingImages { get; set; } = Default.DoNotBuildMissingImages;

        internal bool DoNotStartServices { get; set; } = Default.DoNotStartServices;

        internal bool ForceBuildImages { get; set; } = Default.ForceBuildImages;

        internal bool StopAllContainersIfAnyOneStopped { get; set; } = Default.StopAllContainersIfAnyOneStopped;

        internal bool RecreateAnonymousVolumes { get; set; } = Default.RecreateAnonymousVolumes;

        internal bool RemoveOrphanContainers { get; set; } = Default.RemoveOrphanContainers;

        internal int ShutdownTimeoutSeconds { get; set; } = Default.ShutdownTimeoutSeconds;

        internal string TakeExitCodeFromService { get; set; } = Default.TakeExitCodeFromService;

        internal IDictionary<string, int> Scaling { get; } = new Dictionary<string, int>();

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new UpProjectCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command => "up";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            arguments.AddConditionally(
                Attached == Default.Attached,
                "--detach");
            arguments.AddConditionally(
                WithQuietPull != Default.WithQuietPull,
                "--quiet-pull");
            arguments.AddConditionally(
                DoNotStartLinkedServices != Default.DoNotStartLinkedServices,
                "--no-deps");
            arguments.AddConditionally(
                ForceRecreateContainers != Default.ForceRecreateContainers,
                "--force-recreate");
            arguments.AddConditionally(
                RecreateDependedContainers != Default.RecreateDependedContainers,
                "--always-recreate-deps");
            arguments.AddConditionally(
                DoNotRecreateExistingContainers != Default.DoNotRecreateExistingContainers,
                "--no-recreate");
            arguments.AddConditionally(
                DoNotBuildMissingImages != Default.DoNotBuildMissingImages,
                "--no-build");
            arguments.AddConditionally(
                DoNotStartServices != Default.DoNotStartServices,
                "--no-start");
            arguments.AddConditionally(
                ForceBuildImages != Default.ForceBuildImages,
                "--build");
            arguments.AddConditionally(
                StopAllContainersIfAnyOneStopped != Default.StopAllContainersIfAnyOneStopped,
                "--abort-on-container-exit");
            arguments.AddConditionally(
                RecreateAnonymousVolumes != Default.RecreateAnonymousVolumes,
                "--renew-anon-volumes");
            arguments.AddConditionally(
                RemoveOrphanContainers != Default.RemoveOrphanContainers,
                "--remove-orphans");
            arguments.AddConditionally(
                ShutdownTimeoutSeconds != Default.ShutdownTimeoutSeconds,
                $"--timeout {ShutdownTimeoutSeconds}");
            arguments.AddConditionally(
                !string.Equals(TakeExitCodeFromService, Default.TakeExitCodeFromService, StringComparison.OrdinalIgnoreCase),
                $"--exit-code-from {TakeExitCodeFromService}");
            arguments.AddConditionally(
                Scaling.Count > 0,
                string.Join(" ", Scaling.Select(pair => $"--scale {pair.Key}={pair.Value}")));
            return arguments.ToArray();
        }

        private static class Default
        {
            public const bool Attached = false;
            public const bool WithQuietPull = false;
            public const bool DoNotStartLinkedServices = false;
            public const bool ForceRecreateContainers = false;
            public const bool DoNotRecreateExistingContainers = false;
            public const bool DoNotStartServices = false;
            public const bool DoNotBuildMissingImages = false;
            public const bool RecreateDependedContainers = false;
            public const bool ForceBuildImages = false;
            public const bool StopAllContainersIfAnyOneStopped = false;
            public const bool RecreateAnonymousVolumes = false;
            public const bool RemoveOrphanContainers = false;
            public const int ShutdownTimeoutSeconds = 10;
            public const string TakeExitCodeFromService = "";
        }
    }
}
