using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SFB;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;

// Token: 0x02000082 RID: 130
public class UI_RshwCreator : MonoBehaviour
{
	// Token: 0x060002C2 RID: 706 RVA: 0x0001BDEB File Offset: 0x00019FEB
	private void Start()
	{
		this.playRecord = base.GetComponent<UI_PlayRecord>();
		this.manager = base.GetComponent<UI_ShowtapeManager>();
		this.youtubePlayer = base.GetComponent<YoutubePlayer>();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001BE14 File Offset: 0x0001A014
	public UI_RshwCreator.addWavResult AddWav()
	{
		this.manager.speakerClip = null;
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		Debug.Log("Adding Wav");
		this.manager.wavPath = "";
		this.manager.showtapeSegmentPaths[0] = "";
		string[] array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape Audio", "", "wav", false);
		if (array.Length == 0)
		{
			return UI_RshwCreator.addWavResult.noSource;
		}
		if (!(array[0] != ""))
		{
			return UI_RshwCreator.addWavResult.none;
		}
		this.manager.wavPath = array[0];
		this.manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(array[0]), "", false);
		this.manager.audioVideoGetData.Invoke();
		this.CreateBitArray();
		if (this.manager.speakerClip == null)
		{
			return UI_RshwCreator.addWavResult.uncompressed;
		}
		return UI_RshwCreator.addWavResult.noSource;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0001BEEC File Offset: 0x0001A0EC
	public void AddWavSpecial()
	{
		CursorLockMode lockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		Debug.Log("Adding Wav");
		this.manager.wavPath = "";
		this.manager.showtapeSegmentPaths[0] = "";
		string[] array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape Audio", "", "wav", false);
		if (array.Length != 0 && array[0] != "")
		{
			this.manager.wavPath = array[0];
			this.manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(array[0]), "", false);
			this.CreateBitArray();
		}
		Cursor.lockState = lockState;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0001BF90 File Offset: 0x0001A190
	public void StartNewShow()
	{
		Debug.Log("Starting New Show");
		this.manager.disableCharactersOnStart = false;
		this.manager.recordMovements = true;
		if (this.manager.wavPath != "")
		{
			this.CreateBitArray();
		}
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0001BFDC File Offset: 0x0001A1DC
	private void CreateBitArray()
	{
		this.manager.rshwData = new BitArray[100];
		for (int i = 0; i < this.manager.rshwData.Length; i++)
		{
			this.manager.rshwData[i] = new BitArray(300);
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001C02C File Offset: 0x0001A22C
	public void SaveRecording()
	{
		if (this.manager.rshwData != null)
		{
			this.manager.audioVideoPause.Invoke();
			this.manager.recordMovements = false;
			this.manager.playMovements = false;
			rshwFormat rshwFormat = new rshwFormat
			{
				audioData = OpenWavParser.AudioClipToByteArray(this.manager.speakerClip, OpenWavParser.Resolution._16bit)
			};
			List<int> list = new List<int>();
			for (int i = 0; i < this.manager.rshwData.Length; i++)
			{
				list.Add(0);
				for (int j = 0; j < 300; j++)
				{
					if (this.manager.rshwData[i].Get(j))
					{
						list.Add(j + 1);
					}
				}
			}
			rshwFormat.signalData = list.ToArray();
			rshwFormat.Save(this.manager.showtapeSegmentPaths[0]);
			Debug.Log("Showtape Saved");
			return;
		}
		Debug.Log("No Showtape. Did not save.");
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0001C118 File Offset: 0x0001A318
	public bool SaveRecordingAs()
	{
		if (this.manager.rshwData != null && this.manager.speakerClip != null)
		{
			CursorLockMode lockState = Cursor.lockState;
			Cursor.lockState = CursorLockMode.None;
			this.manager.audioVideoPause.Invoke();
			this.manager.recordMovements = false;
			this.manager.playMovements = false;
			if (this.manager.speakerClip != null)
			{
				string text = StandaloneFileBrowser.SaveFilePanel("Save Showtape", "", "MyShowtape", this.manager.fileExtention);
				Debug.Log("Showtape Saved: " + text);
				if (string.IsNullOrEmpty(text))
				{
					Debug.Log("No Showtape. Did not save.");
					AudioSource component = GameObject.Find("GlobalAudio").GetComponent<AudioSource>();
					component.volume = 1f;
					component.PlayOneShot((AudioClip)Resources.Load("Deny"));
					Cursor.lockState = lockState;
					return false;
				}
				this.manager.showtapeSegmentPaths = new string[1];
				this.manager.showtapeSegmentPaths[0] = text;
				rshwFormat rshwFormat = new rshwFormat
				{
					audioData = OpenWavParser.AudioClipToByteArray(this.manager.speakerClip, OpenWavParser.Resolution._16bit)
				};
				List<int> list = new List<int>();
				for (int i = 0; i < this.manager.rshwData.Length; i++)
				{
					list.Add(0);
					for (int j = 0; j < 300; j++)
					{
						if (this.manager.rshwData[i].Get(j))
						{
							list.Add(j + 1);
						}
					}
				}
				rshwFormat.signalData = list.ToArray();
				rshwFormat.Save(text);
			}
			Cursor.lockState = lockState;
			return true;
		}
		Debug.Log("No Showtape. Did not save.");
		AudioSource component2 = GameObject.Find("GlobalAudio").GetComponent<AudioSource>();
		component2.volume = 1f;
		component2.PlayOneShot((AudioClip)Resources.Load("Deny"));
		return false;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0001C301 File Offset: 0x0001A501
	public void LoadFromURL(string url)
	{
		base.StartCoroutine(this.LoadRoutineA(url));
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0001C311 File Offset: 0x0001A511
	private IEnumerator LoadRoutineA(string url)
	{
		yield return base.StartCoroutine(this.LoadRoutineB(url));
		yield break;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0001C327 File Offset: 0x0001A527
	private IEnumerator LoadRoutineB(string url)
	{
		this.manager.disableCharactersOnStart = false;
		this.manager.playMovements = false;
		if (url != "")
		{
			this.manager.referenceSpeaker.volume = this.manager.refSpeakerVol;
			this.manager.referenceSpeaker.time = 0f;
			this.manager.useVideoAsReference = false;
			this.manager.referenceVideo.time = 0.0;
			this.manager.timeSongStarted = 0f;
			this.manager.timeSongOffset = 0f;
			this.manager.timePauseStart = 0f;
			this.manager.timeInputSpeedStart = 0f;
			yield return null;
			this.manager.curtainOpen.Invoke();
			yield return null;
			rshwFormat thefile = rshwFormat.ReadFromFile(url);
			yield return null;
			this.manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(thefile.audioData, "", false);
			yield return null;
			List<BitArray> list = new List<BitArray>();
			int countlength = 0;
			if (thefile.signalData[0] != 0)
			{
				countlength = 1;
				BitArray item = new BitArray(300);
				list.Add(item);
			}
			for (int i = 0; i < thefile.signalData.Length; i++)
			{
				if (thefile.signalData[i] == 0)
				{
					countlength++;
					BitArray item2 = new BitArray(300);
					list.Add(item2);
				}
				else
				{
					list[countlength - 1].Set(thefile.signalData[i] - 1, true);
				}
			}
			this.manager.rshwData = list.ToArray();
			yield return null;
			if (File.Exists(url.Remove(url.Length - Mathf.Max(this.manager.fileExtention.Length, 4)) + "mp4"))
			{
				Debug.Log("Video Found for Showtape.");
				this.manager.videoPath = url.Remove(url.Length - Mathf.Max(this.manager.fileExtention.Length, 4)) + "mp4";
			}
			else
			{
				this.manager.videoPath = "";
			}
			this.manager.audioVideoGetData.Invoke();
			yield return null;
			if (this.manager.recordMovements)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Recording Showtape: ",
					url,
					" (Length: ",
					((float)countlength / this.manager.dataStreamedFPS).ToString(),
					")"
				}));
			}
			else
			{
				Debug.Log(string.Concat(new string[]
				{
					"Playing Showtape: ",
					url,
					" (Length: ",
					((float)countlength / this.manager.dataStreamedFPS).ToString(),
					")"
				}));
			}
			yield return null;
			this.manager.timeSongStarted = Time.time;
			this.manager.syncTvsAndSpeakers.Invoke();
			Debug.Log(string.Concat(new string[]
			{
				"Length = ",
				this.manager.referenceSpeaker.clip.length.ToString(),
				" Channels = ",
				this.manager.referenceSpeaker.clip.channels.ToString(),
				" Total = ",
				(this.manager.referenceSpeaker.clip.length / (float)this.manager.referenceSpeaker.clip.channels).ToString()
			}));
			thefile = null;
		}
		yield break;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001C340 File Offset: 0x0001A540
	public void LoadYoutubeShow(string url)
	{
		this.playRecord.videoplayer.source = VideoSource.Url;
		this.playRecord.videoplayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		this.youtubePlayer.youtubeUrl = url;
		this.manager.videoPath = url;
		this.PrepareYTVideoA();
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0001C390 File Offset: 0x0001A590
	public void PrepareYTVideoA()
	{
		UI_RshwCreator.<PrepareYTVideoA>d__15 <PrepareYTVideoA>d__;
		<PrepareYTVideoA>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<PrepareYTVideoA>d__.<>4__this = this;
		<PrepareYTVideoA>d__.<>1__state = -1;
		<PrepareYTVideoA>d__.<>t__builder.Start<UI_RshwCreator.<PrepareYTVideoA>d__15>(ref <PrepareYTVideoA>d__);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0001C3C7 File Offset: 0x0001A5C7
	private IEnumerator PrepareYTVideoB()
	{
		this.manager.referenceSpeaker.volume = 1f;
		this.manager.disableCharactersOnStart = false;
		this.manager.playMovements = false;
		this.manager.referenceVideo.time = 0.0;
		this.manager.referenceSpeaker.time = 0f;
		this.manager.useVideoAsReference = true;
		this.manager.timeSongStarted = 0f;
		this.manager.timeSongOffset = 0f;
		this.manager.timePauseStart = 0f;
		this.manager.timeInputSpeedStart = 0f;
		yield return null;
		this.manager.curtainOpen.Invoke();
		yield return null;
		base.GetComponent<AudioSource>();
		this.manager.rshwData = new BitArray[100];
		for (int i = 0; i < this.manager.rshwData.Length; i++)
		{
			this.manager.rshwData[i] = new BitArray(300);
		}
		yield return null;
		this.manager.audioVideoGetData.Invoke();
		yield return null;
		this.manager.timeSongStarted = Time.time;
		this.manager.syncTvsAndSpeakers.Invoke();
		Debug.Log(string.Concat(new string[]
		{
			"Length = ",
			this.manager.referenceVideo.length.ToString(),
			" Channels = ",
			this.manager.referenceVideo.GetAudioChannelCount(0).ToString(),
			" Total = ",
			(this.manager.referenceVideo.length / (double)((float)this.manager.referenceVideo.GetAudioChannelCount(0))).ToString()
		}));
		yield break;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
	public void EraseShowtape()
	{
		this.manager.disableCharactersOnStart = false;
		this.manager.playMovements = false;
		this.manager.referenceSpeaker.time = 0f;
		this.manager.timeSongStarted = 0f;
		this.manager.timeSongOffset = 0f;
		this.manager.timePauseStart = 0f;
		this.manager.timeInputSpeedStart = 0f;
		this.manager.curtainOpen.Invoke();
		this.manager.speakerClip = null;
		this.manager.rshwData = null;
		this.manager.videoPath = "";
		this.manager.referenceSpeaker.clip = null;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x0001C49C File Offset: 0x0001A69C
	public void ReplaceShowAudio()
	{
		this.manager.showtapeSegmentPaths = new string[1];
		string[] array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape", "", this.manager.fileExtention, false);
		if (array.Length != 0)
		{
			this.manager.showtapeSegmentPaths[0] = array[0];
			this.manager.currentShowtapeSegment = 0;
			this.manager.referenceSpeaker.time = 0f;
			this.manager.playMovements = false;
			if (this.manager.showtapeSegmentPaths[0] != "")
			{
				rshwFormat rshwFormat = rshwFormat.ReadFromFile(this.manager.showtapeSegmentPaths[0]);
				List<BitArray> list = new List<BitArray>();
				int num = 0;
				if (rshwFormat.signalData[0] != 0)
				{
					num = 1;
					BitArray item = new BitArray(300);
					list.Add(item);
				}
				for (int i = 0; i < rshwFormat.signalData.Length; i++)
				{
					if (rshwFormat.signalData[i] == 0)
					{
						num++;
						BitArray item2 = new BitArray(300);
						list.Add(item2);
					}
					else
					{
						list[num - 1].Set(rshwFormat.signalData[i] - 1, true);
					}
				}
				this.manager.rshwData = list.ToArray();
				array = StandaloneFileBrowser.OpenFilePanel("Browse Showtape Audio", "", "wav", false);
				if (array.Length != 0 && array[0] != "")
				{
					this.manager.wavPath = array[0];
					this.manager.speakerClip = OpenWavParser.ByteArrayToAudioClip(File.ReadAllBytes(array[0]), "", false);
					this.SaveRecording();
				}
			}
		}
	}

	// Token: 0x040003B4 RID: 948
	private UI_ShowtapeManager manager;

	// Token: 0x040003B5 RID: 949
	private UI_PlayRecord playRecord;

	// Token: 0x040003B6 RID: 950
	private YoutubePlayer youtubePlayer;

	// Token: 0x02000111 RID: 273
	public enum addWavResult
	{
		// Token: 0x04000767 RID: 1895
		none,
		// Token: 0x04000768 RID: 1896
		noSource,
		// Token: 0x04000769 RID: 1897
		uncompressed
	}
}
