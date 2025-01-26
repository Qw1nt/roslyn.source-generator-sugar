using Microsoft.CodeAnalysis;

namespace SourceGenerator.Sugar.Extensions;

public static class NamedTypeSymbolExtensions
{
    /// <summary>
    /// Получить полное имя типа: Пространство имён + имя
    /// </summary>
    /// <param name="typeSymbol"></param>
    /// <returns></returns>
    public static string GetFullName(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.ContainingNamespace + "." + typeSymbol.Name;
    }

    /// <summary>
    /// Получить глобальное имя: global::Пространство имён + имя
    /// </summary>
    /// <param name="typeSymbol"></param>
    /// <returns></returns>
    public static string GetGlobal(this ITypeSymbol typeSymbol)
    {
        return "global::" + typeSymbol.ContainingNamespace + "." + typeSymbol.Name;
    }

    /// <summary>
    /// Получить все поля
    /// </summary>
    /// <param name="typeSymbol"></param>
    /// <returns></returns>
    public static IEnumerable<IFieldSymbol> GetAllFields(this INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.GetMembers()
            .OfType<IFieldSymbol>();
    }
}