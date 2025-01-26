using Microsoft.CodeAnalysis;

namespace SourceGenerator.Sugar.Interfaces;

public interface ISymbolValidationRule
{
    public abstract bool IsValid(ISymbol declarationSymbol, INamedTypeSymbol type, GeneratorSyntaxContext context);
}