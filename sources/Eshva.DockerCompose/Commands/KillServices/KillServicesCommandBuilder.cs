namespace Eshva.DockerCompose.Commands.KillServices
{
    /// <summary>
    /// The builder for <see cref="KillServicesCommand"/> command.
    /// </summary>
    public sealed class KillServicesCommandBuilder : CommandBuilderBase<KillServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal KillServicesCommandBuilder(KillServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Kill all services.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public KillServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Kill only services from <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to kill.
        /// </param> 
        /// <returns>
        /// The same builder.
        /// </returns>
        public KillServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }

        /// <summary>
        /// Sends <paramref name="signal"/> to containers. Default signal is SIGKILL.
        /// </summary>
        /// <param name="signal">
        /// Signal to send to containers.
        /// </param>
        /// <remarks>
        /// See -s option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public KillServicesCommandBuilder WithSignal(string signal)
        {
            Command.WithSignal = signal;
            return this;
        }
    }
}
