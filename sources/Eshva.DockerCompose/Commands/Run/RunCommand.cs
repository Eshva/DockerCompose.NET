#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Run
{
    /// <summary>
    /// Runs a one-time command against a service.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/run/
    /// </summary>
    public sealed class RunCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private RunCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private RunCommand(params string[] files) : base(files)
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
        public static RunCommandBuilder WithFiles(params string[] files) =>
            new RunCommandBuilder(new RunCommand(files));

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
        public static RunCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new RunCommandBuilder(new RunCommand(starter, files));

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new RunCommandValidator();

        internal string InService { get; set; } = Default.InService;

        internal string CommandExecutable { get; set; } = Default.CommandExecutable;

        internal List<string> CommandArguments { get; } = new List<string>();

        internal bool Detached { get; set; } = Default.Detached;

        internal bool RemoveContainerAfterRun { get; set; } = Default.RemoveContainerAfterRun;

        internal string AsUser { get; set; } = Default.AsUser;

        internal bool WithoutTty { get; set; } = Default.WithoutTty;

        internal Dictionary<string, string> EnvironmentVariables { get; } = new Dictionary<string, string>();

        internal string WithinWorkingDirectory { get; set; } = Default.WithinWorkingDirectory;

        internal string NameContainerAs { get; set; } = Default.NameContainerAs;

        internal string OverrideEntryPointWith { get; set; } = Default.OverrideEntryPointWith;

        internal bool DoNotStartLinkedServices { get; set; } = Default.DoNotStartLinkedServices;

        internal bool MapServicePortsOnHost { get; set; } = Default.MapServicePortsOnHost;

        internal bool UseServiceNetworkAliases { get; set; } = Default.UseServiceNetworkAliases;

        internal List<string> Volumes { get; } = new List<string>();

        internal Dictionary<string, string> PortMappings { get; } = new Dictionary<string, string>();

        internal Dictionary<string, string> Labels { get; } = new Dictionary<string, string>();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "run";

        /// <inheritdoc cref="ServicesCommandBase.PrepareOptions"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            AddOptions(arguments);
            AddVolumes(arguments);
            AddPortMappings(arguments);
            AddEnvironmentVariables(arguments);
            AddContainerLabels(arguments);
            AddService(arguments);
            AddCommandAndArguments(arguments);
            return arguments.ToArray();
        }

        private void AddOptions(IList<string> arguments)
        {
            arguments.AddConditionally(
                Detached != Default.Detached,
                "--detach");
            arguments.AddConditionally(
                RemoveContainerAfterRun != Default.RemoveContainerAfterRun,
                "--rm");
            arguments.AddConditionally(
                !string.Equals(AsUser, Default.AsUser, StringComparison.OrdinalIgnoreCase),
                $"--user {AsUser}");
            arguments.AddConditionally(
                WithoutTty != Default.WithoutTty,
                "-T");
            arguments.AddConditionally(
                !WithinWorkingDirectory.Equals(Default.WithinWorkingDirectory),
                $"--workdir \"{WithinWorkingDirectory}\"");
            arguments.AddConditionally(
                !NameContainerAs.Equals(Default.NameContainerAs, StringComparison.OrdinalIgnoreCase),
                $"--name {NameContainerAs}");
            arguments.AddConditionally(
                !OverrideEntryPointWith.Equals(Default.OverrideEntryPointWith, StringComparison.OrdinalIgnoreCase),
                $"--entrypoint {OverrideEntryPointWith}");
            arguments.AddConditionally(
                DoNotStartLinkedServices != Default.DoNotStartLinkedServices,
                "--no-deps");
            arguments.AddConditionally(
                MapServicePortsOnHost != Default.MapServicePortsOnHost,
                "--service-ports");
            arguments.AddConditionally(
                UseServiceNetworkAliases != Default.UseServiceNetworkAliases,
                "--use-aliases");
        }

        private void AddVolumes(IList<string> arguments)
        {
            arguments.AddConditionally(
                Volumes.Any(),
                string.Join(" ", Volumes.Select(volume => $"--volume {volume}")));
        }

        private void AddPortMappings(IList<string> arguments)
        {
            arguments.AddConditionally(
                PortMappings.Any(),
                $"{string.Join(" ", PortMappings.Select(pair => $"--publish {pair.Key}:{pair.Value}"))}");
        }

        private void AddEnvironmentVariables(IList<string> arguments)
        {
            arguments.AddConditionally(
                EnvironmentVariables.Any(),
                $"{string.Join(" ", EnvironmentVariables.Select(pair => $"-e {pair.Key}=\"{pair.Value}\""))}");
        }

        private void AddContainerLabels(IList<string> arguments)
        {
            arguments.AddConditionally(
                Labels.Any(),
                $"{string.Join(" ", Labels.Select(pair => $"--label {pair.Key}=\"{pair.Value}\""))}");
        }

        private void AddService(IList<string> arguments)
        {
            arguments.AddConditionally(!InService.Equals(Default.InService, StringComparison.OrdinalIgnoreCase), InService);
        }

        private void AddCommandAndArguments(IList<string> arguments)
        {
            arguments.AddConditionally(
                !CommandExecutable.Equals(Default.CommandExecutable, StringComparison.OrdinalIgnoreCase),
                CommandExecutable);
            arguments.AddConditionally(
                CommandArguments.Any(),
                string.Join(" ", CommandArguments));
        }

        private static class Default
        {
            public const string InService = "";
            public const string CommandExecutable = "";
            public const string AsUser = "";
            public const string WithinWorkingDirectory = "";
            public const string NameContainerAs = "";
            public const string OverrideEntryPointWith = "";
            public const bool Detached = false;
            public const bool RemoveContainerAfterRun = false;
            public const bool WithoutTty = false;
            public const bool DoNotStartLinkedServices = false;
            public const bool MapServicePortsOnHost = false;
            public const bool UseServiceNetworkAliases = false;
        }
    }
}
