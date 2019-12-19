namespace Eshva.DockerCompose.Commands.KillServices
{
    /// <summary>
    /// Validator for <see cref="KillServicesCommand"/> command.
    /// </summary>
    internal sealed class KillServicesCommandValidator : ServicesCommandValidatorBase<KillServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal KillServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to kill all services and specify to kill some of them. " +
            "AllServices and Services shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to kill all services or some of them. " +
            "Use AllServices or Services method.";
    }
}
