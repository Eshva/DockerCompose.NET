#region Usings

using System.Linq;
using Eshva.DockerCompose.Exceptions;

#endregion


namespace Eshva.DockerCompose.Commands
{
    /// <summary>
    /// Base class for a builder of a Docker Compose command.
    /// </summary>
    /// <typeparam name="TCommand">
    /// Docker Compose command type this builder is dedicated for.
    /// </typeparam>
    public class CommandBuilderBase<TCommand>
        where TCommand : CommandBase
    {
        /// <summary>
        /// Creates a builder for the <paramref name="command"/> command.
        /// </summary>
        /// <param name="command">
        /// Instance of a command.
        /// </param>
        protected CommandBuilderBase(TCommand command)
        {
            Command = command;
        }

        /// <summary>
        /// Builds and validates the built command.
        /// </summary>
        /// <returns>
        /// Built command ready for execution.
        /// </returns>
        /// <exception cref="CommandBuildException">
        /// During validating the built command were found some errors.
        /// </exception>
        public TCommand Build()
        {
            var validator = Command.CreateValidator();
            var errors = validator.Validate(Command).Errors.Select(error => error.ErrorMessage).ToArray();
            if (errors.Any())
            {
                throw CommandBuildException.Create(typeof(TCommand).Name, errors);
            }

            return Command;
        }

        internal TCommand Command { get; }
    }
}
