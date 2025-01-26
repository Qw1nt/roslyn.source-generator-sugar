using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Base;

public abstract class SyntaxReceiverBase<TDeclaration> : SyntaxReceiverBase
    where TDeclaration : TypeDeclarationSyntax
{
    public readonly SyntaxReceiverValidator Validator;
    
    protected SyntaxReceiverBase() : base()
    {
        Validator = new SyntaxReceiverValidator();
    }
    
    public override bool IsValidType(SyntaxNode node, CancellationToken arg2)
    {
        return node is TDeclaration;
    }

    public ItemToGeneration<TDeclaration> IsValidDeclaration(GeneratorSyntaxContext context, CancellationToken arg2)
    {
        var declaration = (TDeclaration)context.Node;
        
        if (IsValid(Validator, declaration, context) == false)
            return default;
        
        return new ItemToGeneration<TDeclaration>(declaration, context.SemanticModel);
        
        /*
        /*
        foreach (var attributeList in declaration.AttributeLists)
        {
            foreach (var attributeSyntax in attributeList.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not { } attributeSymbol)
                    continue;

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (AvailableNames.Contains(fullName) == false)
                    continue;

                var declarationSymbol = context.SemanticModel.GetDeclaredSymbol(declaration);
                var type = context.SemanticModel.Compilation!.GetTypeByMetadataName(
                    declarationSymbol!.ContainingNamespace + "." + declarationSymbol.MetadataName)!;

                if (ValidateAttributeApplier(declarationSymbol, type, context) == false)
                    continue;

                return new ItemToGeneration<TDeclaration>(declaration, context.SemanticModel);
            }
        }#1#

        return new ItemToGeneration<TDeclaration>();*/
    }
}