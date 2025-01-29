using Microsoft.CodeAnalysis;

namespace SourceGenerator.Sugar.Interfaces;

public interface IAccessibility
{
    public Accessibility Accessibility { get; set; }
}