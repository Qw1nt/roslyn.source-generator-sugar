using SourceGenerator.Sugar.Base;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;

namespace SourceGenerator.Sugar.SemanticBuilding;

public class NamespaceBuilder : SemanticStructBuilderBase
{
    public string Namespace;
    public int UsingsCount;
    
    protected override void BuildStruct(SemanticBuildingContext builder, ref int indentLevel)
    {
        builder
            .Indent(indentLevel)
            .Append(UsingsCount == 0 ? string.Empty : "\n")
            .Append("namespace ")
            .Append(Namespace)
            .Push();

        builder.Indent(indentLevel)
            .Append('{')
            .Push();

        BuildChild(builder, ref indentLevel);
        
        builder.Indent(indentLevel)
            .Append('}')
            .Push();
    }
}