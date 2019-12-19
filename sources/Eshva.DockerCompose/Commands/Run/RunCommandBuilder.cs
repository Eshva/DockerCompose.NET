namespace Eshva.DockerCompose.Commands.Run
{
    public sealed class RunCommandBuilder : CommandBuilderBase<RunCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal RunCommandBuilder(RunCommand command) : base(command)
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
        public RunCommandBuilder InService(string service)
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
        public RunCommandBuilder CommandWithArguments(string command, params string[] arguments)
        {
            Command.CommandExecutable = command;
            Command.CommandArguments.AddRange(arguments);
            return this;
        }

        /// <summary>
        /// Execute command in the detached mode.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder Detached()
        {
            Command.Detached = true;
            return this;
        }

        /// <summary>
        /// Assign a name to the container.
        /// </summary>
        /// <param name="name">
        /// Container name.
        /// </param>
        /// <remarks>
        /// See --name option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder NameContainerAs(string name)
        {
            Command.NameContainerAs = name;
            return this;
        }

        /// <summary>
        /// Override the entry point of the image.
        /// </summary>
        /// <param name="command">
        /// Entry point command.
        /// </param>
        /// <remarks>
        /// See --entrypoint option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder OverrideEntryPointWith(string command)
        {
            Command.OverrideEntryPointWith = command;
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
        /// See -e option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder WithEnvironmentVariable(string name, string value)
        {
            Command.EnvironmentVariables.Add(name, value);
            return this;
        }

        /// <summary>
        /// Add or override a label. Can be used multiple times.
        /// </summary>
        /// <param name="name">
        /// Label name.
        /// </param>
        /// <param name="value">
        /// Label value.
        /// </param>
        /// <remarks>
        /// See --label option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder AddOrOverrideContainerLabel(string name, string value)
        {
            Command.Labels.Add(name, value);
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
        public RunCommandBuilder AsUser(string user)
        {
            Command.AsUser = user;
            return this;
        }

        /// <summary>
        /// Don't start linked services.
        /// </summary>
        /// <remarks>
        /// See --no-deps option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder DoNotStartLinkedServices()
        {
            Command.DoNotStartLinkedServices = true;
            return this;
        }

        /// <summary>
        /// Remove container after run. Ignored in detached mode.
        /// </summary>
        /// <remarks>
        /// See --rm option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder RemoveContainerAfterRun()
        {
            Command.RemoveContainerAfterRun = true;
            return this;
        }

        /// <summary>
        /// Publish a container's port to the host. Can be used multiple times.
        /// </summary>
        /// <param name="host">
        /// Host port mapping part.
        /// </param>
        /// <param name="container">
        /// Container port mapping part.
        /// </param>
        /// <remarks>
        /// See --publish option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder MapPorts(string host, string container)
        {
            Command.PortMappings.Add(host, container);
            return this;
        }

        /// <summary>
        /// Run command with the service's ports enabled and mapped to the host.
        /// </summary>
        /// <remarks>
        /// See --service-ports option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder MapServicePortsOnHost()
        {
            Command.MapServicePortsOnHost = true;
            return this;
        }

        /// <summary>
        /// Use the service's network aliases in the network(s) the container connects to.
        /// </summary>
        /// <remarks>
        /// See --use-aliases option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder UseServiceNetworkAliases()
        {
            Command.UseServiceNetworkAliases = true;
            return this;
        }

        /// <summary>
        /// Bind mount <paramref name="volumes"/>.
        /// </summary>
        /// <param name="volumes">
        /// Volumes to bind.
        /// </param>
        /// <remarks>
        /// See --volume option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public RunCommandBuilder BindVolumes(params string[] volumes)
        {
            Command.Volumes.AddRange(volumes);
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
        public RunCommandBuilder WithoutTty()
        {
            Command.WithoutTty = true;
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
        public RunCommandBuilder WithinWorkingDirectory(string path)
        {
            Command.WithinWorkingDirectory = path;
            return this;
        }
    }
}
