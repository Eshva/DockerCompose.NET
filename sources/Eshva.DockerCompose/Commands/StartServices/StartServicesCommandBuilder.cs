namespace Eshva.DockerCompose.Commands.StartServices
{
    /// <summary>
    /// The builder for <see cref="StartServicesCommand"/> command.
    /// </summary>
    public sealed class StartServicesCommandBuilder : CommandBuilderBase<StartServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal StartServicesCommandBuilder(StartServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Start existing containers for all services in project files.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public StartServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Start existing containers for services specified in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to start.
        /// </param>
        /// <returns>
        /// The same builder.
        /// </returns>
        public StartServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }
    }
}
