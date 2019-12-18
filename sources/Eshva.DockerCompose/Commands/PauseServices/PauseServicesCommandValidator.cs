namespace Eshva.DockerCompose.Commands.PauseServices
{
    /// <summary>
    /// Validator for <see cref="PauseServicesCommand"/> command.
    /// </summary>
    internal sealed class PauseServicesCommandValidator : ServicesCommandValidatorBase<PauseServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal PauseServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to pause all services and specify to pause some of them. " +
            "AllServices and Services shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to pause all services or some of them. " +
            "Use AllServices or Services method.";
    }
}
