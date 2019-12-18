#region Usings

using System.Collections.Generic;
using System.Linq;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.BuildServices
{
    /// <summary>
    /// Builds containers for services.
    /// More info about this command find here: https://docs.docker.com/compose/reference/build/
    /// </summary> 
    public sealed class BuildServicesCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private BuildServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private BuildServicesCommand(params string[] files) : base(files)
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
        public static BuildServicesCommandBuilder WithFiles(params string[] files) =>
            new BuildServicesCommandBuilder(new BuildServicesCommand(files));

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
        public static BuildServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new BuildServicesCommandBuilder(new BuildServicesCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new BuildServicesCommandValidator();

        internal bool CompressBuildContext { get; set; } = Default.CompressBuildContext;

        internal bool RemoveIntermediateContainers { get; set; } = Default.RemoveIntermediateContainers;

        internal bool DoNotUseCache { get; set; } = Default.DoNotUseCache;

        internal bool AlwaysPullImages { get; set; } = Default.AlwaysPullImages;

        internal bool InParallel { get; set; } = Default.InParallel;

        internal int WithLimitedMemory { get; set; } = Default.WithLimitedMemory;

        internal Dictionary<string, string> BuildTimeVariables { get; } = new Dictionary<string, string>();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "build";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareOptions()
        {
            var options = new List<string>();
            options.AddConditionally(
                CompressBuildContext != Default.CompressBuildContext,
                "--compress");
            options.AddConditionally(
                RemoveIntermediateContainers != Default.RemoveIntermediateContainers,
                "--force-rm");
            options.AddConditionally(
                DoNotUseCache != Default.DoNotUseCache,
                "--no-cache");
            options.AddConditionally(
                AlwaysPullImages != Default.AlwaysPullImages,
                "--pull");
            options.AddConditionally(
                InParallel != Default.InParallel,
                "--parallel");
            options.AddConditionally(
                WithLimitedMemory > Default.WithLimitedMemory,
                $"--memory {WithLimitedMemory}");
            options.AddConditionally(
                BuildTimeVariables.Count > 0,
                string.Join(" ", BuildTimeVariables.Select(pair => $"--build-arg {pair.Key}=\"{pair.Value}\"")));
            return options.ToArray();
        }

        private static class Default
        {
            public const bool CompressBuildContext = false;
            public const bool RemoveIntermediateContainers = false;
            public const bool DoNotUseCache = false;
            public const bool AlwaysPullImages = false;
            public const bool InParallel = false;
            public const int WithLimitedMemory = 0;
        }
    }
}
