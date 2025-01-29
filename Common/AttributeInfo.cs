using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceGenerator.Sugar.Common;

[StructLayout(LayoutKind.Auto)]
public struct AttributeInfo
{
    public string Namespace;
    public string AttributeName;
    public string? Args;

    public static AttributeInfo Serializable()
    {
        return new AttributeInfo
        {
            Namespace = "System",
            AttributeName = nameof(Serializable),
        };
    } 
    
    public static AttributeInfo StructLayout(LayoutKind kind)
    {
        return new AttributeInfo
        {
            Namespace = typeof(StructLayoutAttribute).Namespace!,
            AttributeName = nameof(StructLayout),
            Args = $"{nameof(LayoutKind)}.{kind}"
        };
    }
    
    public static AttributeInfo MethodImpl(MethodImplOptions options)
    {
        return new AttributeInfo
        {
            Namespace = typeof(MethodImplAttribute).Namespace!,
            AttributeName = nameof(MethodImpl),
            Args = $"{nameof(MethodImplOptions)}.{options}"
        };
    }   
}