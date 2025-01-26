using Microsoft.CodeAnalysis;

namespace SourceGenerator.Sugar.Extensions;

public static class SymbolExtensions
{
    public static bool HasAttribute(this ISymbol symbol, string attributeName)
    {
        var attributes = symbol.GetAttributes();

        if (attributes.Length == 0)
            return false;

        return attributes.Any(x => x.AttributeClass?.Name == attributeName);
    }   
}