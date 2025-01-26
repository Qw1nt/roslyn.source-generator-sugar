using System.Text;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.Common;

public class SemanticBuildingContext
{
    public readonly StringBuilder Builder;
    private readonly IReadOnlyDictionary<Guid, List<BuilderAttribute>> _attributes;

    public SemanticBuildingContext(StringBuilder builder, IReadOnlyDictionary<Guid, List<BuilderAttribute>> attributes)
    {
        Builder = builder;
        _attributes = attributes;
    }

    public IReadOnlyList<BuilderAttribute>? GetAttributes(Guid contextId)
    {
        if (_attributes.ContainsKey(contextId) == false)
            return null;

        return _attributes[contextId];
    }

    public void AddAttributes<T>(T semanticBuilder, in int indentLevel)
        where T : ISemanticStructBuilder
    {
        var attributes = GetAttributes(semanticBuilder.ContextId);

        if (attributes == null)
            return;

        var info = Builder.Indent(indentLevel);

        for (int i = 0; i < attributes.Count; i++)
        {
            var attribute = attributes[i];

            info.Append('[');
            info.Append(attribute.AttributeName);

            if (string.IsNullOrEmpty(attribute.Args) == false)
            {
                info.Append('(');
                info.Append(attribute.Args!);
                info.Append(')');
            }

            info.Append(']');

            if (i < attributes.Count - 1)
                info.NewLine();
        }

        info.Push();
    }

    public static implicit operator StringBuilder(SemanticBuildingContext context)
    {
        return context.Builder;
    }
}