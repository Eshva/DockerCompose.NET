#region Usings

using FluentValidation;

#endregion


namespace Eshva.DockerCompose.Commands.UpProject
{
    /// <summary>
    /// Validator for <see cref="UpProjectCommand"/> command.
    /// </summary>
    internal sealed class UpProjectCommandValidator : AbstractValidator<UpProjectCommand>
    {
        /// <inheritdoc cref="AbstractValidator{T}"/>
        internal UpProjectCommandValidator()
        {
            RuleFor(command => command)
                .Must(command => !(command.StopAllContainersIfAnyOneStopped && !command.Attached))
                .WithMessage("UP command can't stop all containers if any of them stopped and the command executed in detached mode.");
            RuleFor(command => command)
                .Must(command => !(command.DoNotRecreateExistingContainers && command.RecreateDependedContainers))
                .WithMessage("Options of UP command DoNotRecreateExistingContainers and RecreateDependedContainers are not compatible.");
            RuleFor(command => command)
                .Must(command => !(command.DoNotRecreateExistingContainers && command.ForceRecreateContainers))
                .WithMessage("Options of UP command DoNotRecreateExistingContainers and ForceRecreateContainers are not compatible.");
            RuleFor(command => command)
                .Must(command => !(command.DoNotRecreateExistingContainers && command.RecreateAnonymousVolumes))
                .WithMessage("Options of UP command DoNotRecreateExistingContainers and RecreateAnonymousVolumes are not compatible.");
            RuleFor(command => command)
                .Must(command => !(command.DoNotBuildMissingImages && command.ForceBuildImages))
                .WithMessage("Options of UP command DoNotBuildMissingImages and ForceBuildImages are not compatible.");
        }
    }
}
