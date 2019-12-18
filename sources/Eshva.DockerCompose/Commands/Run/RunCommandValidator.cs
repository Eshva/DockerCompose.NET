#region Usings

using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.Run
{
    /// <summary>
    /// Validator for the <see cref="RunCommand"/> command.
    /// </summary>
    public sealed class RunCommandValidator : AbstractValidator<RunCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        public RunCommandValidator()
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
