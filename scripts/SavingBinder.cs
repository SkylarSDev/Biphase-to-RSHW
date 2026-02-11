using System;
using System.Runtime.Serialization;

namespace BiphaseDecoder;

public class SavingBinder : SerializationBinder
{
    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        if (serializedType == typeof(rshwFormat))
        {
            assemblyName = "Assembly-CSharp";
            typeName = "rshwFormat";
        }
        else
        {
            assemblyName = null;
            typeName = null;
        }
    }

    public override Type? BindToType(string assemblyName, string typeName)
    {
        if (typeName == "rshwFormat")
            return typeof(rshwFormat);
        return null;
    }
}
