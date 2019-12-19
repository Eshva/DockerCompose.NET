#region Usings

using System.Linq;
using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands
{
    /// <summary>
    /// Base class for validators of commands with list of services.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The command type.
    /// </typeparam>
    public abstract class ServicesCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
        where TCommand : ServicesCommandBase
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        protected ServicesCommandValidatorBase(string bothSpecifiedErrorMessage, string noneSpecifiedErrorMessage)
        {
            RuleFor(command => command)
                .Must(command => !(command.DoForAllServices && command.Services.Any()))
                .WithMessage(bothSpecifiedErrorMessage);
            RuleFor(command => command)
                .Must(command => command.DoForAllServices || command.Services.Any())
                .WithMessage(noneSpecifiedErrorMessage);
        }
    }
}
