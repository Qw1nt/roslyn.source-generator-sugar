using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct FieldBuilder : ISemanticStructBuilder
{
    public string Modifier;
    public string ValueType;
    public string Name;
    public string? Value;
    
    public FieldBuilderType Type;

    private FieldAccessor? _get;
    private FieldAccessor? _set;

    public Guid ContextId { get; set; }

    public FieldAccessor? Get
    {
        get => _get;
        set
        {
            if (value != null && value.Value.Type == AccessorType.Set)
                throw new ArgumentException("Get accessor must be of type \"Get\"");

            _get = value;
        }
    }

    public FieldAccessor? Set
    {
        get => _set;
        set
        {
            if (value != null && value.Value.Type == AccessorType.Get)
                throw new ArgumentException("Set accessor must be of type \"Set\"");

            _set = value;
        }
    }
    
    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;

        if(Type == FieldBuilderType.Const)
        {
            if (string.IsNullOrEmpty(Value) == true)
                throw new ArgumentException("Constant must have a value");
            
            builder.Indent(indentLevel)
                .AppendSpaceEnd(Modifier)
                .AppendSpaceEnd("const")
                .AppendSpaceEnd(ValueType)
                .AppendSpaceEnd(Name)
                .AppendSpaceEnd('=')
                .Append(Value!)
                .Append(';')
                .Push();
        }
        else if (Type == FieldBuilderType.Field)
        {
            builder
                .Indent(indentLevel)
                .AppendSpaceEnd(Modifier)
                .AppendSpaceEnd(ValueType)
                .Append(Name)
                .Append(';')
                .Push();
        }
        else if (Type == FieldBuilderType.Property)
        {
            builder
                .Indent(indentLevel)
                .AppendSpaceEnd(Modifier)
                .AppendSpaceEnd(ValueType)
                .AppendSpaceEnd(Name)
                .NewLine()
                .Append('{')
                .Push();

            Get ??= FieldAccessor.Get();
            Set ??= FieldAccessor.Set();

            Get.Value.Build(builder, ref indentLevel);
            Set.Value.Build(builder, ref indentLevel);

            builder.Indent(indentLevel)
                .Append('}')
                .Push();
        }

        indentLevel--;
    }

    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }
}