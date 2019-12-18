#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Logs
{
    /// <summary>
    /// Takes log output from services in project.
    /// More info about this command find is here: https://docs.docker.com/compose/reference/logs/
    /// </summary>
    public sealed class LogsCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        private LogsCommand(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        private LogsCommand(params string[] files) : base(files)
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
        public static LogsCommandBuilder WithFiles(params string[] files) =>
            new LogsCommandBuilder(new LogsCommand(files));

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
        public static LogsCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
            new LogsCommandBuilder(new LogsCommand(starter, files));

        internal bool WithTimestamps { get; set; } = Default.WithTimestamps;

        internal int TakeNumberOfLines { get; set; } = Default.TakeNumberOfLines;

        /// <inheritdoc cref="CommandBase.CreateValidator"/>
        protected internal override IValidator CreateValidator() => new LogsCommandValidator();

        /// <inheritdoc cref="CommandBase.Command"/>
        protected override string Command { get; } = "logs";

        /// <inheritdoc cref="ServicesCommandBase.PrepareOptions"/>
        protected override string[] PrepareOptions()
        {
            var options = new List<string>();
            options.AddConditionally(WithTimestamps, "--timestamps");
            options.AddConditionally(TakeNumberOfLines > 0, $"--tail {TakeNumberOfLines}");
            return options.ToArray();
        }

        private static class Default
        {
            public const bool WithTimestamps = false;
            public const int TakeNumberOfLines = 0;
        }
    }
}
