#region Usings

using System;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.StartServices
{
    public sealed class GivenStartServicesCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<StartServicesCommand, StartServicesCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatStartsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" start", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStartsSomeServicesInProject()
        {
            TestOption(
                builder => builder.Services("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        protected override StartServicesCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            StartServicesCommand.WithFilesAndStarter(starter, files);
    }
}
