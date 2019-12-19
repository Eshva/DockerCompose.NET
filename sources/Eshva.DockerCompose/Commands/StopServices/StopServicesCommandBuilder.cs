namespace Eshva.DockerCompose.Commands.StopServices
{
    /// <summary>
    /// The builder for <see cref="StopServicesCommand"/> command.
    /// </summary>
    public sealed class StopServicesCommandBuilder : CommandBuilderBase<StopServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal StopServicesCommandBuilder(StopServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Stop existing containers for all services in project files.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public StopServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Stop existing containers for services specified in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to stop.
        /// </param>
        /// <returns>
        /// The same builder.
        /// </returns>
        public StopServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }

        /// <summary>
        /// Specify a shutdown timeout in seconds. The default is 10 seconds.
        /// </summary>
        /// <param name="shutdownTimeoutSeconds">
        /// Timeout in seconds.
        /// </param>
        /// <remarks>
        /// See --timeout option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public StopServicesCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }
    }
}
