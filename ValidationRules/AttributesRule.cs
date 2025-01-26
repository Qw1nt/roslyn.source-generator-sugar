using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.ValidationRules;

public class AttributesRule : ITypeDeclarationRule
{
    private readonly HashSet<string> _attributesNames;

    public AttributesRule()
    {
        _attributesNames = new HashSet<string>();
    }
    
    public AttributesRule(HashSet<string> attributesNames)
    {
        _attributesNames = attributesNames;
    }

    public AttributesRule Add(string attributeNamespace, string attributeName)
    {
        _attributesNames.Add(attributeNamespace + "." + attributeName);
        return this;
    }

    public bool IsValid(TypeDeclarationSyntax declaration, GeneratorSyntaxContext context)
    {
        foreach (var attributeList in declaration.AttributeLists)
        {
            foreach (var attributeSyntax in attributeList.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not { } attributeSymbol)
                    continue;

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (_attributesNames.Contains(fullName) == true)
                    return true;
            }
        }

        return false;
    }
}