using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Sugar.Extensions;

public static class ExpressionSyntaxExtensions
{
    public static ITypeSymbol? GetTypeSymbol(this ExpressionSyntax typeSyntax, SemanticModel semanticModel)
    {
        if (typeSyntax.SyntaxTree != semanticModel.SyntaxTree)
            semanticModel = semanticModel.Compilation.GetSemanticModel(typeSyntax.SyntaxTree);
        
        return semanticModel.GetTypeInfo(typeSyntax).Type;
    }  
}