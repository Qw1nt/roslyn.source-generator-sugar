using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Extensions;

public static class ItemToGenerationExtensions
{
    public static INamedTypeSymbol GetNamedTypeSymbol<T>(this ItemToGeneration<T> item)
        where T : TypeDeclarationSyntax
    {
        var declarationSymbol = item.SemanticModel.GetDeclaredSymbol(item.Declaration);
        return item.SemanticModel.Compilation!.GetTypeByMetadataName(declarationSymbol!.ContainingNamespace + "." + declarationSymbol.MetadataName)!;
    }

    public static INamedTypeSymbol GetNamedTypeSymbolT1<T1, T2>(this ItemToGeneration<T1, T2> item)
        where T1 : TypeDeclarationSyntax
        where T2 : TypeDeclarationSyntax
    {
        var converted = new ItemToGeneration<T1>(item.Item1!, item.SemanticModel);
        return converted.GetNamedTypeSymbol();
    }
    
    public static INamedTypeSymbol GetNamedTypeSymbolT2<T1, T2>(this ItemToGeneration<T1, T2> item)
        where T1 : TypeDeclarationSyntax
        where T2 : TypeDeclarationSyntax
    {
        var converted = new ItemToGeneration<T2>(item.Item2!, item.SemanticModel);
        return converted.GetNamedTypeSymbol();
    } 
    
    public static INamedTypeSymbol GetNotNullNamedTypeSymbol<T>(this ItemToGeneration<T, T> item)
        where T : TypeDeclarationSyntax
    {
        var notNullItem = item.GetNotNullDeclaration();
        var converted = new ItemToGeneration<T>(notNullItem, item.SemanticModel);
        return converted.GetNamedTypeSymbol();
    }

    public static T GetNotNullDeclaration<T>(this ItemToGeneration<T, T> item)
        where T : TypeDeclarationSyntax
    {
        return item.Item1 != null ? item.Item1 : item.Item2;
    }    
}