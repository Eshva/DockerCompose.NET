namespace Eshva.DockerCompose.Commands.PauseServices
{
    /// <summary>
    /// The builder for <see cref="PauseServicesCommand"/> command.
    /// </summary>
    public sealed class PauseServicesCommandBuilder : CommandBuilderBase<PauseServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal PauseServicesCommandBuilder(PauseServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Pause all services.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public PauseServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Pause only from <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to pause.
        /// </param> 
        /// <returns>
        /// The same builder.
        /// </returns>
        public PauseServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }
    }
}
