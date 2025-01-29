using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Sugar.SyntaxWalkers;

public class ObjectCreationCollector : CSharpSyntaxWalker
{
    private readonly List<ObjectCreationExpressionSyntax> _creation;

    public ObjectCreationCollector(List<ObjectCreationExpressionSyntax> creation)
    {
        _creation = creation;
    }

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        _creation.Add(node);
        base.VisitObjectCreationExpression(node);
    }
}