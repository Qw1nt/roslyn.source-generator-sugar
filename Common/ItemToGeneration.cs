using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator.Sugar.Common;

public readonly record struct ItemToGeneration<TDeclaration>(TDeclaration Declaration, SemanticModel SemanticModel)
    where TDeclaration : TypeDeclarationSyntax
{
    public TDeclaration Declaration { get; } = Declaration;

    public SemanticModel SemanticModel { get; } = SemanticModel;
}

public readonly struct ItemToGeneration<T1, T2> : IEquatable<ItemToGeneration<T1, T2>> where T1 : TypeDeclarationSyntax
    where T2 : TypeDeclarationSyntax
{
    public ItemToGeneration(T1 item1, T2 item2, SemanticModel semanticModel)
    {
        Item1 = item1;
        Item2 = item2;
        SemanticModel = semanticModel;
    }

    public T1? Item1 { get; }

    public T2? Item2 { get; }
    
    public SemanticModel SemanticModel { get; }

    public bool Equals(ItemToGeneration<T1, T2> other)
    {
        return EqualityComparer<T1?>.Default.Equals(Item1, other.Item1) && 
               EqualityComparer<T2?>.Default.Equals(Item2, other.Item2) &&
               SemanticModel.Equals(other.SemanticModel);
    }

    public override bool Equals(object? obj)
    {
        return obj is ItemToGeneration<T1, T2> other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = EqualityComparer<T1?>.Default.GetHashCode(Item1);
            hashCode = (hashCode * 397) ^ EqualityComparer<T2?>.Default.GetHashCode(Item2);
            hashCode = (hashCode * 397) ^ SemanticModel.GetHashCode();
            return hashCode;
        }
    }
}