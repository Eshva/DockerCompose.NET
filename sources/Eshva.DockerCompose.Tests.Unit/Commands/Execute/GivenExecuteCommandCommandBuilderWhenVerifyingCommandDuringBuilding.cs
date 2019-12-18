#region Usings

using System;
using Eshva.DockerCompose.Commands.Execute;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Execute
{
    public sealed class GivenExecuteCommandCommandBuilderWhenVerifyingCommandDuringBuilding
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
            Func<ExecuteCommandBuilder, ExecuteCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = ExecuteCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }
    }
}
