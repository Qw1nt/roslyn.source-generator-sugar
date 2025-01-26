using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.Common;

public class SyntaxReceiverValidator
{
    private readonly HashSet<ITypeDeclarationRule> _typeRules;
    private readonly HashSet<ISymbolValidationRule> _symbolRules;

    public SyntaxReceiverValidator()
    {
        _typeRules = new HashSet<ITypeDeclarationRule>();
        _symbolRules = new HashSet<ISymbolValidationRule>();
    }

    public IReadOnlyCollection<ITypeDeclarationRule> GetTypeRules()
    {
        return _typeRules;
    }

    public IReadOnlyCollection<ISymbolValidationRule> GetSymbolRules()
    {
        return _symbolRules;
    }

    public SyntaxReceiverValidator AddTypeRule(ITypeDeclarationRule rule)
    {
        _typeRules.Add(rule);
        return this;
    }

    public SyntaxReceiverValidator AddSymbolRule(ISymbolValidationRule rule)
    {
        _symbolRules.Add(rule);
        return this;
    }
}