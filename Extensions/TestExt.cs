using Microsoft.CodeAnalysis;
using SourceGenerator.Sugar.Interfaces;
using SourceGenerator.Sugar.SemanticBuilding;

namespace SourceGenerator.Sugar.Extensions;

public static class TestExt
{
    public static LogicContainer Class()
    {
        return new LogicContainer();
    }    
    
    public static T Public<T>(this T item)
        where T : IAccessibility
    {
        item.Accessibility = Accessibility.Public;
        return item;
    }
}