using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Base;

public abstract class SyntaxReceiverBase<T1, T2> : SyntaxReceiverBase
    where T1 : TypeDeclarationSyntax
    where T2 : TypeDeclarationSyntax
{
    public readonly SyntaxReceiverValidator T1Validator;
    public readonly SyntaxReceiverValidator T2Validator;

    protected SyntaxReceiverBase()
    {
        T1Validator = new SyntaxReceiverValidator();
        T2Validator = new SyntaxReceiverValidator();
    }

    public override bool IsValidType(SyntaxNode node, CancellationToken arg2)
    {
        return node is T1 == true || node is T2 == true;

        /*if (node is T1 t1 == true && t1.AttributeLists.Count > 0)
            return true;

        if (node is T2 t2 == true && t2.AttributeLists.Count > 0)
            return true;

        return false;*/
    }

    public ItemToGeneration<T1, T2> IsValidDeclaration(GeneratorSyntaxContext context, CancellationToken arg2)
    {
        var declaration = (TypeDeclarationSyntax)context.Node;

        if (declaration is T1 t1Declaration)
        {
            if (IsValid(T1Validator, declaration, context) == true)
                return new ItemToGeneration<T1, T2>(t1Declaration, null!, context.SemanticModel);
        }

        if (declaration is T2 t2Declaration)
        {
            if (IsValid(T2Validator, declaration, context) == true)
                return new ItemToGeneration<T1, T2>(null!, t2Declaration, context.SemanticModel);
        }

        return default;
    }
}