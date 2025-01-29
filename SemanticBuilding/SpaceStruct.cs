using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct SpaceStruct
{
    public static void Build(SemanticBuildingContext builder)
    {
        builder.Indent(0)
            .Push();
    }
}