using System.Runtime.CompilerServices;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct FieldAccessor : ISemanticStructBuilder
{
    public AccessorType Type;
    public string? Content;

    public Guid ContextId { get; set; }

    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;

        var accessorName = Type switch
        {
            AccessorType.Get => "get",
            AccessorType.Set => "set",
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(Content) == true)
        {
            builder.Indent(indentLevel)
                .Append(accessorName)
                .Append(';')
                .Push();
        }
        else
        {
            builder.AddAttributes(this, indentLevel);
            
            builder.Indent(indentLevel)
                .Append(accessorName)
                .Push();

            builder.Indent(indentLevel)
                .Append('{')
                .Push();

            indentLevel++;

            builder.Indent(0)
                .AppendBlock(Content, indentLevel);

            indentLevel--;

            builder.Indent(indentLevel)
                .Append('}')
                .Push();
        }

        indentLevel--;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Get(Func<FieldAccessor, FieldAccessor>? setup = null)
    {
        return Get(null, setup);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Get(Func<string>? contentBuilder, Func<FieldAccessor, FieldAccessor>? setup = null)
    {
        var accessor = new FieldAccessor
        {
            Type = AccessorType.Get,
            Content = contentBuilder?.Invoke(),
            ContextId = Guid.NewGuid()
        };

        if (setup != null)
            accessor = setup.Invoke(accessor);

        return accessor;
    }

    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Set(Func<FieldAccessor, FieldAccessor>? setup = null)
    {
        return Set(null, setup);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Set(Func<string>? contentBuilder, Func<FieldAccessor, FieldAccessor>? setup = null)
    {
        var accessor = new FieldAccessor
        {
            Type = AccessorType.Set,
            Content = contentBuilder?.Invoke(),
            ContextId = Guid.NewGuid()
        };

        if (setup != null)
            accessor = setup.Invoke(accessor);

        return accessor;
    }
}