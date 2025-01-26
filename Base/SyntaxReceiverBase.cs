using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Base
{
    public abstract class SyntaxReceiverBase
    {
        public abstract bool IsValidType(SyntaxNode node, CancellationToken arg2);

        protected bool IsValid(SyntaxReceiverValidator validator, TypeDeclarationSyntax declaration, GeneratorSyntaxContext context)
        {
            foreach (var declarationRule in validator.GetTypeRules())
            {
                if (declarationRule.IsValid(declaration, context) == false)
                    return false;
            }

            var declarationSymbol = context.SemanticModel.GetDeclaredSymbol(declaration);
            var type = context.SemanticModel.Compilation!.GetTypeByMetadataName(declarationSymbol!.ContainingNamespace + "." + declarationSymbol.MetadataName)!;
        
            foreach (var symbolRule in validator.GetSymbolRules())
            {
                if (symbolRule.IsValid(declarationSymbol, type, context) == false)
                    return false;
            }

            return true;
        }
    }
}