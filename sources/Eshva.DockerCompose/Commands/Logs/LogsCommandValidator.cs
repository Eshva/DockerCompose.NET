namespace Eshva.DockerCompose.Commands.Logs
{
    /// <summary>
    /// Validator for the <see cref="LogsCommand"/> command.
    /// </summary>
    internal sealed class LogsCommandValidator : ServicesCommandValidatorBase<LogsCommand>
    {
        /// <inheritdoc cref="ServicesCommandValidatorBase{TCommand}"/>
        internal LogsCommandValidator() : base(BothSpecifiedErrorMessage, NoneSpecifiedErrorMessage)
        {
        }

        private const string BothSpecifiedErrorMessage =
            "It's not allowed to configure all services and specify to start some of them. " +
            "FromAllServices and FromServices shouldn't be used together.";
        private const string NoneSpecifiedErrorMessage =
            "You should specify to take logs from all services or from some of them. " +
            "Use FromAllServices or FromServices method.";
    }
}
