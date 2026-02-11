using System;
using System.Runtime.Serialization;

namespace BiphaseDecoder;

public class LoadingBinder : SerializationBinder
{
    public override Type? BindToType(string assemblyName, string typeName)
    {
        if (typeName == "rshwFormat")
            return typeof(rshwFormat);
        return null;
    }
}
