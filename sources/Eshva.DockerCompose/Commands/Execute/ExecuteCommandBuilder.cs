namespace Eshva.DockerCompose.Commands.Execute
{
    /// <summary>
    /// The builder for <see cref="ExecuteCommand"/> command.
    /// </summary>
    public sealed class ExecuteCommandBuilder : CommandBuilderBase<ExecuteCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal ExecuteCommandBuilder(ExecuteCommand command) : base(command)
        {
        }

        /// <summary>
        /// Execute command within container of <paramref name="service"/>.
        /// </summary>
        /// <param name="service">
        /// Ths service name.
        /// </param>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder InService(string service)
        {
            Command.InService = service;
            return this;
        }

        /// <summary>
        /// The <paramref name="command"/> to execute with its <paramref name="arguments"/>.
        /// </summary>
        /// <param name="command">
        /// The command to execute.
        /// </param>
        /// <param name="arguments">
        /// Command arguments.
        /// </param>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder CommandWithArguments(string command, params string[] arguments)
        {
            Command.CommandExecutable = command;
            Command.CommandArguments.AddRange(arguments);
            return this;
        }

        /// <summary>
        /// Give extended privileges to the process.
        /// </summary>
        /// <remarks>
        /// See --privileged option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder WithExtendedPrivileges()
        {
            Command.WithExtendedPrivileges = true;
            return this;
        }

        /// <summary>
        /// Execute command in the detached mode.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder Detached()
        {
            Command.Detached = true;
            return this;
        }

        /// <summary>
        /// Run the command as this <paramref name="user"/>.
        /// </summary>
        /// <param name="user">
        /// The user name.
        /// </param>
        /// <remarks>
        /// See --user option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder AsUser(string user)
        {
            Command.AsUser = user;
            return this;
        }

        /// <summary>
        /// Disable pseudo-tty allocation. By default 'docker-compose exec' allocates a TTY.
        /// </summary>
        /// <remarks>
        /// See -T option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder WithoutTty()
        {
            Command.WithoutTty = true;
            return this;
        }

        /// <summary>
        /// Index of the container if there are multiple instances of a service. Default is 1.
        /// </summary>
        /// <param name="containerIndex">
        /// Container index.
        /// </param>
        /// <remarks>
        /// See --index option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder InServiceContainer(int containerIndex)
        {
            Command.InServiceContainer = containerIndex;
            return this;
        }

        /// <summary>
        /// Set environment variable. Can be used multiple times.
        /// </summary>
        /// <param name="name">
        /// Variable name.
        /// </param>
        /// <param name="value">
        /// Variable value.
        /// </param>
        /// <remarks>
        /// See --env option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder WithEnvironmentVariable(string name, string value)
        {
            Command.EnvironmentVariables.Add(name, value);
            return this;
        }

        /// <summary>
        /// <paramref name="path"/> to working directory for this command.
        /// </summary>
        /// <param name="path">
        /// The path to working directory.
        /// </param>
        /// <remarks>
        /// See --workdir option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public ExecuteCommandBuilder WithinWorkingDirectory(string path)
        {
            Command.WithinWorkingDirectory = path;
            return this;
        }
    }
}
