using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <summary>
/// Helper class to extract the property names from an expression tree.
/// </summary>
/// <remarks>
/// https://stackoverflow.com/questions/31515898/traverse-an-expression-tree-and-extract-parameters
/// </remarks>
internal class MemberAccessVisitor : ExpressionVisitor {

    public MemberAccessVisitor(Type forType)
    {
        declaringType = forType;
    }

    public IList<string> PropertyNames { get; } = new List<string>();

    protected override Expression VisitMember(MemberExpression node)
    {
        if(node.Member.DeclaringType == declaringType) {
            PropertyNames.Add(node.Member.Name);
        }
        return base.VisitMember(node);
    }

    private readonly Type declaringType;
}
