namespace Eshva.DockerCompose.Commands.UpProject
{
    /// <summary>
    /// The builder for <see cref="UpProjectCommand"/> command.
    /// </summary>
    public sealed class UpProjectCommandBuilder : CommandBuilderBase<UpProjectCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal UpProjectCommandBuilder(UpProjectCommand command) : base(command)
        {
        }

        /// <summary>
        /// Execute command in the attached mode.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder Attached()
        {
            Command.Attached = true;
            return this;
        }

        /// <summary>
        /// Pull images without printing progress information.
        /// </summary>
        /// <remarks>
        /// See --quiet-pull option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder WithQuietPull()
        {
            Command.WithQuietPull = true;
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
        public UpProjectCommandBuilder DoNotStartLinkedServices()
        {
            Command.DoNotStartLinkedServices = true;
            return this;
        }

        /// <summary>
        /// Recreate containers even if their configuration and image haven't changed.
        /// </summary>
        /// <remarks>
        /// See --force-recreate option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder ForceRecreateContainers()
        {
            Command.ForceRecreateContainers = true;
            return this;
        }

        /// <summary>
        /// Recreate dependent containers. Incompatible with <see cref="DoNotRecreateExistingContainers"/>.
        /// </summary>
        /// <remarks>
        /// See --always-recreate-deps option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder RecreateDependedContainers()
        {
            Command.RecreateDependedContainers = true;
            return this;
        }

        /// <summary>
        /// If containers already exist, don't recreate them. Incompatible with <see cref="ForceRecreateContainers"/>
        /// and <see cref="RecreateAnonymousVolumes"/>.
        /// </summary>
        /// <remarks>
        /// See --no-recreate option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder DoNotRecreateExistingContainers()
        {
            Command.DoNotRecreateExistingContainers = true;
            return this;
        }

        /// <summary>
        /// Don't build an image, even if it's missing. Incompatible with <see cref="ForceBuildImages"/>.
        /// </summary>
        /// <remarks>
        /// See --no-build option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder DoNotBuildMissingImages()
        {
            Command.DoNotBuildMissingImages = true;
            return this;
        }

        /// <summary>
        /// Don't start the services after creating them.
        /// </summary>
        /// <remarks>
        /// See --no-start option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder DoNotStartServices()
        {
            Command.DoNotStartServices = true;
            return this;
        }

        /// <summary>
        /// Build images before starting containers. Incompatible with <see cref="DoNotBuildMissingImages"/>.
        /// </summary>
        /// <remarks>
        /// See --build option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder ForceBuildImages()
        {
            Command.ForceBuildImages = true;
            return this;
        }

        /// <summary>
        /// Stops all containers if any container was stopped. Incompatible with <see cref="Attached"/> == false.
        /// </summary>
        /// <remarks>
        /// See --abort-on-container-exit option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder StopAllContainersIfAnyOneStopped()
        {
            Command.StopAllContainersIfAnyOneStopped = true;
            return this;
        }

        /// <summary>
        /// Remove containers for services not defined in the Compose file.
        /// </summary>
        /// <remarks>
        /// See --remove-orphans option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder RemoveOrphanContainers()
        {
            Command.RemoveOrphanContainers = true;
            return this;
        }

        /// <summary>
        /// Recreate anonymous volumes instead of retrieving data from the previous containers.
        /// Incompatible with see <see cref="DoNotRecreateExistingContainers"/>.
        /// </summary>
        /// <remarks>
        /// See --renew-anon-volumes option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder RecreateAnonymousVolumes()
        {
            Command.RecreateAnonymousVolumes = true;
            return this;
        }

        /// <summary>
        /// Use this timeout in seconds for container shutdown when attached or when
        /// containers are already running. Default is 10 seconds.
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
        public UpProjectCommandBuilder ShutdownTimeoutSeconds(int shutdownTimeoutSeconds)
        {
            Command.ShutdownTimeoutSeconds = shutdownTimeoutSeconds;
            return this;
        }

        /// <summary>
        /// Return the exit code of the <paramref name="serviceName"/> service container.
        /// Implies <see cref="StopAllContainersIfAnyOneStopped"/>.
        /// </summary>
        /// <param name="serviceName">
        /// Name of the service.
        /// </param>
        /// <remarks>
        /// See --exit-code-from option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder TakeExitCodeFromService(string serviceName)
        {
            Command.TakeExitCodeFromService = serviceName;
            return this;
        }

        /// <summary>
        /// Scale <paramref name="serviceName"/> service to <paramref name="instanceNumber"/> instances.
        /// Overrides the 'scale' setting in the Compose file if present.
        /// </summary>
        /// <param name="serviceName">
        /// Name of the service to scale.
        /// </param>
        /// <param name="instanceNumber">
        /// Number of instances.
        /// </param>
        /// <remarks>
        /// See --scale option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public UpProjectCommandBuilder ScaleService(string serviceName, int instanceNumber)
        {
            Command.Scaling.Add(serviceName, instanceNumber);
            return this;
        }
    }
}
