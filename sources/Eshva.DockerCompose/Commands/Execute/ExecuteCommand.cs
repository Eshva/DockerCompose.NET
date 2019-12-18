#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Execute
{
    /// <summary>
    /// Execute arbitrary command in a service container.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/exec/
    /// </summary>
    /// <remarks>
    /// By default the command executed in detached mode as it's more useful for testing purposes.
    /// </remarks>
    public sealed class ExecuteCommand : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private ExecuteCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private ExecuteCommand(params string[] files) : base(files)
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
        public static ExecuteCommandBuilder WithFiles(params string[] files) =>
            new ExecuteCommandBuilder(new ExecuteCommand(files));

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
        public static ExecuteCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new ExecuteCommandBuilder(new ExecuteCommand(starter, files));

        internal string InService { get; set; } = Default.InService;

        internal string CommandExecutable { get; set; } = Default.CommandExecutable;

        internal List<string> CommandArguments { get; } = new List<string>();

        internal bool Detached { get; set; } = Default.Detached;

        internal bool WithExtendedPrivileges { get; set; } = Default.WithExtendedPrivileges;

        internal string AsUser { get; set; } = Default.AsUser;

        internal bool WithoutTty { get; set; } = Default.WithoutTty;

        internal int InServiceContainer { get; set; } = Default.InServiceContainer;

        internal Dictionary<string, string> EnvironmentVariables { get; } = new Dictionary<string, string>();

        internal string WithinWorkingDirectory { get; set; } = Default.WithinWorkingDirectory;

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new ExecuteCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command => "exec";

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected override string[] PrepareArguments()
        {
            var arguments = new List<string>();
            AddOptions(arguments);
            arguments.AddConditionally(
                !InService.Equals(Default.InService, StringComparison.OrdinalIgnoreCase),
                InService);
            AddCommandAndArguments(arguments);
            return arguments.ToArray();
        }

        private void AddOptions(IList<string> arguments)
        {
            arguments.AddConditionally(
                Detached != Default.Detached,
                "--detach");
            arguments.AddConditionally(
                WithExtendedPrivileges != Default.WithExtendedPrivileges,
                "--privileged");
            arguments.AddConditionally(
                !AsUser.Equals(Default.AsUser, StringComparison.OrdinalIgnoreCase),
                $"--user {AsUser}");
            arguments.AddConditionally(
                WithoutTty != Default.WithoutTty,
                "-T");
            arguments.AddConditionally(
                InServiceContainer > Default.InServiceContainer,
                $"--index {InServiceContainer}");
            arguments.AddConditionally(
                !WithinWorkingDirectory.Equals(Default.WithinWorkingDirectory),
                $"--workdir \"{WithinWorkingDirectory}\"");
            arguments.AddConditionally(
                EnvironmentVariables.Any(),
                $"{string.Join(" ", EnvironmentVariables.Select(pair => $"--env {pair.Key}=\"{pair.Value}\""))}");
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
            public const bool Detached = false;
            public const bool WithExtendedPrivileges = false;
            public const string AsUser = "";
            public const bool WithoutTty = false;
            public const int InServiceContainer = 1;
            public const string WithinWorkingDirectory = "";
        }
    }
}
