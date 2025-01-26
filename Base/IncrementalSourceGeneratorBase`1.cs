using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Base;

public abstract class IncrementalSourceGeneratorBase<T1> : IncrementalSourceGeneratorBase
    where T1 : TypeDeclarationSyntax
{
    protected readonly SyntaxReceiverBase<T1> SyntaxReceiverBase;
    
    public IncrementalSourceGeneratorBase(SyntaxReceiverBase<T1> syntaxReceiverBase)
    {
        SyntaxReceiverBase = syntaxReceiverBase;
    }

    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var itemsToGeneration = context.SyntaxProvider
            .CreateSyntaxProvider(SyntaxReceiverBase.IsValidType, SyntaxReceiverBase.IsValidDeclaration)
            .Where(x => x is { SemanticModel: not null })
            .Collect();

        context.RegisterSourceOutput(itemsToGeneration, Execute);
    }

    protected abstract void Execute(SourceProductionContext context, ImmutableArray<ItemToGeneration<T1>> items);
}