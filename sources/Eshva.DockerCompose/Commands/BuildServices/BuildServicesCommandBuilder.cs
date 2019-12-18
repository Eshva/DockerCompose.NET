namespace Eshva.DockerCompose.Commands.BuildServices
{
    /// <summary>
    /// The builder for <see cref="BuildServicesCommand"/> command.
    /// </summary>
    public sealed class BuildServicesCommandBuilder : CommandBuilderBase<BuildServicesCommand>
    {
        /// <inheritdoc cref="CommandBuilderBase{TCommand}"/>
        internal BuildServicesCommandBuilder(BuildServicesCommand command) : base(command)
        {
        }

        /// <summary>
        /// Build all services.
        /// </summary>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder AllServices()
        {
            Command.DoForAllServices = true;
            return this;
        }

        /// <summary>
        /// Build only from <paramref name="services"/>.
        /// </summary>
        /// <param name="services">
        /// Names of services to build.
        /// </param> 
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder Services(params string[] services)
        {
            Command.Services.AddRange(services);
            return this;
        }

        /// <summary>
        /// Compress the build context using gzip.
        /// </summary>
        /// <remarks>
        /// See --compress option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder CompressBuildContext()
        {
            Command.CompressBuildContext = true;
            return this;
        }

        /// <summary>
        /// Always remove intermediate containers.
        /// </summary>
        /// <remarks>
        /// See --force-rm option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder RemoveIntermediateContainers()
        {
            Command.RemoveIntermediateContainers = true;
            return this;
        }

        /// <summary>
        /// Do not use cache when building the image.
        /// </summary>
        /// <remarks>
        /// See --no-cache option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder DoNotUseCache()
        {
            Command.DoNotUseCache = true;
            return this;
        }

        /// <summary>
        /// Always attempt to pull a newer version of the image.
        /// </summary>
        /// <remarks>
        /// See --pull option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder AlwaysPullImages()
        {
            Command.AlwaysPullImages = true;
            return this;
        }

        /// <summary>
        /// Sets memory limit for the build container.
        /// </summary>
        /// <param name="memoryLimitInMegabytes">
        /// Memory limit im megabytes.
        /// </param>
        /// <remarks>
        /// See --memory option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder WithLimitedMemory(int memoryLimitInMegabytes)
        {
            Command.WithLimitedMemory = memoryLimitInMegabytes;
            return this;
        }

        /// <summary>
        /// Set build-time variable for services. Can be called many times.
        /// </summary>
        /// <param name="name">
        /// The name of variable.
        /// </param>
        /// <param name="value">
        /// The value of variable.
        /// </param>
        /// <remarks>
        /// See --build-arg option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder WithBuildTimeVariable(string name, string value)
        {
            Command.BuildTimeVariables.Add(name, value);
            return this;
        }

        /// <summary>
        /// Build images in parallel.
        /// </summary>
        /// <remarks>
        /// See --parallel option.
        /// </remarks>
        /// <returns>
        /// The same builder.
        /// </returns>
        public BuildServicesCommandBuilder InParallel()
        {
            Command.InParallel = true;
            return this;
        }
    }
}
