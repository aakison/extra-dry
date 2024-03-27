using ExtraDry.Server.Security;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace ExtraDry.Server.Tests.Security;

public class AbacAuthorizationTests
{

    [Fact]
    public void DefaultConditionAllowAnonymous()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAnonymous", new AbacCondition() },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAnonymous"]
                }
            ],
        };
        var abac = new AbacAuthorizationHelper(options);
        var user = User([], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Fact]
    public void RoleOnlyConditionAuthorizes()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAgent"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Fact]
    public void WrongRoleNotAuthorized()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Create],
                    Conditions = ["IsAgent"]
                },
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAdmin"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.False(authorized);
    }

    [Fact]
    public void TypeMatchesAnyFromList()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["Fake1", "TestTarget", "Fake2"], // <-- TestTarget is in the list, but not the whole list.
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAgent"]
                }
            ],
        };
        var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Fact]
    public void OperationMatchesAnyFromList()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Create, AbacOperation.Read, AbacOperation.Execute],
                    Conditions = ["IsAgent"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Theory]
    [InlineData("acme-corp")] // match static value
    [InlineData("{route.tenant}")] // match route parameter
    [InlineData("{target.Tenant}")] // match property in target
    [InlineData("{attribute.tenant}")] // match attribute on target 
    public void ConditionClaimMatchesValue(string claimValue)
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsStakeholder", new AbacCondition() {
                    Claims = new Dictionary<string, string>() { { "stakeholder", claimValue } }
                } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsStakeholder"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User([], new Dictionary<string, string> { { "stakeholder", "acme-corp" } });
        var route = Route(new Dictionary<string, string?> { { "tenant", "acme-corp" } });
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Theory]
    [InlineData("Bob")] // match static value
    [InlineData("{user.sub}")] // match from user claim
    public void AttributeClaimMatchesValue(string attributeValue)
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAssignee", new AbacCondition() {
                    Attributes = new Dictionary<string, string>() { { "AssignedUser", attributeValue } }
                } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAssignee"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User([], new Dictionary<string, string> {
            { "stakeholder", "acme-corp" },
            { "sub", "Bob" }
        });
        var route = Route(new Dictionary<string, string?> { { "tenant", "acme-corp" } });
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Theory]
    [InlineData("Alice")] // wrong static value
    [InlineData("{user.missing}")] // no match from missing user claim
    [InlineData("{user.stakeholder}")] // no match from another user claim
    public void AttributeClaimDoesntMatchesValue(string attributeValue)
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAssignee", new AbacCondition() {
                    Attributes = new Dictionary<string, string>() { { "AssignedUser", attributeValue } }
                } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAssignee"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User([], new Dictionary<string, string> {
            { "stakeholder", "acme-corp" },
            { "sub", "Bob" }
        });
        var route = Route(new Dictionary<string, string?> { { "tenant", "acme-corp" } });
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.False(authorized);
    }

    [Fact]
    public void MultiplePoliciesAllSucceed()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAgent"]
                },
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAdmin"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent", "Admin"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.True(authorized);
    }

    [Fact]
    public void MultiplePoliciesSubsetFails()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAgent"]
                },
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAdmin"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.False(authorized);
    }

    [Theory]
    [InlineData("Admin", false)]
    [InlineData("Agent", true)]
    public void EmptyPolicyTypeListAppliesToAllTypes(string role, bool shouldAuthorize)
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = [], // <-- applies to all types
                    Operations = [AbacOperation.Read],
                    Conditions = ["IsAgent"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User([role], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.Equal(shouldAuthorize, authorized);
    }

    [Theory]
    [InlineData("Admin", false)]
    [InlineData("Agent", true)]
    public void EmptyPolicyOperationListAppliesToAllOperations(string role, bool shouldAuthorize)
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [], // <-- applies to all operations
                    Conditions = ["IsAgent"]
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User([role], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.Equal(shouldAuthorize, authorized);
    }

    [Fact]
    public void EmptyConditionListFails()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsAgent", new AbacCondition() { Roles = ["Agent"] } },
                { "IsAdmin", new AbacCondition() { Roles = ["Admin"] } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = []  // <-- forces fail
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent", "Admin"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.False(authorized);
    }

    [Fact]
    public void InvalidPropetyFails()
    {
        var options = new AbacOptions() {
            Conditions = {
                { "IsReviewer", new AbacCondition() {
                    Attributes = new Dictionary<string, string>() { { "ReviewingUser", "Bob" } } // <-- invalid property
                } },
            },
            Policies = [
                new AbacPolicy {
                    Types = ["TestTarget"],
                    Operations = [AbacOperation.Read],
                    Conditions = [ "IsReviewer" ] 
                }
            ],
        };
                var abac = new AbacAuthorizationHelper(options);
        var user = User(["Agent", "Admin"], []);
        var route = Route([]);
        var target = new TestTarget();

        var authorized = abac.IsAuthorized(user, route, target, AbacOperation.Read);

        Assert.False(authorized);
    }

    private ClaimsPrincipal User(string[] roles, Dictionary<string, string> claims) => 
        new(new ClaimsIdentity(
            roles.Select(e => new Claim(ClaimTypes.Role, e))
            .Union(claims.Select(e => new Claim(e.Key, e.Value)))            
        ));

    private RouteValueDictionary Route(Dictionary<string, string?> routes) => 
        new(
            routes.Select(e => KeyValuePair.Create(e.Key, e.Value))
            .Union([ KeyValuePair.Create("controller", "TestController"), KeyValuePair.Create("action", "TestEntity") ])
        );

    public class TestTarget : IAttributed { 
        public string Tenant { get; set; } = "acme-corp";

        public string AssignedUser { get; set; } = "Bob";

        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string> { 
            { "tenant", "acme-corp" },
            { "AssignedUser", "Bob" }
        };
    }

}
