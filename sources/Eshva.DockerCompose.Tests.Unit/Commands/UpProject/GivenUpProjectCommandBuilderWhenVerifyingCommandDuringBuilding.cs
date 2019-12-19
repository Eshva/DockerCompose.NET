#region Usings

using System;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.UpProject
{
    public sealed class GivenUpProjectCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldDisallowCombiningOfDetachingAndStoppingAllContainersIfAnyOneStopped()
        {
            ValidateOptions(
                builder => builder.StopAllContainersIfAnyOneStopped(),
                error => error.Contains("detach", StringComparison.OrdinalIgnoreCase) &&
                         error.Contains("stop", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldDisallowCombiningOfDoNotRecreateExistingContainersAndRecreateDependedContainers()
        {
            ValidateOptions(
                builder => builder.DoNotRecreateExistingContainers().RecreateDependedContainers(),
                error => error.Contains("DoNotRecreateExistingContainers", StringComparison.OrdinalIgnoreCase) &&
                         error.Contains("RecreateDependedContainers", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldDisallowCombiningOfDoNotRecreateExistingContainersAndForceRecreateContainers()
        {
            ValidateOptions(
                builder => builder.DoNotRecreateExistingContainers().ForceRecreateContainers(),
                error => error.Contains("DoNotRecreateExistingContainers", StringComparison.OrdinalIgnoreCase) &&
                         error.Contains("ForceRecreateContainers", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldDisallowCombiningOfDoNotRecreateExistingContainersAndRecreateAnonymousVolumes()
        {
            ValidateOptions(
                builder => builder.DoNotRecreateExistingContainers().RecreateAnonymousVolumes(),
                error => error.Contains("DoNotRecreateExistingContainers", StringComparison.OrdinalIgnoreCase) &&
                         error.Contains("RecreateAnonymousVolumes", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldDisallowCombiningOfDoNotBuildMissingImagesAndForceBuildImages()
        {
            ValidateOptions(
                builder => builder.DoNotBuildMissingImages().ForceBuildImages(),
                error => error.Contains("DoNotBuildMissingImages", StringComparison.OrdinalIgnoreCase) &&
                         error.Contains("ForceBuildImages", StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<UpProjectCommandBuilder, UpProjectCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = UpProjectCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
