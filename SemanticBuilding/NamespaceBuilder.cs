using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct NamespaceBuilder : ISemanticStructBuilder
{
    public string Namespace;
    public int UsingsCount;
    
    public List<LogicContainer> Classes;

    public Guid ContextId { get; set; }

    public void Build(SemanticBuildingContext builder, ref int indentLevel)
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
        
        foreach (var classBuilder in Classes)
            classBuilder.Build(builder, ref indentLevel);
        
        builder.Indent(indentLevel)
            .Append('}')
            .Push();
    }
    
    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }
}