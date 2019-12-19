#region Usings

using System;
using Eshva.DockerCompose.Commands.Run;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Run
{
    public sealed class GivenRunCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<RunCommand, RunCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandForSpecificService()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("service1", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandWithCommandWithArguments()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1", "--arg1", "--arg2"),
                arguments => arguments.EndsWith("exec1 --arg1 --arg2", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldPlaceArgumentsInProperOrder()
        {
            TestOption(
                builder => builder.InService("service1")
                                  .CommandWithArguments("exec1", "--arg1", "--arg2")
                                  .RemoveContainerAfterRun(),
                arguments => arguments.EndsWith(
                    "run --rm service1 exec1 --arg1 --arg2",
                    StringComparison.CurrentCulture));
        }

        [Fact]
        public void ShouldBuildAttachedCommandByDefault()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => !arguments.Contains("--detach") && !arguments.Contains("-d"));
        }

        [Fact]
        public void ShouldBuildThatRemovesContainerAfterRun()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1")
                           .RemoveContainerAfterRun(),
                arguments => arguments.Contains("--rm"));
        }

        [Fact]
        public void ShouldBuildDetachedCommand()
        {
            TestOption(
                builder => builder
                           .Detached()
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("--detach"));
        }

        [Fact]
        public void ShouldBuildCommandThatExecutesAsUser()
        {
            TestOption(
                builder => builder
                           .AsUser("user1")
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("--user user1"));
        }

        [Fact]
        public void ShouldBuildCommandWithoutTty()
        {
            TestOption(
                builder => builder
                           .WithoutTty()
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("-T"));
        }

        [Fact]
        public void ShouldBuildCommandThatSetsEnvironmentVariables()
        {
            TestOption(
                builder => builder
                           .WithEnvironmentVariable("var1", "value1")
                           .InService("service1")
                           .CommandWithArguments("exec1")
                           .WithEnvironmentVariable("var2", "value2"),
                arguments => arguments.Contains("-e var1=\"value1\" -e var2=\"value2\""));
        }

        [Fact]
        public void ShouldBuildCommandThatExecutesWithinSpecificWorkDirectory()
        {
            TestOption(
                builder => builder.WithinWorkingDirectory("/bin")
                                  .InService("service1").CommandWithArguments("exec1"),
                arguments => arguments.Contains("--workdir \"/bin\""));
        }

        [Fact]
        public void ShouldBuildCommandThanGivesContainerSpecificName()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").NameContainerAs("container1"),
                arguments => arguments.Contains("--name container1"));
        }

        [Fact]
        public void ShouldBuildCommandThatOverridesEntryPoint()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").OverrideEntryPointWith("command1"),
                arguments => arguments.Contains("--entrypoint command1"));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotStartLinkedServices()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").DoNotStartLinkedServices(),
                arguments => arguments.Contains("--no-deps"));
        }

        [Fact]
        public void ShouldBuildCommandThatMapsServicePortsOnHost()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").MapServicePortsOnHost(),
                arguments => arguments.Contains("--service-ports"));
        }

        [Fact]
        public void ShouldBuildCommandThatUsesServiceNetworkAliases()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").UseServiceNetworkAliases(),
                arguments => arguments.Contains("--use-aliases"));
        }

        [Fact]
        public void ShouldBuildCommandThatBindsVolumes()
        {
            TestOption(
                builder => builder.InService("service1").CommandWithArguments("exec1").BindVolumes("volume1", "volume2"),
                arguments => arguments.Contains("--volume volume1 --volume volume2"));
        }

        [Fact]
        public void ShouldBuildCommandThatMapsPorts()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1")
                           .MapPorts("host1", "container1")
                           .MapPorts("host2", "container2"),
                arguments => arguments.Contains("--publish host1:container1 --publish host2:container2"));
        }

        [Fact]
        public void ShouldBuildCommandThatOverridesContainerLabels()
        {
            TestOption(
                builder => builder
                           .InService("service1")
                           .CommandWithArguments("exec1")
                           .AddOrOverrideContainerLabel("label1", "label value1")
                           .AddOrOverrideContainerLabel("label2", "label value2"),
                arguments => arguments.Contains("--label label1=\"label value1\" --label label2=\"label value2\""));
        }

        protected override RunCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            RunCommand.WithFilesAndStarter(starter, files);
    }
}
