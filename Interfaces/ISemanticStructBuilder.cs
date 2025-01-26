using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Interfaces;

public interface ISemanticStructBuilder
{
    public Guid ContextId { get; set; }
    
    void Build(SemanticBuildingContext builder, ref int indentLevel);
}