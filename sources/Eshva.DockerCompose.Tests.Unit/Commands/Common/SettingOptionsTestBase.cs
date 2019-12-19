#region Usings

using System;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Infrastructure;
using Moq;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Common
{
    public abstract class SettingOptionsTestBase<TCommand, TBuilder>
        where TCommand : CommandBase
        where TBuilder : CommandBuilderBase<TCommand>
    {
        protected void TestOption(
            Func<TBuilder, TBuilder> configure,
            Func<string, bool> checkArguments)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = CreateBuilder(processStarterMock.Object, "file1", "file2");
            var command = configure(builder).Build();
            command.Execute();
            processStarterMock.Verify(
                starter => starter.Start(
                    It.Is<string>(arguments => checkArguments(arguments)),
                    TimeSpan.FromDays(1)),
                Times.Once());
        }

        protected abstract TBuilder CreateBuilder(IProcessStarter starter, params string[] files);
    }
}
