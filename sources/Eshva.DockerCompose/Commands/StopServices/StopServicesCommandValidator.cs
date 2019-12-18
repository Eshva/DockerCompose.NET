namespace Eshva.DockerCompose.Commands.StopServices
{
    /// <summary>
    /// Validator for the <see cref="StopServicesCommand"/> command.
    /// </summary>
    internal sealed class StopServicesCommandValidator : ServicesCommandValidatorBase<StopServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal StopServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to configure all services and specify to stop some of them. " +
            "StopAllServices and StopServices shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to stop all services or some of them. " +
            "Use StopAllServices or StopServices method.";
    }
}
