#region Usings

using Eshva.DockerCompose.Commands.UpProject;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.UpProject
{
    public sealed class GivenUpProjectCommandBuilderWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<UpProjectCommand, UpProjectCommandBuilder>
    {
        [Fact]
        public void ShouldBuildDetachedCommandByDefault()
        {
            TestOption(
                builder => builder,
                arguments => arguments.Contains("--detach"));
        }

        [Fact]
        public void ShouldBuildAttachedCommand()
        {
            TestOption(
                builder => builder.Attached(),
                arguments => !arguments.Contains("--detach") && !arguments.Contains("-d"));
        }

        [Fact]
        public void ShouldBuildCommandWithQuietPull()
        {
            TestOption(
                builder => builder.WithQuietPull(),
                arguments => arguments.Contains("--quiet-pull"));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotStartLinkedServices()
        {
            TestOption(
                builder => builder.DoNotStartLinkedServices(),
                arguments => arguments.Contains("--no-deps"));
        }

        [Fact]
        public void ShouldBuildCommandThatForcesRecreateContainers()
        {
            TestOption(
                builder => builder.ForceRecreateContainers(),
                arguments => arguments.Contains("--force-recreate"));
        }

        [Fact]
        public void ShouldBuildCommandThatRecreatesDependedContainers()
        {
            TestOption(
                builder => builder.RecreateDependedContainers(),
                arguments => arguments.Contains("--always-recreate-deps"));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotRecreateExistingContainers()
        {
            TestOption(
                builder => builder.DoNotRecreateExistingContainers(),
                arguments => arguments.Contains("--no-recreate"));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotBuildMissingImages()
        {
            TestOption(
                builder => builder.DoNotBuildMissingImages(),
                arguments => arguments.Contains("--no-build"));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotStartServices()
        {
            TestOption(
                builder => builder.DoNotStartServices(),
                arguments => arguments.Contains("--no-start"));
        }

        [Fact]
        public void ShouldBuildCommandThatForcesBuildImages()
        {
            TestOption(
                builder => builder.ForceBuildImages(),
                arguments => arguments.Contains("--build"));
        }

        [Fact]
        public void ShouldBuildCommandThatStopsAllContainersIfAnyOneStopped()
        {
            TestOption(
                builder => builder.Attached().StopAllContainersIfAnyOneStopped(),
                arguments => arguments.Contains("--abort-on-container-exit"));
        }

        [Fact]
        public void ShouldBuildCommandThatRecreatesAnonymousVolumes()
        {
            TestOption(
                builder => builder.RecreateAnonymousVolumes(),
                arguments => arguments.Contains("--renew-anon-volumes"));
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

        [Fact]
        public void ShouldBuildCommandThatTakesExitCodeFromSpecificService()
        {
            TestOption(
                builder => builder.TakeExitCodeFromService("service1"),
                arguments => arguments.Contains("--exit-code-from service1"));
        }

        [Fact]
        public void ShouldBuildCommandThatCanScaleServices()
        {
            TestOption(
                builder => builder.ScaleService("service1", 5).ScaleService("service2", 10),
                arguments => arguments.Contains("--scale service1=5") && arguments.Contains("--scale service2=10"));
        }

        protected override UpProjectCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            UpProjectCommand.WithFilesAndStarter(starter, "file1", "file2");
    }
}
