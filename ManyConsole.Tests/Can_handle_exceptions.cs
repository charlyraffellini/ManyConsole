using System;
using System.IO;
using NJasmine;

namespace ManyConsole.Tests
{
    public class Can_handle_exceptions : GivenWhenThenFixture
    {
        public class WithHandlerCommand : ConsoleCommand
        {
            public WithHandlerCommand()
            {
                IsCommand("handle-exception");
                HasExceptionHandler(typeof(TestException), (e, w) =>
                {
                    w.WriteLine($"The message of the exception is: {e.Message}");
                });
            }

            public override int Run(string[] remainingArguments)
            {
                throw new TestException("test message");
            }
        }

        public override void Specify()
        {
            //it("can can handle exception", () =>
            //{
            //    var output = new StringWriter();
            //    var command = new Can_handle_exceptions.WithHandlerCommand();

            //    var exitCode = arrange(() => ConsoleCommandDispatcher.DispatchCommand(command, new string[0], output));

            //    expect(() => exitCode == -1);
            //    expect(() => String.IsNullOrEmpty(output.ToString()));
            //});

            given("a command with exceptions handler", delegate
            {
                when("it throws exception whe is called", delegate ()
                {

                    var output = run_command();

                    then("the output shows the logging corresponding to the handler", delegate ()
                    {
                        expect(() => output.Contains($"Command have thown exception: {typeof(TestException).FullName}"));
                        expect(() => output.Contains($"The message of the exception is: test message"));
                    });
                });
            });
        }

        private string run_command()
        {
            return arrange(delegate
            {
                var withHandlerCommand = new Can_handle_exceptions.WithHandlerCommand();
                var stringWriter = new StringWriter();

                ConsoleCommandDispatcher.DispatchCommand(withHandlerCommand, new string[0], stringWriter);

                return stringWriter.ToString();
            });
        }
    }

    public class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
        }
    }
}