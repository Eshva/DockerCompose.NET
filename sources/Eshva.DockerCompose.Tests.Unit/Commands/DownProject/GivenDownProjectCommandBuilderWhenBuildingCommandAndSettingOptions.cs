#region Usings

using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.DownProject
{
    public sealed class GivenDownProjectCommandBuilderWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<DownProjectCommand, DownProjectCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatRemovesAllImages()
        {
            TestOption(
                builder => builder.RemoveAllImages(),
                arguments => arguments.Contains("--rmi all"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesLocalImages()
        {
            TestOption(
                builder => builder.RemoveLocalImages(),
                arguments => arguments.Contains("--rmi local"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesVolumes()
        {
            TestOption(
                builder => builder.RemoveVolumes(),
                arguments => arguments.Contains("--volumes"));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesOrphanContainers()
        {
            TestOption(
                builder => builder.RemoveOrphanContainers(),
                arguments => arguments.Contains("--remove-orphans"));
        }

        [Fact]
        public void ShouldBuildCommandThatHasShutdownTimeoutSeconds()
        {
            TestOption(
                builder => builder.ShutdownTimeoutSeconds(111),
                arguments => arguments.Contains("--timeout 111"));
        }

        protected override DownProjectCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            DownProjectCommand.WithFilesAndStarter(starter, files);
    }
}
