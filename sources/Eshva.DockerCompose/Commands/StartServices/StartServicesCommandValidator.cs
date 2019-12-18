namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// Validator for the <see cref="StartServicesCommand"/> command.
    /// </summary>
    internal sealed class StartServicesCommandValidator : ServicesCommandValidatorBase<StartServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal StartServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to start all services and specify to start some of them. " +
            "StartAllServices and StartServices shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to start all services or some of them. " +
            "Use StartAllServices or StartServices method.";
    }
}
