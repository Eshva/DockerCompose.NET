namespace Eshva.DockerCompose.Commands.RestartServices
{
    /// <summary>
    /// The builder for <see cref="RestartServicesCommand"/> command.
    /// </summary>
    public sealed class RestartServicesCommandBuilder : CommandBuilderBase<RestartServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal RestartServicesCommandBuilder(RestartServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Restart existing containers for all services in project files.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RestartServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Restart existing containers for services specified in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to restart.
        /// </param>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RestartServicesCommandBuilder Services(params string[] services)
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
        public RestartServicesCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }
    }
}
