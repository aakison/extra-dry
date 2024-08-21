using System.Linq.Expressions;

namespace ExtraDry.Server;

/// <summary>
/// Helper class to extract the property names from an expression tree.
/// </summary>
/// <remarks>
/// https://stackoverflow.com/questions/31515898/traverse-an-expression-tree-and-extract-parameters
/// </remarks>
internal class MemberAccessVisitor(
    Type forType)
    : ExpressionVisitor
{
    public IList<string> PropertyNames { get; } = [];

    protected override Expression VisitMember(MemberExpression node)
    {
        if(node.Member.DeclaringType == forType) {
            PropertyNames.Add(node.Member.Name);
        }
        return base.VisitMember(node);
    }
}
