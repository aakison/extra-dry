﻿namespace ExtraDry.Blazor.Tests.Models;

public class NakedParameterlessCommandInfoTests
{
    [Fact]
    public void DefaultValues()
    {
        var command = new CommandInfo(this, NakedParameterlessCommand);

        Assert.Equal(CommandContext.Regular, command.Context);
        Assert.Equal("regular", command.DisplayClass);
        Assert.Equal(CommandArguments.None, command.Arguments);
        Assert.Null(command.Icon);
        Assert.Equal("NakedParameterlessCommand", command.Method.Name);
        Assert.Equal(this, command.ViewModel);
    }

    [Fact]
    public void NakedMethodGetsCaption()
    {
        var command = new CommandInfo(this, NakedParameterlessCommand);

        Assert.Equal("Naked Parameterless Command", command.Caption);
    }

    [Fact]
    public async Task SimpleSuccessfulMethodCall()
    {
        var command = new CommandInfo(this, NakedParameterlessCommand);

        await command.ExecuteAsync();

        Assert.Equal("NakedParameterlessCommand", lastMethodCalled);
    }

    [Fact]
    public async Task AsyncSuccessfulMethodCall()
    {
        var command = new CommandInfo(this, NakedParameterlessCommandAsync);

        await command.ExecuteAsync();

        Assert.Equal("NakedParameterlessCommandAsync", lastMethodCalled);
    }

    private void NakedParameterlessCommand()
    {
        lastMethodCalled = "NakedParameterlessCommand";
    }

    private async Task NakedParameterlessCommandAsync()
    {
        await Task.Delay(1);
        lastMethodCalled = "NakedParameterlessCommandAsync";
    }

    private string lastMethodCalled = string.Empty;
}
