using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct SpaceStruct : ISemanticStructBuilder
{
    public Guid ContextId { get; set; }
   
    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        builder.Indent(0)
            .Push();
    }

    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }
}