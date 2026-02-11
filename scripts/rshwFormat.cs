using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BiphaseDecoder;

[System.Serializable]
public class rshwFormat
{
    public byte[]? audioData { get; set; }
    public int[]? signalData { get; set; }
    public byte[]? videoData { get; set; }

    public static rshwFormat Load(string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new LoadingBinder();
        FileStream stream = File.OpenRead(path);
        var result = (rshwFormat)formatter.Deserialize(stream);
        stream.Close();
        return result;
    }

    public static void Save(string path, rshwFormat file)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new SavingBinder();
        FileStream stream = File.Open(path, FileMode.Create);
        formatter.Serialize(stream, file);
        stream.Close();
    }
}
