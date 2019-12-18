namespace Eshva.DockerCompose.Commands.Logs
{
    /// <summary>
    /// The builder for <see cref="LogsCommand"/> command.
    /// </summary>
    public sealed class LogsCommandBuilder : CommandBuilderBase<LogsCommand>

    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal LogsCommandBuilder(LogsCommand command) : base(command)
        {
        }

        /// <summary>
        /// Take logs from all services.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public LogsCommandBuilder FromAllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Take logs only from <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services from which take logs from. 
        /// </param> 
        /// <returns>
        /// The same builder.
        /// </returns>
        public LogsCommandBuilder FromServices(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }

        /// <summary>
        /// Log lines should contain timestamps.
        /// </summary>
        /// <remarks>
        /// See --timestamps option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public LogsCommandBuilder WithTimestamps()
        {
            Command.WithTimestamps = true;
            return this;
        }

        /// <summary>
        /// Take only <paramref name="numberOfLines"/> tail lines for each container.
        /// </summary>
        /// <param name="numberOfLines">
        /// Number of tail lines to take.
        /// </param>
        /// <remarks>
        /// See --tail option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public LogsCommandBuilder TakeNumberOfLines(int numberOfLines)
        {
            Command.TakeNumberOfLines = numberOfLines;
            return this;
        }
    }
}
