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

        /// <summary>
        /// Asynchronously executes the command with <paramref name="executionTimeout"/> timeout.
        /// </summary>
        /// <param name="executionTimeout">
        /// Execution timeout.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        /// <exception cref="CommandExecutionException">
        /// An error occured during command execution.
        /// </exception>
        public async Task Execute(TimeSpan executionTimeout)
        {
            var projectFileNames = _files.Aggregate(string.Empty, (result, current) => $"{result} -f \"{current}\"");
            var arguments = string.Join(" ", PrepareArguments());

            int exitCode;
            try
            {
                exitCode = await _starter.Start(
                    $"{projectFileNames.Trim()} {Command.Trim()} {arguments.Trim()}".Trim(),
                    executionTimeout);
            }
            catch (TimeoutException exception)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} execution exceeded timeout {executionTimeout:g}.{Environment.NewLine}" +
                    FormatOutputForException(),
                    exception);
            }
            catch (InvalidOperationException exception)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} not started.{Environment.NewLine}" +
                    FormatOutputForException(),
                    exception);
            }

            if (exitCode != 0)
            {
                throw new CommandExecutionException(
                    $"Docker Compose command {GetType().Name} executed with an error. {Environment.NewLine}" +
                    $"Exit code was {exitCode}.{Environment.NewLine}" +
                    FormatOutputForException());
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

        private string FormatOutputForException() =>
            $"{Environment.NewLine}Command STDOUT:{Environment.NewLine}{_starter.StandardOutput}{Environment.NewLine}" +
            $"Command STDERR:{Environment.NewLine}{_starter.StandardError}{Environment.NewLine}";

        private const string DockerComposeExecutable = "docker-compose";
        private readonly string[] _files;
        private readonly TimeSpan _oneDayLong = TimeSpan.FromDays(1);
        private readonly IProcessStarter _starter;
    }
}
