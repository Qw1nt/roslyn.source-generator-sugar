using Microsoft.CodeAnalysis;
using SourceGenerator.Sugar.Base;
using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public class LogicContainer : SemanticStructBuilderBase, IAccessibility
{
    public string Modifier;
    public string Name;
    public MethodContainerType Type;

    public Accessibility Accessibility { get; set; }

    protected override void BuildStruct(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;
        
        BuildAttribute(builder, ref indentLevel);

        builder.Indent(indentLevel)
            .AppendSpaceEnd(Modifier)
            .AppendSpaceEnd(GetTypeName())
            .Append(Name)
            .Push();

        builder.Indent(indentLevel)
            .Append('{')
            .Push();

        BuildChild(builder, ref indentLevel);

        builder.Indent(indentLevel)
            .Append('}')
            .Push();

        indentLevel--;
    }

    private string GetTypeName()
    {
        switch (Type)
        {
            case MethodContainerType.Class:
                return "class";

            case MethodContainerType.Struct:
                return "struct";

            default:
                throw new ArgumentOutOfRangeException();
        }

        return string.Empty;
    }

    public override int GetHashCode()
    {
        return ContextId.GetHashCode();
    }
}