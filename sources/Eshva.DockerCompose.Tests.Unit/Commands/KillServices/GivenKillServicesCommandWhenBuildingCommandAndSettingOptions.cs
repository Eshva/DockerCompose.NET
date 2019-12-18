#region Usings

using System;
using Eshva.DockerCompose.Commands.KillServices;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.KillServices
{
    public sealed class GivenKillServicesCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<KillServicesCommand, KillServicesCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatKillsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" kill", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatKillsSomeServicesInProject()
        {
            TestOption(
                builder => builder.Services("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatSendsCustomSignalToServices()
        {
            TestOption(
                builder => builder.AllServices().WithSignal("SIGINT"),
                arguments => arguments.Contains("-s SIGINT"));
        }

        protected override KillServicesCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            KillServicesCommand.WithFilesAndStarter(starter, files);
    }
}
