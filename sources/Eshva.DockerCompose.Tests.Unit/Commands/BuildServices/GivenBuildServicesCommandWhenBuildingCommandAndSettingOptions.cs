#region Usings

using System;
using Eshva.DockerCompose.Commands.BuildServices;
using Eshva.DockerCompose.Infrastructure;
using Eshva.DockerCompose.Tests.Unit.Commands.Common;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.BuildServices
{
    public sealed class GivenBuildServicesCommandWhenBuildingCommandAndSettingOptions
        : SettingOptionsTestBase<BuildServicesCommand, BuildServicesCommandBuilder>
    {
        [Fact]
        public void ShouldBuildCommandThatStopsAllServicesInProject()
        {
            TestOption(
                builder => builder.AllServices(),
                arguments => arguments.Equals("-f \"file1\" -f \"file2\" build", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatStopsSomeServicesInProject()
        {
            TestOption(
                builder => builder.Services("service1", "service2", "service3"),
                arguments => arguments.EndsWith("service1 service2 service3", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatCompressesBuildContext()
        {
            TestOption(
                builder => builder.AllServices().CompressBuildContext(),
                arguments => arguments.Contains("--compress", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatRemovesIntermediateContainers()
        {
            TestOption(
                builder => builder.AllServices().RemoveIntermediateContainers(),
                arguments => arguments.Contains("--force-rm", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatDoesNotUseCache()
        {
            TestOption(
                builder => builder.AllServices().DoNotUseCache(),
                arguments => arguments.Contains("--no-cache", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatAlwaysPullImages()
        {
            TestOption(
                builder => builder.AllServices().AlwaysPullImages(),
                arguments => arguments.Contains("--pull", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatHasLimitedMemoryDuringBuild()
        {
            TestOption(
                builder => builder.AllServices().WithLimitedMemory(111),
                arguments => arguments.Contains("--memory 111", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatSpecifiesBuildTimeVariables()
        {
            TestOption(
                builder => builder.AllServices()
                                  .WithBuildTimeVariable("var1", "111")
                                  .WithBuildTimeVariable("var2", "222"),
                arguments => arguments.Contains("--build-arg var1=\"111\"", StringComparison.OrdinalIgnoreCase) &&
                             arguments.Contains("--build-arg var2=\"222\"", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldBuildCommandThatBuildsInParallel()
        {
            TestOption(
                builder => builder.AllServices().InParallel(),
                arguments => arguments.Contains("--parallel", StringComparison.OrdinalIgnoreCase));
        }

        protected override BuildServicesCommandBuilder CreateBuilder(IProcessStarter starter, params string[] files) =>
            BuildServicesCommand.WithFilesAndStarter(starter, files);
    }
}
