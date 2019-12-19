#region Usings

using System.Collections.Generic;
using Eshva.DockerCompose.Extensions;
using Eshva.DockerCompose.Infrastructure;

#endregion


namespace Eshva.DockerCompose.Commands
{
    /// <summary>
    /// Base class for commands with list of services.
    /// </summary>
    public abstract class ServicesCommandBase : CommandBase
    {
        /// <inheritdoc cref="CommandBase"/>
        protected ServicesCommandBase(IProcessStarter starter, params string[] files) : base(starter, files)
        {
        }

        /// <inheritdoc cref="CommandBase"/>
        protected ServicesCommandBase(params string[] files) : base(files)
        {
        }

        /// <summary>
        /// Gets or sets should command be applied for all services in the project or don't.
        /// </summary>
        internal bool DoForAllServices { get; set; }

        /// <summary>
        /// Gets list of selected services this command should be applied to.
        /// </summary>
        internal List<string> Services { get; } = new List<string>();

        /// <inheritdoc cref="CommandBase.PrepareArguments"/>
        protected sealed override string[] PrepareArguments()
        {
            var arguments = new List<string>(PrepareOptions());

            arguments.AddConditionally(
                !string.IsNullOrWhiteSpace(string.Join(" ", Services)),
                string.Join(" ", Services));

            return arguments.ToArray();
        }

        /// <summary>
        /// Prepare options of the command for Docker Compose command-line interface.
        /// </summary>
        /// <returns></returns>
        protected abstract string[] PrepareOptions();
    }
}
