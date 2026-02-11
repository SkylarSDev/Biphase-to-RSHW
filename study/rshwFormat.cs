using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Token: 0x0200004E RID: 78
[Serializable]
public class rshwFormat
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060001AB RID: 427 RVA: 0x0000E1DC File Offset: 0x0000C3DC
	// (set) Token: 0x060001AC RID: 428 RVA: 0x0000E1E4 File Offset: 0x0000C3E4
	public byte[] audioData { get; set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060001AD RID: 429 RVA: 0x0000E1ED File Offset: 0x0000C3ED
	// (set) Token: 0x060001AE RID: 430 RVA: 0x0000E1F5 File Offset: 0x0000C3F5
	public int[] signalData { get; set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060001AF RID: 431 RVA: 0x0000E1FE File Offset: 0x0000C3FE
	// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000E206 File Offset: 0x0000C406
	public byte[] videoData { get; set; }

	// Token: 0x060001B1 RID: 433 RVA: 0x0000E210 File Offset: 0x0000C410
	public void Save(string filePath)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		using (FileStream fileStream = File.Open(filePath, FileMode.Create))
		{
			binaryFormatter.Serialize(fileStream, this);
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000E250 File Offset: 0x0000C450
	public static rshwFormat ReadFromFile(string filepath)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		rshwFormat result;
		using (FileStream fileStream = File.OpenRead(filepath))
		{
			if (fileStream.Length != 0L)
			{
				fileStream.Position = 0L;
				try
				{
					return (rshwFormat)binaryFormatter.Deserialize(fileStream);
				}
				catch (Exception)
				{
					return null;
				}
			}
			result = null;
		}
		return result;
	}
}
