using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using SourceGenerator.Sugar.Base;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;

namespace SourceGenerator.Sugar.SemanticBuilding;

public class FieldAccessor : SemanticStructBuilderBase
{
    public AccessorType Type;
    public Accessibility? Accessibility;
    public string? Content;

    protected override void BuildStruct(SemanticBuildingContext builder, ref int indentLevel)
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
            var info = builder.Indent(indentLevel);

            if (Accessibility != null)
            {
                info.Append(Accessibility.ToString().ToLower())
                    .Append(' ');
            }

            info.Append(accessorName)
                .Append(';')
                .Push();
        }
        else
        {
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
    public static FieldAccessor Get(Action<FieldAccessor>? setup = null)
    {
        return Get(null, setup);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Get(Func<string>? contentBuilder, Action<FieldAccessor>? setup = null)
    {
        var accessor = new FieldAccessor
        {
            Type = AccessorType.Get,
            Content = contentBuilder?.Invoke(),
            ContextId = Guid.NewGuid()
        };

        setup?.Invoke(accessor);
        return accessor;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Set(Action<FieldAccessor>? setup = null)
    {
        return Set(null, setup);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FieldAccessor Set(Func<string>? contentBuilder, Action<FieldAccessor>? setup = null)
    {
        var accessor = new FieldAccessor
        {
            Type = AccessorType.Set,
            Content = contentBuilder?.Invoke(),
            ContextId = Guid.NewGuid()
        };

        setup?.Invoke(accessor);
        return accessor;
    }
}