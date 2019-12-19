#region Usings

using System;
using Eshva.DockerCompose.Commands.StopServices;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StopServices
{
    public sealed class GivenStopServicesCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<StopServicesCommand, StopServicesCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatStopsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" stop", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStopsSomeServicesInProject()
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

        protected override StopServicesCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            StopServicesCommand.WithFilesAndStarter(starter, files);
    }
}
