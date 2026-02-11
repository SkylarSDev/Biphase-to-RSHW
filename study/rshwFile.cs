using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Token: 0x0200004D RID: 77
[Serializable]
public class rshwFile
{
	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000E12C File Offset: 0x0000C32C
	// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000E134 File Offset: 0x0000C334
	public byte[] audioData { get; set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000E13D File Offset: 0x0000C33D
	// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000E145 File Offset: 0x0000C345
	public int[] signalData { get; set; }

	// Token: 0x060001A8 RID: 424 RVA: 0x0000E150 File Offset: 0x0000C350
	public void Save(string filePath)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		using (FileStream fileStream = File.Open(filePath, FileMode.Create))
		{
			binaryFormatter.Serialize(fileStream, this);
		}
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000E190 File Offset: 0x0000C390
	public static rshwFile ReadFromFile(string filepath)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		rshwFile result;
		using (FileStream fileStream = File.OpenRead(filepath))
		{
			result = (rshwFile)binaryFormatter.Deserialize(fileStream);
		}
		return result;
	}
}
