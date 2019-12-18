namespace Eshva.DockerCompose.Commands.BuildServices
{
    /// <summary>
    /// Validator for <see cref="BuildServicesCommand"/> command.
    /// </summary>
    internal sealed class BuildServicesCommandValidator : ServicesCommandValidatorBase<BuildServicesCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal BuildServicesCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to build all services and specify to build some of them. " +
            "AllServices and Services shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to build all services or some of them. " +
            "Use AllServices or Services method.";
    }
}
