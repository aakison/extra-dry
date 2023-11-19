using ExtraDry.Blazor.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace ExtraDry.Blazor.Tests.Models {

    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Accessed via reflection")]
    public class NakedSingleParameterCommandInfoTests {

        [Fact]
        public void DefaultValues()
        {
            var method = GetType().GetMethod("NakedSingleCommand", BindingFlags.NonPublic | BindingFlags.Instance);

            var command = method == null ? null : new CommandInfo(this, method);

            Assert.Equal(CommandContext.Alternate, command?.Context);
            Assert.Equal("alternate", command?.DisplayClass);
            Assert.Equal(CommandArguments.Single, command?.Arguments);
            Assert.Null(command?.Icon);
            Assert.Equal("NakedSingleCommand", command?.Method.Name);
            Assert.Equal(this, command?.ViewModel);
        }

        [Fact]
        public void StringValueIsSingleArgument() // String is IEnumerable<char> can fool the logic...
        {
            var method = GetType().GetMethod("NakedSingleStringCommand", BindingFlags.NonPublic | BindingFlags.Instance);

            var command = method == null ? null : new CommandInfo(this, method);

            Assert.Equal(CommandArguments.Single, command?.Arguments);
        }

        [Fact]
        public void NakedMethodGetsCaption()
        {
            var method = GetType().GetMethod("NakedSingleCommand", BindingFlags.NonPublic | BindingFlags.Instance);

            var command = method == null ? null : new CommandInfo(this, method);

            Assert.Equal("Naked Single Command", command?.Caption);
        }

        [Fact]
        public async Task SimpleSuccessfulMethodCall()
        {
            var method = GetType().GetMethod("NakedSingleCommand", BindingFlags.NonPublic | BindingFlags.Instance);
            var command = method == null ? null : new CommandInfo(this, method);

            await command!.ExecuteAsync("test");

            Assert.NotNull(command);
            Assert.Equal("NakedSingleCommand(test)", lastMethodCalled);
        }

        [Fact]
        public async Task SuccessOnSingleEvenIfNoParameter()
        {
            // Note this works because it implicitly passes a Null, which is a valid single value.
            var method = GetType().GetMethod("NakedSingleCommand", BindingFlags.NonPublic | BindingFlags.Instance);
            var command = method == null ? null : new CommandInfo(this, method);

            await command!.ExecuteAsync();

            Assert.NotNull(command);
            Assert.Equal("NakedSingleCommand()", lastMethodCalled);

        }

        [Fact]
        public async Task SimpleAsyncSuccessfulMethodCall()
        {
            var method = GetType().GetMethod("NakedSingleCommandAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var command = method == null ? null : new CommandInfo(this, method);

            if(command != null) {
                await command.ExecuteAsync("test");
            }

            Assert.NotNull(command);
            Assert.Equal("NakedSingleCommandAsync(test)", lastMethodCalled);
        }

        [Fact]
        public async Task AsyncSuccessOnSingleEvenIfNoParameter()
        {
            // Note this works because it implicitly passes a Null, which is a valid single value.
            var method = GetType().GetMethod("NakedSingleCommandAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            var command = method == null ? null : new CommandInfo(this, method);

            await command!.ExecuteAsync();

            Assert.NotNull(command);
            Assert.Equal("NakedSingleCommandAsync()", lastMethodCalled);
        }

        private void NakedSingleCommand(object value)
        {
            lastMethodCalled = $"NakedSingleCommand({value})";
        }

        private void NakedSingleStringCommand(string value)
        {
            lastMethodCalled = $"NakedSingleStringCommand({value})";
        }

        private async Task NakedSingleCommandAsync(string value)
        {
            await Task.Delay(1);
            lastMethodCalled = $"NakedSingleCommandAsync({value})";
        }

        private string lastMethodCalled = string.Empty;

    }
}
