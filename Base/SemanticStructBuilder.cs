using SourceGenerator.Sugar.Common;
using SourceGenerator.Sugar.Extensions;
using SourceGenerator.Sugar.Interfaces;

namespace SourceGenerator.Sugar.Base;

public abstract class SemanticStructBuilderBase : ISemanticStructBuilder
{
    private static readonly List<AttributeInfo> EmptyAttributes = new(Array.Empty<AttributeInfo>());

    private List<AttributeInfo>? _attributes = null!;
    private List<ISemanticStructBuilder>? _children = null!;

    private int _spacesCount = 0;

    public Guid ContextId { get; set; }

    public IReadOnlyList<AttributeInfo> GetAttributes()
    {
        if (_attributes == null)
            return EmptyAttributes;

        return _attributes;
    }

    public IReadOnlyList<ISemanticStructBuilder>? GetChild()
    {
        return _children;
    }

    public int GetSpacesCount()
    {
        return _spacesCount;
    }

    public void AddAttribute(AttributeInfo attributeInfoData)
    {
        if (_attributes == null)
            _attributes = new List<AttributeInfo>();

        _attributes.Add(attributeInfoData);
    }

    public void AddSpace(int count = 1)
    {
        _spacesCount += count;
    }

    public void AddChild(ISemanticStructBuilder builder)
    {
        if (_children == null)
            _children = new List<ISemanticStructBuilder>();

        _children.Add(builder);
    }

    protected abstract void BuildStruct(SemanticBuildingContext builder, ref int indentLevel);

    public void Build(SemanticBuildingContext builder, ref int indentLevel)
    {
        BuildSpaces(builder, ref indentLevel);
        BuildStruct(builder, ref indentLevel);
    }

    private void BuildSpaces(SemanticBuildingContext buildingContext, ref int indentLevel)
    {
        for (int i = 0; i < _spacesCount; i++)
        {
            buildingContext
                .Indent(0)
                .Push();
        }
    }

    protected void BuildChild(SemanticBuildingContext builder, ref int indentLevel)
    {
        if (_children == null)
            return;

        foreach (var item in _children)
            item.Build(builder, ref indentLevel);
    }

    protected void BuildAttribute(SemanticBuildingContext builder, ref int indentLevel)
    {
        if (_attributes == null)
            return;

        var info = builder.Indent(indentLevel);

        for (int i = 0; i < _attributes.Count; i++)
        {
            var attribute = _attributes[i];

            info.Append('[');
            info.Append(attribute.AttributeName);

            if (string.IsNullOrEmpty(attribute.Args) == false)
            {
                info.Append('(');
                info.Append(attribute.Args!);
                info.Append(')');
            }

            info.Append(']');

            if (i < _attributes.Count - 1)
                info.NewLine();
        }

        info.Push();
    }
}