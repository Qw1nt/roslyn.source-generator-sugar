using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct MethodStructBuilder : ISemanticStructBuilder
{
    public string Modifier;
    public string Type;
    public string Name;
    public string Args;
    public string Body;

    public Guid ContextId { get; set; }

    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;

        builder.Indent(indentLevel)
            .AppendSpaceEnd(Modifier)
            .AppendSpaceEnd(Type, string.IsNullOrEmpty(Type))
            .Append(Name)
            .Append('(').Append(Args).Append(')')
            .Push();

        builder.Indent(indentLevel)
            .Append('{')
            .Push();

        indentLevel++;

        builder.Indent(0)
            .AppendBlock(Body, indentLevel);

        indentLevel--;

        builder.Indent(indentLevel)
            .Append('}')
            .Push();

        indentLevel--;
    }
    
    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }
}