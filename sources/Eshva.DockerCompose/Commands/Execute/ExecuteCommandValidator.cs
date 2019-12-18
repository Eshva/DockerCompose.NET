#region Usings

using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Execute
{
    /// <summary>
    /// Validator for the <see cref="ExecuteCommand"/> command.
    /// </summary>
    public sealed class ExecuteCommandValidator : AbstractValidator<ExecuteCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        public ExecuteCommandValidator()
        {
            RuleFor(command => command.InService)
                .NotEmpty()
                .WithMessage("The service name must be specified. Use InService()");
            RuleFor(command => command.CommandExecutable)
                .NotEmpty()
                .WithMessage("The executing command must be specified. Use CommandWithArguments().");
        }
    }
}
