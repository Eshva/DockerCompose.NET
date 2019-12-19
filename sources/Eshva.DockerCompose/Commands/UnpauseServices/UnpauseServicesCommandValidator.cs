namespace Eshva.DockerCompose.Commands.UnpauseServices
{
    /// <summary>
    /// Validator for <see cref="UnpauseServicesCommand"/> command.
    /// </summary>
    internal sealed class UnpauseServicesCommandValidator : ServicesCommandValidatorBase<UnpauseServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal UnpauseServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to unpause all services and specify to unpause some of them. " +
            "AllServices and Services shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to unpause all services or some of them. " +
            "Use AllServices or Services method.";
    }
}
