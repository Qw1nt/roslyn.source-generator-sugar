using System.Text;

namespace SourceGenerator.Sugar.Common;

public class SemanticBuildingContext
{
    public readonly StringBuilder Builder;

    public SemanticBuildingContext(StringBuilder builder)
    {
        Builder = builder;
    }
    
    /*public void AddAttributes<T>(T semanticBuilder, in int indentLevel)
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
    }*/

    public static implicit operator StringBuilder(SemanticBuildingContext context)
    {
        return context.Builder;
    }
}