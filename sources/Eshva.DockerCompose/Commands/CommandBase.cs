#region Usings

using System;
using System.Linq;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands
{
    /// <summary>
    /// Base class for a Docker Compose command.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files with process starter <paramref name="starter"/>.
        /// </summary>
        /// <param name="starter">
        /// Process starter that will be used to start docker-compose executable.
        /// </param>
        /// <param name="files">
        /// Project files.
        /// </param>
        protected CommandBase(IProcessStarter starter, params string[] files)
        {
            _starter = starter;
            _files = files;
        }

        /// <summary>
        /// Creates a command with specified in <paramref name="files"/> files.
        /// </summary>
        /// <param name="files">
        /// Project files.
        /// </param>
        protected CommandBase(params string[] files)
            : this(new ExecutableProcessStarter(DockerComposeExecutable), files)
        {
            _files = files;
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        public Task Execute() => Execute(_oneDayLong);

        public async Task Execute(TimeSpan executionTimeout)
        {
            var projectFileNames = _files.Aggregate(string.Empty, (result, current) => $"{result} -f \"{current}\"");
            var arguments = string.Join(" ", PrepareArguments());

            var exitCode = await _starter.Start(
                $"{projectFileNames.Trim()} {Command.Trim()} {arguments.Trim()}".Trim(),
                executionTimeout);
            if (exitCode != 0)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} executed with an error. " +
                    $"Exit code was {exitCode}.{Environment.NewLine}{Environment.NewLine}" +
                    $"Command STDOUT:{Environment.NewLine}{_starter.StandardOutput.ReadToEnd()}{Environment.NewLine}" +
                    $"Command STDERR:{Environment.NewLine}{_starter.StandardError.ReadToEnd()}{Environment.NewLine}");
            }
        }

        /// <summary>
        /// Creates a validator for the command.
        /// </summary>
        /// <returns>
        /// A FluentValidations validator.
        /// </returns>
        protected internal virtual IValidator CreateValidator() => new InlineValidator<CommandBase>();

        /// <summary>
        /// Name of the corresponding Docker Compose command.
        /// </summary>
        protected abstract string Command { get; }

        /// <summary>
        /// Prepares arguments of the command for Docker Compose command-line interface.
        /// </summary>
        /// <returns>
        /// Array of Docker Compose arguments.
        /// </returns>
        protected abstract string[] PrepareArguments();

        private const string DockerComposeExecutable = "docker-compose";
        private readonly string[] _files;
        private readonly TimeSpan _oneDayLong = TimeSpan.FromDays(1);
        private readonly IProcessStarter _starter;
    }
}
