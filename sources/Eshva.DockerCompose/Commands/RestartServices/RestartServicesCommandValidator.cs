namespace Eshva.DockerCompose.Commands.RestartServices
{
    /// <summary>
    /// Validator for the <see cref="RestartServicesCommand"/> command.
    /// </summary>
    internal sealed class RestartServicesCommandValidator : ServicesCommandValidatorBase<RestartServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal RestartServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to configure all services and specify to restart some of them. " +
            "RestartAllServices and RestartServices shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to restart all services or some of them. " +
            "Use RestartAllServices or RestartServices method.";
    }
}
