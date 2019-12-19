#region Usings

using System;
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
    public sealed class GivenServicesCommandBaseClassWhenValidatingCommandDuringBuild
    {
        [Fact]
        public void ShouldDisallowCombiningTakeLogsFromAllAndSomeServices()
        {
            ValidateOptions(
                builder => builder.AllServices().Services("service1", "service2", "service3"),
                error => error.Equals("both", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ShouldRequireTakingLogsFromAllServicesOrSomeOfThem()
        {
            ValidateOptions(
                builder => builder,
                error => error.Equals("none", StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidateOptions(
            Func<TestServicesCommandBuilder, TestServicesCommandBuilder> configure,
            Func<string, bool> validateOptions)
        {
            var processStarterMock = new Mock<IProcessStarter>();
            var builder = TestServicesCommand.WithFilesAndStarter(processStarterMock.Object, "file1", "file2");
            Action build = () => configure(builder).Build();
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain(error => validateOptions(error));
        }

        private sealed class TestServicesCommand : ServicesCommandBase
        {
            private TestServicesCommand(IProcessStarter starter, params string[] files) : base(starter, files)
            {
            }

            public static TestServicesCommandBuilder WithFilesAndStarter(IProcessStarter starter, params string[] files) =>
                new TestServicesCommandBuilder(new TestServicesCommand(starter, files));

            protected internal override IValidator CreateValidator() => new TestServicesCommandValidator();

            protected override string Command { get; } = "test";

            protected override string[] PrepareOptions()
            {
                return new string[0];
            }
        }

        private sealed class TestServicesCommandBuilder : CommandBuilderBase<TestServicesCommand>
        {
            internal TestServicesCommandBuilder(TestServicesCommand command) : base(command)
            {
            }

            public TestServicesCommandBuilder AllServices()
            {
                Command.DoForAllServices = true;
                return this;
            }

            public TestServicesCommandBuilder Services(params string[] services)
            {
                Command.Services.AddRange(services);
                return this;
            }
        }

        private sealed class TestServicesCommandValidator : ServicesCommandValidatorBase<TestServicesCommand>
        {
            public TestServicesCommandValidator() : base("both", "none")
            {
            }
        }
    }
}
