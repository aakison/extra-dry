using ExtraDry.Server.Security;

namespace ExtraDry.Server.Tests.Security;

public class AbacConditionTests
{

    [Fact]
    public void DefaultAllowAnonymous()
    {
        var condition = new AbacCondition();

        Assert.True(condition.AllowAnonymous);
    }

}
