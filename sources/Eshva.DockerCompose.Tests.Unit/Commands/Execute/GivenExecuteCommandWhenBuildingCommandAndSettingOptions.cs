#region Usings

using System;
using Eshva.DockerCompose.Commands.Execute;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Execute
{
    public sealed class GivenExecuteCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<ExecuteCommand, ExecuteCommandBuilder>
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
                                  .WithExtendedPrivileges(),
                arguments => arguments.EndsWith(
                    "exec --privileged service1 exec1 --arg1 --arg2",
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
        public void ShouldBuildCommandWithExtendedPrivileges()
        {
            TestOption(
                builder => builder
                           .WithExtendedPrivileges()
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("--privileged"));
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
        public void ShouldBuildCommandThatTargetsSpecificServiceContainer()
        {
            TestOption(
                builder => builder
                           .InServiceContainer(111)
                           .InService("service1")
                           .CommandWithArguments("exec1"),
                arguments => arguments.Contains("--index 111"));
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
                arguments => arguments.Contains("--env var1=\"value1\" --env var2=\"value2\""));
        }

        [Fact]
        public void ShouldBuildCommandThatExecutesWithinSpecificWorkDirectory()
        {
            TestOption(
                builder => builder.WithinWorkingDirectory("/bin")
                                  .InService("service1").CommandWithArguments("exec1"),
                arguments => arguments.Contains("--workdir \"/bin\""));
        }

        protected override ExecuteCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            ExecuteCommand.WithFilesAndStarter(starter, files);
    }
}
