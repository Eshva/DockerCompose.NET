#region Usings

using System;
using Eshva.DockerCompose.Commands;
using Eshva.DockerCompose.Exceptions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

#endregion


namespace Eshva.DockerCompose.Tests.Unit.Commands.Common
{
    public sealed class GivenCommandBuilderBaseWhenBuildCalled
    {
        [Fact]
        public void ShouldVerifyCommand()
        {
            var validatorMock = new Mock<IValidator<CommandBase>>();
            validatorMock.Setup(validator => validator.Validate(It.IsAny<object>()))
                         .Returns(new ValidationResult());
            var commandMock = new Mock<CommandBase>();
            commandMock.Setup(command => command.CreateValidator()).Returns(validatorMock.Object);
            var builder = new TestCommandBuilder(commandMock.Object);

            builder.Build();

            commandMock.Verify(command => command.CreateValidator(), Times.Once());
        }

        [Fact]
        public void ShouldThrowWithErrorsIfAnyFound()
        {
            var validatorMock = new Mock<IValidator<CommandBase>>();
            validatorMock.Setup(validator => validator.Validate(It.IsAny<object>()))
                         .Returns(
                             new ValidationResult(
                                 new[]
                                 {
                                     new ValidationFailure("not-important", "error1"),
                                     new ValidationFailure("not-important", "error2")
                                 }));
            var commandMock = new Mock<CommandBase>();
            commandMock.Setup(command => command.CreateValidator()).Returns(validatorMock.Object);
            var builder = new TestCommandBuilder(commandMock.Object);

            Action build = () => builder.Build();

            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Errors.Should().Contain("error1").And.Contain("error2");
            build.Should().ThrowExactly<CommandBuildException>()
                 .Which.Message.Should().Contain("error1").And.Contain("error2");
        }

        [Fact]
        public void ShouldReturnBuiltCommandIfNoVerificationErrorsFound()
        {
            var validatorMock = new Mock<IValidator<CommandBase>>();
            validatorMock.Setup(validator => validator.Validate(It.IsAny<object>()))
                         .Returns(new ValidationResult());
            var commandMock = new Mock<CommandBase>();
            commandMock.Setup(command => command.CreateValidator()).Returns(validatorMock.Object);
            var builder = new TestCommandBuilder(commandMock.Object);

            Func<CommandBase> build = () => builder.Build();

            build.Should().NotThrow().Which.Should().BeSameAs(commandMock.Object);
        }

        private sealed class TestCommandBuilder : CommandBuilderBase<CommandBase>
        {
            public TestCommandBuilder(CommandBase command) : base(command)
            {
            }
        }
    }
}
