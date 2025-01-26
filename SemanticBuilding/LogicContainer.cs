using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.SemanticBuilding;

public struct LogicContainer : ISemanticStructBuilder
{
    public string Modifier;
    public string Name;
    public MethodContainerType Type;
    
    public List<FieldBuilder> Fields; 
    public List<MethodStructBuilder> Methods;

    public Guid ContextId { get; set; }

    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        indentLevel++;

        builder.AddAttributes(this, indentLevel);
        
        builder.Indent(indentLevel)
            .AppendSpaceEnd(Modifier)
            .AppendSpaceEnd(GetTypeName())
            .Append(Name)
            .Push();
        
        builder.Indent(indentLevel)
            .Append('{')
            .Push();

        foreach (var field in Fields)
            field.Build(builder, ref indentLevel);
            
        foreach (var methodBuilder in Methods)
            methodBuilder.Build(builder, ref indentLevel);
        
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