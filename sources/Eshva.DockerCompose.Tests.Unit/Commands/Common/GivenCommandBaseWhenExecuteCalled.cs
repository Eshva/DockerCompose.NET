#region Usings

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Exceptions;
using Eshva.DockerCompose.Infrastructure;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Common
{
    public sealed class GivenCommandBaseWhenExecuteCalled
    {
        [Fact]
        public void ShouldExecuteDockerComposeWithNoArgumentsWithoutExceptions()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();
            execute.Should().NotThrow(
                "При условии установленного Docker Compose получение его версии всегда допустим.");
        }

        [Fact]
        public void ShouldThrowIfBadCommandExecuted()
        {
            var command = new BadCommand();
            Func<Task> execute = async () => await command.Execute();
            execute.Should().ThrowExactly<CommandExecutionException>();
        }

        [Fact]
        public async Task ShouldStartWithProperArguments()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start("--version", It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object);
            await command.Execute();
            starterMock.Verify(starter => starter.Start("--version", It.IsAny<TimeSpan>()), Times.Once());
        }

        [Fact]
        public async Task ShouldStartWithProperTimeout()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), TimeSpan.FromHours(1)))
                       .Returns(Task.FromResult(0));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            await command.Execute(TimeSpan.FromHours(1));
            starterMock.Verify(starter => starter.Start(It.IsAny<string>(), TimeSpan.FromHours(1)), Times.Once());
        }

        [Fact]
        public async Task ShouldStartWithAllProjectFilesIncluded()
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Contains("-f \"project1.yaml\"") && arguments.Contains("-f \"project2.yaml\"");
            starterMock.Setup(
                           starter => starter.Start(It.Is(argumentsValidator), TimeSpan.FromHours(1)))
                       .Returns(Task.FromResult(0));
            var command = new CommandWithProjects(starterMock.Object, "project1.yaml", "project2.yaml");
            await command.Execute(TimeSpan.FromHours(1));
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), TimeSpan.FromHours(1)),
                Times.Once());
        }

        [Fact]
        public void ShouldThrowIfCommandExecutionExceededTimeout()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Throws<TimeoutException>();
            starterMock.Setup(starter => starter.StandardOutput).Returns(() => new StringBuilder());
            starterMock.Setup(starter => starter.StandardError).Returns(() => new StringBuilder());
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();
            execute.Should()
                   .ThrowExactly<CommandExecutionException>()
                   .WithMessage("*timeout*")
                   .WithInnerExceptionExactly<TimeoutException>();
        }

        [Fact]
        public void ShouldCopyStandardOutputAndStandardErrorFromProcessStarterOnFailedExecution()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Throws<TimeoutException>();
            starterMock.Setup(starter => starter.StandardOutput).Returns(() => new StringBuilder("stdout"));
            starterMock.Setup(starter => starter.StandardError).Returns(() => new StringBuilder("stderr"));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();

            execute.Should().ThrowExactly<CommandExecutionException>();
            command.StandardOutput.ToString().Should().Be("stdout");
            command.StandardError.ToString().Should().Be("stderr");
        }

        [Fact]
        public void ShouldCopyStandardOutputAndStandardErrorFromProcessStarterOnSuccessfulExecution()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            starterMock.Setup(starter => starter.StandardOutput).Returns(() => new StringBuilder("stdout"));
            starterMock.Setup(starter => starter.StandardError).Returns(() => new StringBuilder("stderr"));
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();

            execute.Should().NotThrow();
            command.StandardOutput.ToString().Should().Be("stdout");
            command.StandardError.ToString().Should().Be("stderr");
        }

        [Fact]
        public void ShouldThrowIfCommandNotStarted()
        {
            var starterMock = new Mock<IProcessStarter>();
            starterMock.Setup(starter => starter.Start(It.IsAny<string>(), It.IsAny<TimeSpan>()))
                       .Throws<InvalidOperationException>();
            starterMock.Setup(starter => starter.StandardError).Returns(() => new StringBuilder());
            starterMock.Setup(starter => starter.StandardOutput).Returns(() => new StringBuilder());
            var command = new VersionCommand(starterMock.Object, string.Empty);
            Func<Task> execute = async () => await command.Execute();
            execute.Should()
                   .ThrowExactly<CommandExecutionException>()
                   .WithMessage("*not started*")
                   .WithInnerExceptionExactly<InvalidOperationException>();
        }

        private sealed class BadCommand : CommandBase
        {
            protected internal override IValidator CreateValidator()
            {
                return new InlineValidator<BadCommand>();
            }

            protected override string Command => "fook";

            protected override string[] PrepareArguments() =>
                new List<string>(new[] { "fook", "-it" }).ToArray();
        }

        private sealed class CommandWithProjects : CommandBase
        {
            public CommandWithProjects(IProcessStarter starter, params string[] files)
                : base(starter, files)
            {
            }

            protected internal override IValidator CreateValidator()
            {
                return new InlineValidator<CommandWithProjects>();
            }

            protected override string Command => "some";

            protected override string[] PrepareArguments() => new List<string>().ToArray();
        }

        private sealed class VersionCommand : CommandBase
        {
            public VersionCommand(IProcessStarter starter, params string[] files)
                : base(starter, files)
            {
            }

            protected internal override IValidator CreateValidator()
            {
                return new InlineValidator<VersionCommand>();
            }

            protected override string Command => string.Empty;

            protected override string[] PrepareArguments() =>
                new List<string>(new[] { "--version" }).ToArray();
        }
    }
}
