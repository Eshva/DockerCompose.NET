#region Usings

using System;
using Eshva.DockerCompose.Commands.RestartServices;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.RestartServices
{
    public sealed class GivenRestartServicesCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<RestartServicesCommand, RestartServicesCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatRestartsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" restart", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatRestartsSomeServicesInProject()
        {
            TestOption(
                builder => builder.Services("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatHasShutdownTimeout()
        {
            TestOption(
                builder => builder.Services("service1").ShutdownTimeoutSeconds(111),
                arguments => arguments.EndsWith("--timeout 111 service1", StringComparison.OrdinalIgnoreCase));
        }

        protected override RestartServicesCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            RestartServicesCommand.WithFilesAndStarter(starter, files);
    }
}
