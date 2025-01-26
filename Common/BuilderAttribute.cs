using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceGenerator.Sugar.Common;

[StructLayout(LayoutKind.Auto)]
public struct BuilderAttribute
{
    public string Namespace;
    public string AttributeName;
    public string? Args;

    public static BuilderAttribute StructLayout(LayoutKind kind)
    {
        return new BuilderAttribute
        {
            Namespace = typeof(StructLayoutAttribute).Namespace!,
            AttributeName = nameof(StructLayout),
            Args = $"{nameof(LayoutKind)}.{kind}"
        };
    }
    
    public static BuilderAttribute MethodImpl(MethodImplOptions options)
    {
        return new BuilderAttribute
        {
            Namespace = typeof(MethodImplAttribute).Namespace!,
            AttributeName = nameof(MethodImpl),
            Args = $"{nameof(MethodImplOptions)}.{options}"
        };
    }   
}