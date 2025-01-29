using SourceGenerator.Sugar.Common;

namespace SourceGenerator.Sugar.Interfaces;

public interface ISemanticStructBuilder
{
    public Guid ContextId { get; set; }

    public IReadOnlyList<AttributeInfo> GetAttributes();

    public IReadOnlyList<ISemanticStructBuilder>? GetChild();
    
    public int GetSpacesCount();
    
    void AddAttribute(AttributeInfo attributeInfoData);

    void AddSpace(int count = 1);
    
    void AddChild(ISemanticStructBuilder builder);
    
    void Build(SemanticBuildingContext builder, ref int indentLevel);
}