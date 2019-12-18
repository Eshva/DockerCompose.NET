#region Usings

using System;
using Eshva.DockerCompose.Commands.Run;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Run
{
    public sealed class GivenRunCommandCommandBuilderWhenVerifyingCommandDuringBuilding
    {
        [Fact]
        public void ShouldRequireProvidingServiceName()
        {
            ValidateOptions(
                builder => builder,
                error => error.Contains("InService", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldRequireCommandToExecute()
        {
            ValidateOptions(
                builder => builder,
                error => error.Contains("CommandWithArguments", StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<RunCommandBuilder, RunCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = RunCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
