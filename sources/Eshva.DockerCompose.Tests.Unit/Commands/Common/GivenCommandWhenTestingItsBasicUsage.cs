#region Usings

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Commands.BuildServices;
using Eshva.DockerCompose.Commands.DownProject;
using Eshva.DockerCompose.Commands.Execute;
using Eshva.DockerCompose.Commands.KillServices;
using Eshva.DockerCompose.Commands.Logs;
using Eshva.DockerCompose.Commands.PauseServices;
using Eshva.DockerCompose.Commands.RestartServices;
using Eshva.DockerCompose.Commands.Run;
using Eshva.DockerCompose.Commands.StartServices;
using Eshva.DockerCompose.Commands.StopServices;
using Eshva.DockerCompose.Commands.UnpauseServices;
using Eshva.DockerCompose.Commands.UpProject;
using Eshva.DockerCompose.Infrastructure;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Common
{
    public sealed class GivenCommandWhenTestingItsBasicUsage
    {
        [Fact]
        public async Task ShouldAcceptKillServicesCommand()
        {
            await TestBasicCommandUsage(
                "kill",
                (files, starter) => KillServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptLogsCommand()
        {
            await TestBasicCommandUsage(
                "logs",
                (files, starter) => LogsCommand
                                    .WithFilesAndStarter(starter, files)
                                    .FromAllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptDownProjectCommand()
        {
            await TestBasicCommandUsage(
                "down",
                (files, starter) => DownProjectCommand
                                    .WithFilesAndStarter(starter, files)
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptPauseServicesCommand()
        {
            await TestBasicCommandUsage(
                "pause",
                (files, starter) => PauseServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptRestartServicesCommand()
        {
            await TestBasicCommandUsage(
                "restart",
                (files, starter) => RestartServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptStartServicesCommand()
        {
            await TestBasicCommandUsage(
                "start",
                (files, starter) => StartServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptStopServicesCommand()
        {
            await TestBasicCommandUsage(
                "stop",
                (files, starter) => StopServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptUnpauseServicesCommand()
        {
            await TestBasicCommandUsage(
                "unpause",
                (files, starter) => UnpauseServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptUpProjectCommand()
        {
            await TestBasicCommandUsage(
                "up --detach",
                (files, starter) => UpProjectCommand
                                    .WithFilesAndStarter(starter, files)
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptBuildServicesCommand()
        {
            await TestBasicCommandUsage(
                "build",
                (files, starter) => BuildServicesCommand
                                    .WithFilesAndStarter(starter, files)
                                    .AllServices()
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptExecuteCommand()
        {
            await TestBasicCommandUsage(
                "exec service1 exec1",
                (files, starter) => ExecuteCommand
                                    .WithFilesAndStarter(starter, files)
                                    .InService("service1")
                                    .CommandWithArguments("exec1")
                                    .Build());
        }

        [Fact]
        public async Task ShouldAcceptRunCommand()
        {
            await TestBasicCommandUsage(
                "run service1 exec1",
                (files, starter) => RunCommand
                                    .WithFilesAndStarter(starter, files)
                                    .InService("service1")
                                    .CommandWithArguments("exec1")
                                    .Build());
        }

        private async Task TestBasicCommandUsage<TCommand>(string command, Func<string[], IProcessStarter, TCommand> createCommand)
            where TCommand : CommandBase
        {
            var starterMock = new Mock<IProcessStarter>();
            Expression<Func<string, bool>> argumentsValidator =
                arguments => arguments.Equals($"-f \"project.yaml\" {command}", StringComparison.OrdinalIgnoreCase);
            starterMock.Setup(starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()))
                       .Returns(Task.FromResult(0));
            await createCommand(new[] { "project.yaml" }, starterMock.Object).Execute();
            starterMock.Verify(
                starter => starter.Start(It.Is(argumentsValidator), It.IsAny<TimeSpan>()),
                Times.Once());
        }
    }
}
