namespace Eshva.DockerCompose.Commands.UnpauseServices
{
    /// <summary>
    /// The builder for <see cref="UnpauseServicesCommand"/> command.
    /// </summary>
    public sealed class UnpauseServicesCommandBuilder : CommandBuilderBase<UnpauseServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal UnpauseServicesCommandBuilder(UnpauseServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Unpause all services.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UnpauseServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Unpause only from <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to unpause.
        /// </param> 
        /// <returns>
        /// The same builder.
        /// </returns>
        public UnpauseServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }
    }
}
