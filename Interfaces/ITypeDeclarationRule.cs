using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Sugar.Interfaces;

public interface ITypeDeclarationRule
{
    bool IsValid(TypeDeclarationSyntax declaration, GeneratorSyntaxContext context);
}