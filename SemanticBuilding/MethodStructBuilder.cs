using SourceGenerator.Sugar.Base;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;

namespace SourceGenerator.Sugar.SemanticBuilding;

public class MethodStructBuilder : SemanticStructBuilderBase
{
    public string Modifier;
    public string Type;
    public string Name;
    public string Args;
    public string Body;
    
    protected override void BuildStruct(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;

        BuildAttribute(builder, ref indentLevel);

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