using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFB;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

// Token: 0x02000084 RID: 132
public class UI_ShowtapeManager : MonoBehaviour
{
	// Token: 0x060002D3 RID: 723 RVA: 0x0001C643 File Offset: 0x0001A843
	private void Start()
	{
		this.creator = base.GetComponent<UI_RshwCreator>();
		this.referenceVideo = base.GetComponent<VideoPlayer>();
		this.refSpeakerVol = this.referenceSpeaker.volume;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0001C670 File Offset: 0x0001A870
	private void Update()
	{
		this.syncTimer += Time.deltaTime;
		if (this.syncTimer >= 30f)
		{
			this.syncTimer = 0f;
			this.syncTvsAndSpeakers.Invoke();
		}
		if (this.inputHandler != null)
		{
			InputDataObj inputDataObj = this.inputHandler.InputCheck();
			this.mack.topDrawer = inputDataObj.topDrawer;
			this.mack.bottomDrawer = inputDataObj.bottomDrawer;
			if (this.inputHandler != null && this.mack != null && (this.useVideoAsReference || (!this.useVideoAsReference && this.referenceSpeaker.clip != null)) && this.rshwData != null)
			{
				int num;
				if (this.useVideoAsReference)
				{
					num = (int)(this.referenceVideo.time * (double)this.dataStreamedFPS);
				}
				else
				{
					num = (int)(this.referenceSpeaker.time * this.dataStreamedFPS);
				}
				if (num >= this.rshwData.Length && this.rshwData.Length != 0)
				{
					if (this.recordMovements)
					{
						while (num + 1 > this.rshwData.Length)
						{
							this.rshwData = this.rshwData.Append(new BitArray(300)).ToArray<BitArray>();
						}
					}
					else
					{
						num = this.rshwData.Length;
					}
				}
				if (this.recordMovements)
				{
					if (inputDataObj.anyButtonHeld)
					{
						for (int i = 0; i < 150; i++)
						{
							if (inputDataObj.topDrawer[i])
							{
								this.rshwData[num].Set(i, true);
							}
							if (inputDataObj.bottomDrawer[i])
							{
								this.rshwData[num].Set(i + 150, true);
							}
						}
						if (this.previousAnyButtonHeld)
						{
							if (this.previousFramePosition <= num)
							{
								for (int j = 0; j < num - this.previousFramePosition; j++)
								{
									for (int k = 0; k < 150; k++)
									{
										if (inputDataObj.topDrawer[k])
										{
											this.rshwData[this.previousFramePosition + j].Set(k, true);
										}
										if (inputDataObj.bottomDrawer[k])
										{
											this.rshwData[this.previousFramePosition + j].Set(k + 150, true);
										}
									}
								}
							}
							else
							{
								for (int l = 0; l < this.previousFramePosition - num; l++)
								{
									for (int m = 0; m < 150; m++)
									{
										if (inputDataObj.topDrawer[m])
										{
											this.rshwData[this.previousFramePosition - l].Set(m, true);
										}
										if (inputDataObj.bottomDrawer[m])
										{
											this.rshwData[this.previousFramePosition - l].Set(m + 150, true);
										}
									}
								}
							}
						}
						this.newDataRecorded.Invoke();
					}
					if (Input.anyKey)
					{
						this.ticketCheck = true;
					}
					if (!this.useVideoAsReference)
					{
						if (this.referenceSpeaker.time % 5f < 1f && !this.ticketCheck2)
						{
							this.ticketCheck2 = true;
							if (Input.anyKey)
							{
								this.ticketCheck = false;
							}
							if (this.ticketCheck)
							{
								PlayerPrefs.SetInt("TicketCount", PlayerPrefs.GetInt("TicketCount") + 1);
								this.updateTickets.Invoke();
							}
							this.ticketCheck = false;
						}
						if (this.referenceSpeaker.time % 5f >= 1f)
						{
							this.ticketCheck2 = false;
						}
					}
				}
				if (num < this.rshwData.Length)
				{
					for (int n = 0; n < 150; n++)
					{
						if (this.rshwData[num].Get(n))
						{
							this.mack.topDrawer[n] = true;
						}
						if (this.rshwData[num].Get(n + 150))
						{
							this.mack.bottomDrawer[n] = true;
						}
					}
				}
				if (((!this.useVideoAsReference && this.referenceSpeaker.time >= this.speakerClip.length) || (this.useVideoAsReference && this.referenceVideo.time >= this.referenceVideo.length * (double)this.referenceVideo.GetAudioChannelCount(0))) && !this.recordMovements)
				{
					Debug.Log("Song is over. Queuing next song / stopping.");
					this.Play(true, false);
					if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopSong)
					{
						if (this.currentShowtapeSegment == -1)
						{
							this.referenceSpeaker.time = 0f;
							this.referenceVideo.time = 0.0;
						}
						else
						{
							this.creator.LoadFromURL(this.showtapeSegmentPaths[this.currentShowtapeSegment]);
						}
					}
					else if (this.currentShowtapeSegment == -1)
					{
						this.referenceSpeaker.time = 0f;
						this.referenceVideo.time = 0.0;
						this.Unload();
					}
					else
					{
						this.currentShowtapeSegment++;
						if (this.currentShowtapeSegment >= this.showtapeSegmentPaths.Length)
						{
							if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopPlaylist)
							{
								this.currentShowtapeSegment = 0;
								this.creator.LoadFromURL(this.showtapeSegmentPaths[this.currentShowtapeSegment]);
							}
							else
							{
								this.Unload();
							}
						}
						else
						{
							this.creator.LoadFromURL(this.showtapeSegmentPaths[this.currentShowtapeSegment]);
						}
					}
				}
				this.previousFramePosition = num;
				this.previousAnyButtonHeld = inputDataObj.anyButtonHeld;
			}
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0001CBB4 File Offset: 0x0001ADB4
	public void Load()
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		Debug.Log("Load");
		if (this.referenceSpeaker != null)
		{
			this.referenceSpeaker.time = 0f;
		}
		if (this.referenceVideo != null)
		{
			this.referenceVideo.time = 0.0;
		}
		this.showtapeSegmentPaths = new string[1];
		string[] array;
		if (this.fileExtention == "")
		{
			ExtensionFilter[] extensions = new ExtensionFilter[]
			{
				new ExtensionFilter("Show Files", new string[]
				{
					"cshw",
					"sshw",
					"rshw",
					"nshw"
				})
			};
			array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape Audio", "", extensions, false);
		}
		else
		{
			array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape Audio", "", this.fileExtention, false);
		}
		if (array.Length != 0)
		{
			this.showtapeSegmentPaths[0] = array[0];
			this.currentShowtapeSegment = 0;
			this.creator.LoadFromURL(array[0]);
		}
		Cursor.lockState = lockState;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
	public void LoadFolder()
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		this.referenceSpeaker.time = 0f;
		this.referenceVideo.time = 0.0;
		string[] array = StandaloneFileBrowser.OpenFolderPanel("Select Folder of Showtapes", "", false);
		if (array.Length != 0)
		{
			this.showtapeSegmentPaths = Directory.GetFiles(array[0], "*." + this.fileExtention);
			this.currentShowtapeSegment = 0;
			this.creator.LoadFromURL(this.showtapeSegmentPaths[this.currentShowtapeSegment]);
		}
		Cursor.lockState = lockState;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0001CD5C File Offset: 0x0001AF5C
	public void Play(bool force, bool onOff)
	{
		if (force)
		{
			this.playMovements = onOff;
		}
		else
		{
			this.playMovements = !this.playMovements;
		}
		this.syncTvsAndSpeakers.Invoke();
		if (this.playMovements)
		{
			this.timeSongOffset += Time.time - this.timePauseStart;
			this.timePauseStart = 0f;
			this.audioVideoPlay.Invoke();
			return;
		}
		this.timePauseStart = Time.time;
		this.audioVideoPause.Invoke();
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001CDDD File Offset: 0x0001AFDD
	public void SwapLoop()
	{
		if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopPlaylist)
		{
			this.songLoopSetting = UI_ShowtapeManager.LoopVers.loopSong;
			return;
		}
		if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopSong)
		{
			this.songLoopSetting = UI_ShowtapeManager.LoopVers.noLoop;
			return;
		}
		this.songLoopSetting = UI_ShowtapeManager.LoopVers.loopPlaylist;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0001CE08 File Offset: 0x0001B008
	public void SkipSong(int skip)
	{
		this.playMovements = false;
		this.referenceSpeaker.time = 0f;
		this.referenceVideo.time = 0.0;
		if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.noLoop || this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopPlaylist)
		{
			this.currentShowtapeSegment += skip;
		}
		if (this.currentShowtapeSegment < 0)
		{
			this.currentShowtapeSegment = 0;
		}
		else if (this.currentShowtapeSegment >= this.showtapeSegmentPaths.Length)
		{
			if (this.songLoopSetting == UI_ShowtapeManager.LoopVers.loopPlaylist)
			{
				this.currentShowtapeSegment = 0;
			}
			else
			{
				this.currentShowtapeSegment = this.showtapeSegmentPaths.Length - 1;
			}
		}
		this.creator.LoadFromURL(this.showtapeSegmentPaths[this.currentShowtapeSegment]);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0001CEBC File Offset: 0x0001B0BC
	public void Unload()
	{
		this.videoPath = "";
		this.playMovements = false;
		this.recordMovements = false;
		this.referenceSpeaker.time = 0f;
		this.referenceVideo.time = 0.0;
		this.currentShowtapeSegment = -1;
		this.showtapeSegmentPaths = new string[1];
		this.rshwData = new BitArray[0];
		this.audioVideoPause.Invoke();
		this.curtainClose.Invoke();
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001CF3C File Offset: 0x0001B13C
	public void DeleteMove(int bitDelete)
	{
		int num = bitDelete + 24 * base.GetComponent<UI_WindowMaker>().deletePage;
		Debug.Log("Deleting Move: " + num.ToString());
		this.showtapeSegmentPaths = new string[1];
		string[] array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape", "", this.fileExtention, false);
		if (array.Length != 0)
		{
			this.showtapeSegmentPaths[0] = array[0];
			this.currentShowtapeSegment = 0;
			this.playMovements = false;
			if (this.showtapeSegmentPaths[0] != "")
			{
				rshwFormat rshwFormat = rshwFormat.ReadFromFile(this.showtapeSegmentPaths[0]);
				this.speakerClip = OpenWavParser.ByteArrayToAudioClip(rshwFormat.audioData, "", false);
				List<BitArray> list = new List<BitArray>();
				int num2 = 0;
				if (rshwFormat.signalData[0] != 0)
				{
					num2 = 1;
					BitArray item = new BitArray(300);
					list.Add(item);
				}
				for (int i = 0; i < rshwFormat.signalData.Length; i++)
				{
					if (rshwFormat.signalData[i] == 0)
					{
						num2++;
						BitArray item2 = new BitArray(300);
						list.Add(item2);
					}
					else
					{
						list[num2 - 1].Set(rshwFormat.signalData[i] - 1, true);
					}
				}
				this.rshwData = list.ToArray();
				for (int j = 0; j < this.rshwData.Length; j++)
				{
					this.rshwData[j].Set(num - 1, false);
				}
				this.creator.SaveRecording();
			}
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0001D0B4 File Offset: 0x0001B2B4
	public void DeleteMoveNoSaving(int bitDelete, bool fill)
	{
		Debug.Log("Deleting Move (No Save): " + bitDelete.ToString());
		for (int i = 0; i < this.rshwData.Length; i++)
		{
			this.rshwData[i].Set(bitDelete - 1, fill);
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0001D0FC File Offset: 0x0001B2FC
	public void PadMove(int bitPad, int padding)
	{
		bitPad--;
		if (padding > 0)
		{
			int num = this.rshwData.Length;
			for (int i = 0; i < padding; i++)
			{
				this.rshwData = this.rshwData.Append(new BitArray(300)).ToArray<BitArray>();
			}
			for (int j = 0; j < num; j++)
			{
				this.rshwData[this.rshwData.Length - 1 - j].Set(bitPad, this.rshwData[num - 1 - j].Get(bitPad));
			}
			return;
		}
		padding = Mathf.Abs(padding);
		for (int k = 0; k < this.rshwData.Length - padding; k++)
		{
			this.rshwData[k].Set(bitPad, this.rshwData[k + padding].Get(bitPad));
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0001D1BC File Offset: 0x0001B3BC
	public void PadAllBits(int padding)
	{
		if (this.rshwData != null)
		{
			int i;
			if (this.useVideoAsReference)
			{
				i = (int)(this.referenceVideo.time * (double)this.dataStreamedFPS);
			}
			else
			{
				i = (int)(this.referenceSpeaker.time * this.dataStreamedFPS);
			}
			if (padding > 0)
			{
				for (int j = 0; j < padding; j++)
				{
					this.rshwData = this.rshwData.Append(new BitArray(300)).ToArray<BitArray>();
				}
				for (int k = this.rshwData.Length; k > i; k--)
				{
					if (k + padding < this.rshwData.Length)
					{
						this.rshwData[k + padding] = this.rshwData[k];
					}
				}
				return;
			}
			while (i < this.rshwData.Length)
			{
				if (i + padding > 0)
				{
					this.rshwData[i + padding] = this.rshwData[i];
				}
				i++;
			}
		}
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0001D291 File Offset: 0x0001B491
	public void RREngineFPS()
	{
		this.dataStreamedFPS = 60f;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0001D29E File Offset: 0x0001B49E
	public void SPTEFPS()
	{
		this.dataStreamedFPS = 30f;
	}

	// Token: 0x040003B7 RID: 951
	[Header("Inspector Objects")]
	public Mack_Valves mack;

	// Token: 0x040003B8 RID: 952
	public InputHandler inputHandler;

	// Token: 0x040003B9 RID: 953
	[Space(20f)]
	[Header("File Show Metadata")]
	[HideInInspector]
	public BitArray[] rshwData;

	// Token: 0x040003BA RID: 954
	[HideInInspector]
	public UI_RshwCreator creator;

	// Token: 0x040003BB RID: 955
	public AudioSource referenceSpeaker;

	// Token: 0x040003BC RID: 956
	[HideInInspector]
	public float refSpeakerVol;

	// Token: 0x040003BD RID: 957
	public VideoPlayer referenceVideo;

	// Token: 0x040003BE RID: 958
	public AudioClip speakerClip;

	// Token: 0x040003BF RID: 959
	public UI_ShowtapeManager.LoopVers songLoopSetting;

	// Token: 0x040003C0 RID: 960
	public string wavPath;

	// Token: 0x040003C1 RID: 961
	public string videoPath;

	// Token: 0x040003C2 RID: 962
	public string fileExtention = "rshw";

	// Token: 0x040003C3 RID: 963
	public string[] showtapeSegmentPaths = new string[1];

	// Token: 0x040003C4 RID: 964
	public int currentShowtapeSegment = -1;

	// Token: 0x040003C5 RID: 965
	public float dataStreamedFPS = 60f;

	// Token: 0x040003C6 RID: 966
	[Space(20f)]
	public UnityEvent audioVideoPlay;

	// Token: 0x040003C7 RID: 967
	public UnityEvent audioVideoPause;

	// Token: 0x040003C8 RID: 968
	public UnityEvent audioVideoGetData;

	// Token: 0x040003C9 RID: 969
	public UnityEvent newDataRecorded;

	// Token: 0x040003CA RID: 970
	public UnityEvent curtainClose;

	// Token: 0x040003CB RID: 971
	public UnityEvent curtainOpen;

	// Token: 0x040003CC RID: 972
	public UnityEvent syncTvsAndSpeakers;

	// Token: 0x040003CD RID: 973
	public UnityEvent updateTickets;

	// Token: 0x040003CE RID: 974
	private bool ticketCheck;

	// Token: 0x040003CF RID: 975
	private bool ticketCheck2;

	// Token: 0x040003D0 RID: 976
	[HideInInspector]
	public bool disableCharactersOnStart = true;

	// Token: 0x040003D1 RID: 977
	public bool recordMovements;

	// Token: 0x040003D2 RID: 978
	public bool playMovements;

	// Token: 0x040003D3 RID: 979
	public bool useVideoAsReference;

	// Token: 0x040003D4 RID: 980
	[HideInInspector]
	public float timeSongStarted;

	// Token: 0x040003D5 RID: 981
	[HideInInspector]
	public float timeSongOffset;

	// Token: 0x040003D6 RID: 982
	[HideInInspector]
	public float timePauseStart;

	// Token: 0x040003D7 RID: 983
	[HideInInspector]
	public float timeInputSpeedStart;

	// Token: 0x040003D8 RID: 984
	[HideInInspector]
	public int previousFramePosition;

	// Token: 0x040003D9 RID: 985
	[HideInInspector]
	public bool previousAnyButtonHeld;

	// Token: 0x040003DA RID: 986
	private float syncTimer;

	// Token: 0x02000116 RID: 278
	public enum LoopVers
	{
		// Token: 0x0400077C RID: 1916
		noLoop,
		// Token: 0x0400077D RID: 1917
		loopPlaylist,
		// Token: 0x0400077E RID: 1918
		loopSong
	}
}
