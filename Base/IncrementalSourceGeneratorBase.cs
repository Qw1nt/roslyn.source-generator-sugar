using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Interfaces;
using SourceGenerator.Sugar.SemanticBuilding;

namespace SourceGenerator.Sugar.Base;

public abstract class IncrementalSourceGeneratorBase : IIncrementalGenerator
{
    protected readonly StringBuilder Builder = new();

    private readonly List<LogicContainer> _classBuilders = new();
    private readonly List<FieldBuilder> _fieldBuilders = new();
    private readonly List<MethodStructBuilder> _methodBuilders = new();
    private readonly HashSet<string> _usings = new();

    private readonly Dictionary<Guid, List<BuilderAttribute>> _attributes = new();
    private readonly List<BuilderAttribute> _attributesStore = new();

    private NamespaceBuilder _namespaceBuilder;
    private LogicContainer _currentClassBuilder;

    private Guid _currentContextId;

    private Guid CurrentContextId
    {
        get => _currentContextId;
        set
        {
            _currentContextId = value;
            TryAddAttributesToContext();
        }
    }

    protected void Using(string usingLine)
    {
        _usings.Add(usingLine);
    }

    protected void Using(ICollection<string> usings)
    {
        foreach (var @using in usings)
            Using(@using);
    }

    protected void Attribute(BuilderAttribute source)
    {
        _attributesStore.Add(source);
    }

    protected void Attribute<T>(T builder, BuilderAttribute source)
        where T : ISemanticStructBuilder
    {
        var contextId = builder.ContextId;

        if (_attributes.ContainsKey(contextId) == false)
            _attributes.Add(contextId, new List<BuilderAttribute>());

        _attributes[contextId].Add(source);
    }

    private void TryAddAttributesToContext()
    {
        if (CurrentContextId == Guid.Empty || _attributesStore.Count == 0)
            return;

        if (_attributes.ContainsKey(CurrentContextId) == false)
            _attributes.Add(CurrentContextId, new List<BuilderAttribute>(_attributesStore));
        else
            _attributes[CurrentContextId].AddRange(_attributesStore);

        _attributesStore.Clear();
    }

    protected void Space()
    {
    }

    protected void Space(Guid contextId)
    {
    }

    protected void Namespace(ITypeSymbol type, Action<NamespaceBuilder> context)
    {
        Namespace(type.ContainingNamespace.ToDisplayString(), context);
    }

    protected void Namespace(INamespaceSymbol namespaceSymbol, Action<NamespaceBuilder> context)
    {
        Namespace(namespaceSymbol.ToDisplayString(), context);
    }

    protected void Namespace(string namespaceName, Action<NamespaceBuilder> context)
    {
        _classBuilders.Clear();
        _fieldBuilders.Clear();
        _methodBuilders.Clear();

        _namespaceBuilder = new NamespaceBuilder
        {
            Namespace = namespaceName,
            Classes = _classBuilders,
            ContextId = Guid.NewGuid()
        };

        CurrentContextId = _namespaceBuilder.ContextId;
        context.Invoke(_namespaceBuilder);
    }

    protected void Class(string modifier, string name, Action<LogicContainer> context)
    {
        Container(modifier, name, MethodContainerType.Class, context);
    }

    protected void Struct(string modifier, string name, Action<LogicContainer> context)
    {
        Container(modifier, name, MethodContainerType.Struct, context);
    }

    protected void Container(string modifier, string name, MethodContainerType type, Action<LogicContainer> context)
    {
        var classBuilder = new LogicContainer
        {
            Modifier = modifier,
            Name = name,
            Type = type,
            Fields = _fieldBuilders,
            Methods = _methodBuilders,
            ContextId = Guid.NewGuid()
        };

        _currentClassBuilder = classBuilder;
        CurrentContextId = classBuilder.ContextId;

        context.Invoke(_currentClassBuilder);
        _classBuilders.Add(_currentClassBuilder);
    }

    protected void Const(Accessibility accessibility, string type, string name, string value)
    {
        CreateDataField(accessibility.ToString().ToLower(),
            type,
            name,
            null!,
            FieldBuilderType.Const,
            value: value
        );
    }

    protected void Field(Accessibility accessibility, string type, string name,
        Func<FieldBuilder, FieldBuilder>? builder = null)
    {
        CreateDataField(accessibility.ToString().ToLower(), type, name, builder!, FieldBuilderType.Field);
    }

    protected void Field(string modifier, string type, string name, Func<FieldBuilder, FieldBuilder>? builder = null)
    {
        CreateDataField(modifier, type, name, null!, FieldBuilderType.Field);
    }

    protected void Property(Accessibility accessibility, string type, string name,
        Func<FieldBuilder, FieldBuilder> builder)
    {
        CreateDataField(accessibility.ToString().ToLower(), type, name, builder, FieldBuilderType.Property);
    }

    protected void Property(string accessibility, string type, string name, Func<FieldBuilder, FieldBuilder> builder)
    {
        CreateDataField(accessibility, type, name, builder, FieldBuilderType.Property);
    }

    private void CreateDataField(string modifier, string type, string name, Func<FieldBuilder, FieldBuilder> builder, FieldBuilderType builderType, string? value = null)
    {
        var field = new FieldBuilder
        {
            Modifier = modifier,
            ValueType = type,
            Type = builderType,
            Name = name,
            Value = value,
            ContextId = Guid.NewGuid()
        };

        CurrentContextId = field.ContextId;

        if (builder != null!)
            field = builder.Invoke(field);

        _fieldBuilders.Add(field);
    }

    protected void Constructor(string modifier, ITypeSymbol type, Func<string> body)
    {
        Method(modifier, string.Empty, type.Name, body);
    }

    protected void Method(string modifier, string type, string name, Func<string> body)
    {
        Method(modifier, type, name, string.Empty, body);
    }

    protected void Method(string modifier, string type, string name, string args, Func<string> body)
    {
        var methodBuilder = new MethodStructBuilder
        {
            Modifier = modifier,
            Type = type,
            Name = name,
            Args = args,
            Body = body.Invoke(),
            ContextId = Guid.NewGuid()
        };

        CurrentContextId = methodBuilder.ContextId;
        _methodBuilders.Add(methodBuilder);
    }

    private int BuildUsings()
    {
        foreach (var keyValue in _attributes)
        {
            var usings = keyValue.Value.Select(x => x.Namespace);

            foreach (var usingLine in usings)
                _usings.Add(usingLine);
        }

        var count = _usings.Count;

        foreach (var usingLine in _usings)
        {
            Builder.Append("using ");
            Builder.Append(usingLine);
            Builder.Append(';');
            Builder.Append('\n');
        }

        _usings.Clear();
        return count;
    }

    protected void Build()
    {
        var indent = 0;

        _namespaceBuilder.UsingsCount = BuildUsings();
        _namespaceBuilder.Build(new SemanticBuildingContext(Builder, _attributes), ref indent);
    }

    protected void AddSource(SourceProductionContext context, string fileName)
    {
        Build();
        context.AddSource($"{fileName}.g.cs", SourceText.From(Builder.ToString(), Encoding.UTF8));
    }

    public abstract void Initialize(IncrementalGeneratorInitializationContext context);
}