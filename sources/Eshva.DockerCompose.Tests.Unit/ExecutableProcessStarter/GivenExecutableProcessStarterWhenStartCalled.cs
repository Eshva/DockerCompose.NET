#region Usings

using System;
using System.Threading.Tasks;
using Eshva.DockerCompose.Exceptions;
using FluentAssertions;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.ExecutableProcessStarter
{
    public sealed class GivenExecutableProcessStarterWhenStartCalled
    {
        [Fact]
        public void ShouldThrowIfExecutableNotFound()
        {
            var starter = new Infrastructure.ExecutableProcessStarter("unknown-process-name");
            Func<Task<int>> start = () => starter.Start(string.Empty, TimeSpan.FromDays(1));
            start.Should().Throw<ProcessStartException>();
        }

        [Fact]
        public void ShouldBreakExecutionAfterSpecifiedAmountOfTime()
        {
            var starter = new Infrastructure.ExecutableProcessStarter("docker-compose");
            Func<Task> execute = async () => await starter.Start(string.Empty, TimeSpan.FromMilliseconds(1));
            execute.Should().ThrowExactly<TimeoutException>();
        }

        [Fact]
        public async Task ShouldCaptureProcessStandardOutput()
        {
            var starter = new Infrastructure.ExecutableProcessStarter("docker");
            await starter.Start("--version", TimeSpan.FromDays(1));
            starter.StandardOutput.ReadToEnd().Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ShouldCaptureProcessStandardError()
        {
            var starter = new Infrastructure.ExecutableProcessStarter("docker");
            await starter.Start(string.Empty, TimeSpan.FromDays(1));
            starter.StandardError.ReadToEnd().Length.Should().BeGreaterThan(0);
        }
    }
}
