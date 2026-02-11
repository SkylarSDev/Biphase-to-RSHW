using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000081 RID: 129
public class UI_PlayRecord : MonoBehaviour
{
	// Token: 0x060002A6 RID: 678 RVA: 0x00018A54 File Offset: 0x00016C54
	private void Awake()
	{
		this.UpdateTickets();
		this.thePlayer = GameObject.Find("Player");
		this.inputHandlercomp = this.mackValves.GetComponent<InputHandler>();
		this.mack = this.mackValves.GetComponent<Mack_Valves>();
		this.manager.inputHandler = this.inputHandlercomp;
		this.videoplayer = base.GetComponent<VideoPlayer>();
		this.creator = base.GetComponent<UI_RshwCreator>();
		for (int i = 0; i < this.stages.Length; i++)
		{
			this.stages[i].Startup();
		}
		this.RecreateAllCharacters("");
		this.SwapCheck();
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00018AF4 File Offset: 0x00016CF4
	private void Update()
	{
		if (this.manager.recordMovements && this.manager.referenceSpeaker.clip != null && (double)this.manager.referenceSpeaker.time >= (double)this.manager.speakerClip.length - 0.05)
		{
			GameObject gameObject = GameObject.Find("Tutorial");
			if (gameObject != null)
			{
				gameObject.GetComponent<TutorialStart>().AttemptAdvanceTutorial("FinishShowtape");
			}
			this.SpecialSaveAs(11);
		}
		this.UpdateAnims();
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00018B88 File Offset: 0x00016D88
	private void UpdateAnims()
	{
		UI_PlayRecord.SignalChange signalChange = this.signalChange;
		if (signalChange != UI_PlayRecord.SignalChange.PreCU)
		{
			if (signalChange != UI_PlayRecord.SignalChange.PrePTT)
			{
			}
		}
		else
		{
			bool flag = this.mack.topDrawer[85];
			this.mack.topDrawer[85] = this.mack.topDrawer[80];
			this.mack.topDrawer[80] = false;
			this.mack.topDrawer[83] = flag;
			this.mack.topDrawer[92] = this.mack.topDrawer[90];
			this.mack.topDrawer[93] = this.mack.topDrawer[91];
			this.mack.bottomDrawer[79] = this.mack.bottomDrawer[74];
			this.mack.bottomDrawer[90] = this.mack.bottomDrawer[74];
			this.mack.bottomDrawer[89] = false;
			this.mack.bottomDrawer[63] = true;
			this.mack.topDrawer[25] = !this.mack.topDrawer[25];
			this.mack.topDrawer[26] = !this.mack.topDrawer[26];
		}
		UI_PlayRecord.SpotlightsOverride spotlightsOverride = this.spotlightsOverride;
		if (spotlightsOverride != UI_PlayRecord.SpotlightsOverride.RAE)
		{
			if (spotlightsOverride != UI_PlayRecord.SpotlightsOverride.CU)
			{
			}
		}
		else
		{
			this.mack.topDrawer[80] = true;
			this.mack.topDrawer[81] = true;
			this.mack.topDrawer[82] = true;
			this.mack.topDrawer[83] = true;
			this.mack.topDrawer[84] = true;
			this.mack.topDrawer[85] = true;
			this.mack.topDrawer[86] = true;
			this.mack.topDrawer[87] = true;
			this.mack.bottomDrawer[87] = true;
			this.mack.bottomDrawer[88] = true;
			this.mack.bottomDrawer[89] = true;
			this.mack.bottomDrawer[95] = true;
		}
		UI_PlayRecord.DimOverride dimOverride = this.dimOverride;
		if (dimOverride != UI_PlayRecord.DimOverride.RAE)
		{
			if (dimOverride != UI_PlayRecord.DimOverride.CU)
			{
			}
		}
		else
		{
			this.mack.bottomDrawer[116] = true;
		}
		UI_PlayRecord.KlunkBitsChange klunkBitsChange = this.klunkBitsChange;
		if (klunkBitsChange != UI_PlayRecord.KlunkBitsChange.Rockafire)
		{
			if (klunkBitsChange == UI_PlayRecord.KlunkBitsChange.Dreamfactory)
			{
				this.mack.topDrawer[0] = this.mack.topDrawer[96];
				this.mack.topDrawer[1] = this.mack.topDrawer[97];
				this.mack.topDrawer[2] = this.mack.topDrawer[98];
				this.mack.topDrawer[3] = this.mack.topDrawer[99];
				this.mack.topDrawer[4] = this.mack.topDrawer[100];
				this.mack.topDrawer[5] = this.mack.topDrawer[101];
				this.mack.topDrawer[6] = this.mack.topDrawer[102];
				this.mack.topDrawer[7] = this.mack.topDrawer[103];
				this.mack.topDrawer[8] = this.mack.topDrawer[104];
				this.mack.topDrawer[9] = this.mack.topDrawer[105];
				this.mack.topDrawer[10] = this.mack.topDrawer[106];
				this.mack.topDrawer[11] = this.mack.topDrawer[107];
				this.mack.topDrawer[12] = this.mack.topDrawer[108];
				this.mack.topDrawer[13] = this.mack.topDrawer[109];
				this.mack.topDrawer[14] = this.mack.topDrawer[110];
				this.mack.topDrawer[15] = this.mack.topDrawer[111];
				this.mack.topDrawer[16] = this.mack.bottomDrawer[98];
				this.mack.topDrawer[17] = this.mack.bottomDrawer[99];
				this.mack.topDrawer[18] = this.mack.bottomDrawer[100];
				this.mack.topDrawer[19] = this.mack.bottomDrawer[101];
				this.mack.topDrawer[35] = this.mack.bottomDrawer[102];
				this.mack.topDrawer[36] = this.mack.bottomDrawer[103];
				this.mack.topDrawer[96] = this.mack.topDrawer[112];
				this.mack.topDrawer[97] = this.mack.topDrawer[113];
				this.mack.topDrawer[98] = this.mack.topDrawer[114];
				this.mack.topDrawer[99] = this.mack.topDrawer[115];
				this.mack.topDrawer[100] = this.mack.topDrawer[116];
				this.mack.topDrawer[101] = this.mack.topDrawer[117];
				this.mack.topDrawer[102] = this.mack.topDrawer[118];
				this.mack.topDrawer[103] = this.mack.bottomDrawer[97];
				this.mack.bottomDrawer[69] = this.mack.bottomDrawer[107];
			}
		}
		else
		{
			this.mack.topDrawer[96] = this.mack.topDrawer[0];
			this.mack.topDrawer[97] = this.mack.topDrawer[1];
			this.mack.topDrawer[98] = this.mack.topDrawer[2];
			this.mack.topDrawer[99] = this.mack.topDrawer[3];
			this.mack.topDrawer[100] = this.mack.topDrawer[4];
			this.mack.topDrawer[101] = this.mack.topDrawer[5];
			this.mack.topDrawer[102] = this.mack.topDrawer[6];
			this.mack.topDrawer[103] = this.mack.topDrawer[7];
			this.mack.topDrawer[104] = this.mack.topDrawer[8];
			this.mack.topDrawer[105] = this.mack.topDrawer[9];
			this.mack.topDrawer[106] = this.mack.topDrawer[10];
			this.mack.topDrawer[107] = this.mack.topDrawer[11];
			this.mack.topDrawer[108] = this.mack.topDrawer[12];
			this.mack.topDrawer[109] = this.mack.topDrawer[13];
			this.mack.topDrawer[110] = this.mack.topDrawer[14];
			this.mack.topDrawer[111] = this.mack.topDrawer[15];
			this.mack.bottomDrawer[98] = this.mack.topDrawer[16];
			this.mack.bottomDrawer[99] = this.mack.topDrawer[17];
			this.mack.bottomDrawer[100] = this.mack.topDrawer[18];
			this.mack.bottomDrawer[101] = this.mack.topDrawer[19];
			this.mack.bottomDrawer[102] = this.mack.topDrawer[35];
			this.mack.bottomDrawer[103] = this.mack.topDrawer[36];
			this.mack.topDrawer[112] = this.mack.topDrawer[96];
			this.mack.topDrawer[113] = this.mack.topDrawer[97];
			this.mack.topDrawer[114] = this.mack.topDrawer[98];
			this.mack.topDrawer[115] = this.mack.topDrawer[99];
			this.mack.topDrawer[116] = this.mack.topDrawer[100];
			this.mack.topDrawer[117] = this.mack.topDrawer[101];
			this.mack.topDrawer[118] = this.mack.topDrawer[102];
			this.mack.bottomDrawer[97] = this.mack.topDrawer[103];
			this.mack.bottomDrawer[107] = this.mack.bottomDrawer[69];
			this.mack.topDrawer[87] = this.mack.bottomDrawer[104];
			this.mack.topDrawer[86] = this.mack.bottomDrawer[105];
		}
		UI_PlayRecord.LightingOverride lightingOverride = this.lightingOverride;
		if (lightingOverride != UI_PlayRecord.LightingOverride.DimsOff)
		{
			if (lightingOverride == UI_PlayRecord.LightingOverride.DimsOn)
			{
				this.mack.topDrawer[80] = false;
				this.mack.topDrawer[81] = false;
				this.mack.topDrawer[82] = false;
				this.mack.topDrawer[83] = false;
				this.mack.topDrawer[84] = false;
				this.mack.topDrawer[85] = false;
				this.mack.topDrawer[86] = false;
				this.mack.topDrawer[87] = false;
				this.mack.topDrawer[65] = false;
				this.mack.topDrawer[66] = false;
				this.mack.topDrawer[67] = false;
				this.mack.topDrawer[68] = false;
				this.mack.topDrawer[70] = false;
				this.mack.topDrawer[71] = false;
				this.mack.topDrawer[72] = false;
				this.mack.topDrawer[73] = false;
				this.mack.topDrawer[74] = false;
				this.mack.topDrawer[75] = false;
				this.mack.topDrawer[76] = false;
				this.mack.topDrawer[77] = false;
				this.mack.topDrawer[78] = false;
				this.mack.topDrawer[79] = false;
				this.mack.bottomDrawer[65] = false;
				this.mack.bottomDrawer[66] = false;
				this.mack.bottomDrawer[67] = false;
				this.mack.bottomDrawer[68] = false;
				this.mack.bottomDrawer[69] = false;
				this.mack.bottomDrawer[70] = false;
				this.mack.bottomDrawer[71] = false;
				this.mack.bottomDrawer[72] = false;
				this.mack.bottomDrawer[73] = false;
				this.mack.bottomDrawer[74] = false;
				this.mack.bottomDrawer[75] = false;
				this.mack.bottomDrawer[76] = false;
				this.mack.bottomDrawer[77] = false;
				this.mack.bottomDrawer[78] = false;
				this.mack.bottomDrawer[79] = false;
				this.mack.bottomDrawer[80] = false;
				this.mack.bottomDrawer[81] = false;
				this.mack.bottomDrawer[82] = false;
				this.mack.bottomDrawer[83] = false;
				this.mack.bottomDrawer[84] = false;
				this.mack.bottomDrawer[85] = false;
				this.mack.bottomDrawer[86] = false;
				this.mack.bottomDrawer[87] = false;
				this.mack.bottomDrawer[88] = false;
				this.mack.bottomDrawer[89] = false;
				this.mack.bottomDrawer[90] = false;
				this.mack.bottomDrawer[91] = false;
				this.mack.bottomDrawer[92] = false;
				this.mack.bottomDrawer[93] = false;
				this.mack.bottomDrawer[94] = false;
				this.mack.bottomDrawer[95] = false;
				this.mack.bottomDrawer[104] = false;
				this.mack.bottomDrawer[105] = false;
				this.mack.bottomDrawer[114] = false;
				this.mack.bottomDrawer[115] = false;
				this.mack.bottomDrawer[116] = true;
			}
		}
		else
		{
			this.mack.topDrawer[80] = false;
			this.mack.topDrawer[81] = false;
			this.mack.topDrawer[82] = false;
			this.mack.topDrawer[83] = false;
			this.mack.topDrawer[84] = false;
			this.mack.topDrawer[85] = false;
			this.mack.topDrawer[86] = false;
			this.mack.topDrawer[87] = false;
			this.mack.topDrawer[65] = false;
			this.mack.topDrawer[66] = false;
			this.mack.topDrawer[67] = false;
			this.mack.topDrawer[68] = false;
			this.mack.topDrawer[70] = false;
			this.mack.topDrawer[71] = false;
			this.mack.topDrawer[72] = false;
			this.mack.topDrawer[73] = false;
			this.mack.topDrawer[74] = false;
			this.mack.topDrawer[75] = false;
			this.mack.topDrawer[76] = false;
			this.mack.topDrawer[77] = false;
			this.mack.topDrawer[78] = false;
			this.mack.topDrawer[79] = false;
			this.mack.bottomDrawer[65] = false;
			this.mack.bottomDrawer[66] = false;
			this.mack.bottomDrawer[67] = false;
			this.mack.bottomDrawer[68] = false;
			this.mack.bottomDrawer[69] = false;
			this.mack.bottomDrawer[70] = false;
			this.mack.bottomDrawer[71] = false;
			this.mack.bottomDrawer[72] = false;
			this.mack.bottomDrawer[73] = false;
			this.mack.bottomDrawer[74] = false;
			this.mack.bottomDrawer[75] = false;
			this.mack.bottomDrawer[76] = false;
			this.mack.bottomDrawer[77] = false;
			this.mack.bottomDrawer[78] = false;
			this.mack.bottomDrawer[79] = false;
			this.mack.bottomDrawer[80] = false;
			this.mack.bottomDrawer[81] = false;
			this.mack.bottomDrawer[82] = false;
			this.mack.bottomDrawer[83] = false;
			this.mack.bottomDrawer[84] = false;
			this.mack.bottomDrawer[85] = false;
			this.mack.bottomDrawer[86] = false;
			this.mack.bottomDrawer[87] = false;
			this.mack.bottomDrawer[88] = false;
			this.mack.bottomDrawer[89] = false;
			this.mack.bottomDrawer[90] = false;
			this.mack.bottomDrawer[91] = false;
			this.mack.bottomDrawer[92] = false;
			this.mack.bottomDrawer[93] = false;
			this.mack.bottomDrawer[94] = false;
			this.mack.bottomDrawer[95] = false;
			this.mack.bottomDrawer[104] = false;
			this.mack.bottomDrawer[105] = false;
			this.mack.bottomDrawer[114] = false;
			this.mack.bottomDrawer[115] = false;
			this.mack.bottomDrawer[116] = false;
		}
		UI_PlayRecord.ServiceLights serviceLights = this.serviceLights;
		if (serviceLights != UI_PlayRecord.ServiceLights.Off && serviceLights == UI_PlayRecord.ServiceLights.On)
		{
			this.mack.topDrawer[75] = true;
			this.mack.topDrawer[76] = true;
			this.mack.topDrawer[77] = true;
			this.mack.bottomDrawer[65] = true;
			this.mack.bottomDrawer[66] = true;
			this.mack.bottomDrawer[67] = true;
			this.mack.bottomDrawer[70] = true;
			this.mack.bottomDrawer[71] = true;
			this.mack.bottomDrawer[72] = true;
			this.mack.bottomDrawer[75] = true;
			this.mack.bottomDrawer[76] = true;
			this.mack.bottomDrawer[77] = true;
			this.mack.topDrawer[65] = false;
			this.mack.topDrawer[66] = false;
			this.mack.topDrawer[67] = false;
			this.mack.topDrawer[68] = false;
			this.mack.topDrawer[70] = false;
			this.mack.topDrawer[71] = false;
			this.mack.topDrawer[72] = false;
			this.mack.topDrawer[73] = false;
			this.mack.topDrawer[74] = false;
			this.mack.topDrawer[78] = false;
			this.mack.topDrawer[79] = false;
			this.mack.topDrawer[80] = false;
			this.mack.topDrawer[81] = false;
			this.mack.topDrawer[82] = false;
			this.mack.topDrawer[83] = false;
			this.mack.topDrawer[84] = false;
			this.mack.topDrawer[85] = false;
			this.mack.topDrawer[86] = false;
			this.mack.topDrawer[87] = false;
			this.mack.bottomDrawer[68] = false;
			this.mack.bottomDrawer[69] = false;
			this.mack.bottomDrawer[73] = false;
			this.mack.bottomDrawer[74] = false;
			this.mack.bottomDrawer[78] = false;
			this.mack.bottomDrawer[79] = false;
			this.mack.bottomDrawer[80] = false;
			this.mack.bottomDrawer[81] = false;
			this.mack.bottomDrawer[82] = false;
			this.mack.bottomDrawer[83] = false;
			this.mack.bottomDrawer[84] = false;
			this.mack.bottomDrawer[85] = false;
			this.mack.bottomDrawer[86] = false;
			this.mack.bottomDrawer[87] = false;
			this.mack.bottomDrawer[88] = false;
			this.mack.bottomDrawer[89] = false;
			this.mack.bottomDrawer[90] = false;
			this.mack.bottomDrawer[91] = false;
			this.mack.bottomDrawer[92] = false;
			this.mack.bottomDrawer[93] = false;
			this.mack.bottomDrawer[94] = false;
			this.mack.bottomDrawer[95] = false;
			this.mack.bottomDrawer[104] = false;
			this.mack.bottomDrawer[105] = false;
			this.mack.bottomDrawer[107] = false;
			this.mack.bottomDrawer[108] = false;
			this.mack.bottomDrawer[114] = false;
			this.mack.bottomDrawer[115] = false;
			this.mack.bottomDrawer[116] = false;
		}
		this.characterEvent.Invoke(Time.deltaTime * 60f);
		for (int i = 0; i < this.stages[this.currentStage].lightValves.Length; i++)
		{
			this.stages[this.currentStage].lightValves[i].CreateMovements(Time.deltaTime * 60f);
		}
		if (this.stages[this.currentStage].curtainValves != null)
		{
			if (this.manager.referenceSpeaker.pitch < 0f)
			{
				this.stages[this.currentStage].curtainValves.CreateMovements(Time.deltaTime * 60f, true);
			}
			else
			{
				this.stages[this.currentStage].curtainValves.CreateMovements(Time.deltaTime * 60f, false);
			}
		}
		for (int j = 0; j < this.stages[this.currentStage].tableValves.Length; j++)
		{
			this.stages[this.currentStage].tableValves[j].CreateMovements(Time.deltaTime * 60f);
		}
		if (this.stages[this.currentStage].texController != null)
		{
			this.stages[this.currentStage].texController.CreateTex();
		}
		if (this.manager.videoPath != "")
		{
			for (int k = 0; k < this.stages[this.currentStage].tvs.Length; k++)
			{
				bool flag2 = false;
				bool flag3 = false;
				if (this.stages[this.currentStage].tvs[k].drawer)
				{
					if (this.mack.bottomDrawer[this.stages[this.currentStage].tvs[k].bitOff])
					{
						flag2 = true;
					}
					if (this.mack.bottomDrawer[this.stages[this.currentStage].tvs[k].bitOn])
					{
						flag3 = true;
					}
				}
				else
				{
					if (this.mack.topDrawer[this.stages[this.currentStage].tvs[k].bitOff])
					{
						flag2 = true;
					}
					if (this.mack.topDrawer[this.stages[this.currentStage].tvs[k].bitOn])
					{
						flag3 = true;
					}
				}
				if (this.stages[this.currentStage].tvs[k].onWhenCurtain)
				{
					if (this.stages[this.currentStage].curtainValves.curtainOverride && this.stages[this.currentStage].tvs[k].curtainSubState == 1)
					{
						this.stages[this.currentStage].tvs[k].curtainSubState = 2;
					}
					if (!this.stages[this.currentStage].curtainValves.curtainOverride && this.stages[this.currentStage].tvs[k].curtainSubState == 3)
					{
						this.stages[this.currentStage].tvs[k].curtainSubState = 0;
					}
					if (this.stages[this.currentStage].tvs[k].curtainSubState == 0)
					{
						for (int l = 0; l < this.stages[this.currentStage].tvs[k].tvs.Length; l++)
						{
							this.stages[this.currentStage].tvs[k].tvs[l].material.SetColor("_BaseColor", Color.black);
						}
						this.stages[this.currentStage].tvs[k].curtainSubState = 1;
						Debug.Log("Force Closed Curtain");
					}
					else if (this.stages[this.currentStage].tvs[k].curtainSubState == 2)
					{
						for (int m = 0; m < this.stages[this.currentStage].tvs[k].tvs.Length; m++)
						{
							this.stages[this.currentStage].tvs[k].tvs[m].material.SetColor("_BaseColor", Color.white);
						}
						this.stages[this.currentStage].tvs[k].curtainSubState = 3;
						Debug.Log("Force Opened Curtain");
					}
				}
				switch (this.stages[this.currentStage].tvs[k].tvSettings)
				{
				case ShowTV.tvSetting.onOnly:
					if (!flag3)
					{
						for (int n = 0; n < this.stages[this.currentStage].tvs[k].tvs.Length; n++)
						{
							this.stages[this.currentStage].tvs[k].tvs[n].material.SetColor("_BaseColor", Color.black);
						}
					}
					else
					{
						for (int num = 0; num < this.stages[this.currentStage].tvs[k].tvs.Length; num++)
						{
							this.stages[this.currentStage].tvs[k].tvs[num].material.SetColor("_BaseColor", Color.white);
						}
					}
					break;
				case ShowTV.tvSetting.offOn:
					if (this.stages[this.currentStage].tvs[k].onWhenCurtain && this.manager.referenceSpeaker.pitch < 0f)
					{
						bool flag4 = flag2;
						flag2 = flag3;
						flag3 = flag4;
					}
					if (flag2)
					{
						Debug.Log("Closed Curtain");
						for (int num2 = 0; num2 < this.stages[this.currentStage].tvs[k].tvs.Length; num2++)
						{
							this.stages[this.currentStage].tvs[k].tvs[num2].material.SetColor("_BaseColor", Color.black);
						}
					}
					if (flag3)
					{
						Debug.Log("Opened Curtain");
						for (int num3 = 0; num3 < this.stages[this.currentStage].tvs[k].tvs.Length; num3++)
						{
							this.stages[this.currentStage].tvs[k].tvs[num3].material.SetColor("_BaseColor", Color.white);
						}
					}
					break;
				case ShowTV.tvSetting.none:
					for (int num4 = 0; num4 < this.stages[this.currentStage].tvs[k].tvs.Length; num4++)
					{
						this.stages[this.currentStage].tvs[k].tvs[num4].material.SetColor("_BaseColor", Color.white);
					}
					break;
				}
			}
		}
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0001A618 File Offset: 0x00018818
	public void SwitchWindow(int thewindow)
	{
		switch (thewindow)
		{
		case 0:
			base.GetComponent<UI_WindowMaker>().MakeTitleWindow();
			this.creator.EraseShowtape();
			return;
		case 1:
			base.GetComponent<UI_WindowMaker>().MakeThreeWindow(this.icons[0], this.icons[1], this.icons[2], 0, 8, 2, 3, "Customize", "Play", "Record", 45);
			this.WindowSwitchDisable(true);
			this.creator.EraseShowtape();
			return;
		case 2:
			if (base.GetComponent<UI_WindowMaker>().allShowtapes.Length != 0)
			{
				base.GetComponent<UI_WindowMaker>().MakeThreeWindow(this.icons[4], this.icons[1], this.icons[10], 1, 7, 6, 32, "Play Showtape", "Play Segment", "Show List", 37);
			}
			else
			{
				base.GetComponent<UI_WindowMaker>().MakeTwoWindow(this.icons[4], this.icons[1], 1, 7, 6, "Play Showtape", "Play Segment", 37);
			}
			this.creator.EraseShowtape();
			this.WindowSwitchDisable(true);
			return;
		case 3:
			base.GetComponent<UI_WindowMaker>().MakeTwoWindow(this.icons[2], this.icons[3], 1, 5, 4, "New Recording", "Edit Recording", 37);
			this.WindowSwitchDisable(true);
			this.creator.EraseShowtape();
			return;
		case 4:
			base.GetComponent<UI_WindowMaker>().MakeTwoWindow(this.icons[3], this.icons[5], 3, 34, 21, "Edit Segment", "Add to Segment", 30);
			this.WindowSwitchDisable(true);
			this.creator.EraseShowtape();
			return;
		case 5:
			base.GetComponent<UI_WindowMaker>().MakeNewRecordWindow();
			this.creator.EraseShowtape();
			return;
		case 6:
			this.manager.Load();
			if (this.manager.rshwData != null)
			{
				base.GetComponent<UI_WindowMaker>().MakePlayWindow(false);
				return;
			}
			break;
		case 7:
			this.manager.LoadFolder();
			if (this.manager.rshwData != null)
			{
				base.GetComponent<UI_WindowMaker>().MakePlayWindow(false);
				return;
			}
			break;
		case 8:
			base.GetComponent<UI_WindowMaker>().MakeTwoWindow(this.icons[8], this.icons[9], 1, 16, 9, "Edit Stage", "Edit Characters", 35);
			return;
		case 9:
			base.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeIconsWindow(this.characters);
			return;
		case 10:
		case 12:
		case 13:
		case 14:
		case 15:
		case 18:
		case 19:
		case 20:
		case 25:
		case 26:
		case 27:
		case 29:
		case 30:
		case 31:
			break;
		case 11:
			base.GetComponent<UI_WindowMaker>().MakeRecordIconsWindow();
			this.WindowSwitchDisable(false);
			this.creator.EraseShowtape();
			return;
		case 16:
			this.StageCustomMenu();
			return;
		case 17:
			if (this.manager.rshwData != null)
			{
				base.GetComponent<UI_WindowMaker>().MakePlayWindow(false);
				return;
			}
			break;
		case 21:
			base.GetComponent<UI_WindowMaker>().MakeRecordIconsWindow();
			this.WindowSwitchDisable(false);
			this.creator.EraseShowtape();
			return;
		case 22:
			base.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(0);
			return;
		case 23:
			base.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(-1);
			return;
		case 24:
			base.GetComponent<UI_WindowMaker>().MakeDeleteMoveMenu(1);
			return;
		case 28:
			base.GetComponent<UI_WindowMaker>().MakeShowtapeWindow(0);
			return;
		case 32:
			base.GetComponent<UI_WindowMaker>().MakeShowtapeWindow(1);
			return;
		case 33:
			base.GetComponent<UI_WindowMaker>().MakeShowtapeWindow(-1);
			return;
		case 34:
			base.GetComponent<UI_WindowMaker>().MakeTwoWindow(this.icons[6], this.icons[5], 4, 22, 35, "Delete Bits", "Replace Audio", 30);
			return;
		case 35:
			this.creator.ReplaceShowAudio();
			break;
		default:
			return;
		}
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0001A9AE File Offset: 0x00018BAE
	public void StartNewShow(int input)
	{
		Debug.Log("Starting New Show");
		this.manager.Load();
		if (this.manager.speakerClip != null)
		{
			this.SwitchWindow(input);
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0001A9DF File Offset: 0x00018BDF
	public void CharacterCustomMenu(int input)
	{
		base.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(this.characters, input);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0001A9F3 File Offset: 0x00018BF3
	public void StageCustomMenu()
	{
		base.GetComponent<UI_WindowMaker>().MakeStageCustomizeWindow(this.stages, this.currentStage);
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0001AA0C File Offset: 0x00018C0C
	public void CostumeUp(int input)
	{
		if (this.characters[input].currentCostume > -1)
		{
			this.characters[input].currentCostume--;
			this.RecreateAllCharacters(this.characters[input].characterName);
			base.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(this.characters, input);
		}
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0001AA64 File Offset: 0x00018C64
	public void CostumeDown(int input)
	{
		if (this.characters[input].currentCostume < this.characters[input].allCostumes.Length - 1)
		{
			this.characters[input].currentCostume++;
			this.RecreateAllCharacters(this.characters[input].characterName);
			base.GetComponent<UI_WindowMaker>().MakeCharacterCustomizeWindow(this.characters, input);
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0001AACC File Offset: 0x00018CCC
	public void StageUp(int input)
	{
		if (input > 0)
		{
			this.currentStage--;
			for (int i = 0; i < this.stages.Length; i++)
			{
				if (i != this.currentStage)
				{
					if (this.stages[i].stage.activeSelf)
					{
						this.stages[i].stage.SetActive(false);
					}
				}
				else if (!this.stages[i].stage.activeSelf)
				{
					this.stages[i].stage.SetActive(true);
				}
			}
			this.RecreateAllCharacters("");
			this.SwapCheck();
			base.GetComponent<UI_WindowMaker>().MakeStageCustomizeWindow(this.stages, this.currentStage);
		}
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0001AB84 File Offset: 0x00018D84
	public void StageDown(int input)
	{
		if (input < this.stages.Length - 1)
		{
			this.currentStage++;
			for (int i = 0; i < this.stages.Length; i++)
			{
				if (i != this.currentStage)
				{
					if (this.stages[i].stage.activeSelf)
					{
						this.stages[i].stage.SetActive(false);
					}
				}
				else if (!this.stages[i].stage.activeSelf)
				{
					this.stages[i].stage.SetActive(true);
				}
			}
			this.RecreateAllCharacters("");
			this.SwapCheck();
			base.GetComponent<UI_WindowMaker>().MakeStageCustomizeWindow(this.stages, this.currentStage);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x0001AC44 File Offset: 0x00018E44
	public void RecreateAllCharacters(string singleCharacter)
	{
		if (singleCharacter == "")
		{
			foreach (object obj in this.characterHolder.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
			int num = 0;
			for (int i = 0; i < this.stages[this.currentStage].stageCharacters.Length; i++)
			{
				for (int j = 0; j < this.characters.Length; j++)
				{
					if (this.stages[this.currentStage].stageCharacters[i].characterName == this.characters[j].characterName && this.characters[j].currentCostume != -1)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.characters[j].mainCharacter);
						gameObject.name = this.characters[j].characterName;
						gameObject.transform.parent = this.characterHolder.transform;
						gameObject.transform.localPosition = this.stages[this.currentStage].stageCharacters[i].characterPos + this.characters[j].allCostumes[this.characters[j].currentCostume].offsetPos;
						gameObject.transform.localRotation = Quaternion.Euler(this.stages[this.currentStage].stageCharacters[i].characterRot);
						gameObject.transform.GetChild(0).GetComponent<Character_Valves>().mackValves = this.mackValves;
						gameObject.transform.GetChild(0).GetComponent<Character_Valves>().StartUp();
						num++;
						foreach (object obj2 in gameObject.transform.GetChild(0).transform)
						{
							Transform transform = (Transform)obj2;
							if (!(transform.gameObject.name == this.characters[j].allCostumes[this.characters[j].currentCostume].costumeName) && transform.gameObject.name != "Armature")
							{
								UnityEngine.Object.Destroy(transform.gameObject);
							}
						}
					}
				}
			}
		}
		else
		{
			foreach (object obj3 in this.characterHolder.transform)
			{
				Transform transform2 = (Transform)obj3;
				if (transform2.name == singleCharacter)
				{
					UnityEngine.Object.Destroy(transform2.gameObject);
				}
			}
			for (int k = 0; k < this.characters.Length; k++)
			{
				if (singleCharacter == this.characters[k].characterName)
				{
					bool[] array = new bool[this.stages[this.currentStage].stageCharacters.Length];
					for (int l = 0; l < this.stages[this.currentStage].stageCharacters.Length; l++)
					{
						if (this.stages[this.currentStage].stageCharacters[l].characterName == singleCharacter)
						{
							array[l] = true;
						}
					}
					for (int m = 0; m < array.Length; m++)
					{
						if (this.characters[k].currentCostume != -1 && array[m])
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.characters[k].mainCharacter);
							gameObject2.name = this.characters[k].characterName;
							gameObject2.transform.parent = this.characterHolder.transform;
							gameObject2.transform.GetChild(0).GetComponent<Character_Valves>().mackValves = this.mackValves;
							gameObject2.transform.localPosition = this.stages[this.currentStage].stageCharacters[m].characterPos;
							gameObject2.transform.localRotation = Quaternion.Euler(this.stages[this.currentStage].stageCharacters[m].characterRot);
							gameObject2.transform.GetChild(0).GetComponent<Character_Valves>().StartUp();
							foreach (object obj4 in gameObject2.transform.GetChild(0).transform)
							{
								Transform transform3 = (Transform)obj4;
								if (!(transform3.gameObject.name == this.characters[k].allCostumes[this.characters[k].currentCostume].costumeName) && transform3.gameObject.name != "Armature")
								{
									UnityEngine.Object.Destroy(transform3.gameObject);
								}
							}
						}
					}
				}
			}
		}
		this.sidePanel.FlowLoad(-1);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0001B174 File Offset: 0x00019374
	public void SwapCheck()
	{
		if (this.CharSwapCheck)
		{
			if (this.swap)
			{
				for (int i = 0; i < this.characters.Length; i++)
				{
					if (this.characters[i].characterName == "Rolfe & Earl")
					{
						this.characters[i].mainCharacter.transform.localPosition = new Vector3(0f, -100f, 0f);
					}
					if (this.characters[i].characterName == "Klunk")
					{
						for (int j = 0; j < this.stages[this.currentStage].stageCharacters.Length; j++)
						{
							if (this.stages[this.currentStage].stageCharacters[j].characterName == this.characters[i].characterName)
							{
								this.characters[i].mainCharacter.transform.localPosition = this.stages[this.currentStage].stageCharacters[j].characterPos;
								this.characters[i].mainCharacter.transform.rotation = Quaternion.Euler(this.stages[this.currentStage].stageCharacters[j].characterRot);
							}
						}
					}
				}
				return;
			}
			for (int k = 0; k < this.characters.Length; k++)
			{
				if (this.characters[k].characterName == "Rolfe & Earl")
				{
					for (int l = 0; l < this.stages[this.currentStage].stageCharacters.Length; l++)
					{
						if (this.stages[this.currentStage].stageCharacters[l].characterName == this.characters[k].characterName)
						{
							this.characters[k].mainCharacter.transform.localPosition = this.stages[this.currentStage].stageCharacters[l].characterPos;
							this.characters[k].mainCharacter.transform.rotation = Quaternion.Euler(this.stages[this.currentStage].stageCharacters[l].characterRot);
						}
					}
				}
				if (this.characters[k].characterName == "Klunk")
				{
					this.characters[k].mainCharacter.transform.localPosition = new Vector3(0f, -100f, 0f);
				}
			}
		}
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0001B3F8 File Offset: 0x000195F8
	public void RecordingGroupMenu(int input)
	{
		base.GetComponent<UI_WindowMaker>().MakeMoveTestWindow(input);
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0001B406 File Offset: 0x00019606
	private void WindowSwitchDisable(bool curtainStop)
	{
		this.manager.recordMovements = false;
		this.inputHandlercomp.valveMapping = 0;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0001B420 File Offset: 0x00019620
	public void UnloadScene(int input)
	{
		GameObject.Find("Global Controller").GetComponent<GlobalController>().LoadShowScene("");
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0001B43C File Offset: 0x0001963C
	public void loadAudio()
	{
		this.manager.referenceSpeaker.clip = this.manager.speakerClip;
		for (int i = 0; i < this.speakerL.Length; i++)
		{
			this.speakerL[i].clip = this.manager.speakerClip;
		}
		for (int j = 0; j < this.speakerR.Length; j++)
		{
			this.speakerR[j].clip = this.manager.speakerClip;
		}
		if (this.manager.videoPath != "")
		{
			if (!this.manager.useVideoAsReference)
			{
				this.videoplayer.url = this.manager.videoPath;
			}
			this.videoplayer.Play();
			this.videoplayer.Pause();
		}
		this.manager.Play(true, true);
		this.syncAudio();
		this.SwitchWindow(17);
		this.playMultiText.GetComponent<PlayMenuManager>().TextUpdate(false);
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001B538 File Offset: 0x00019738
	public void syncAudio()
	{
		if (!this.manager.useVideoAsReference)
		{
			if (this.manager.videoPath != "")
			{
				this.videoplayer.time = (double)this.manager.referenceSpeaker.time;
			}
			for (int i = 0; i < this.speakerL.Length; i++)
			{
				this.speakerL[i].time = this.manager.referenceSpeaker.time;
			}
			for (int j = 0; j < this.speakerR.Length; j++)
			{
				this.speakerR[j].time = this.manager.referenceSpeaker.time;
			}
		}
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0001B5E8 File Offset: 0x000197E8
	public void Stop()
	{
		this.manager.referenceSpeaker.time = 0f;
		this.manager.referenceVideo.time = 0.0;
		this.manager.Play(true, false);
		this.SwitchWindow(2);
		for (int i = 0; i < this.stages[this.currentStage].curtainValves.curtainbools.Length; i++)
		{
			this.stages[this.currentStage].curtainValves.curtainbools[i] = false;
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0001B678 File Offset: 0x00019878
	public void pauseSong()
	{
		if ((this.manager.useVideoAsReference && this.manager.referenceVideo.isPlaying) || (!this.manager.useVideoAsReference && this.manager.referenceSpeaker.isPlaying))
		{
			this.AVPause();
			return;
		}
		this.AVPlay();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0001B6D0 File Offset: 0x000198D0
	public void SpecialSaveAs(int input)
	{
		if (this.creator.SaveRecordingAs())
		{
			this.SwitchWindow(input);
			TutorialStart tutorialStart = UnityEngine.Object.FindObjectOfType<TutorialStart>();
			if (tutorialStart != null)
			{
				Debug.Log("tut advance");
				tutorialStart.AttemptAdvanceTutorial("Create WAV");
			}
		}
		if (input == 11)
		{
			this.SwitchWindow(input);
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0001B724 File Offset: 0x00019924
	public void AVPause()
	{
		Debug.Log("Audio Video Pause");
		if (this.manager.videoPath != "")
		{
			this.videoplayer.Pause();
		}
		this.manager.referenceSpeaker.Pause();
		for (int i = 0; i < this.speakerL.Length; i++)
		{
			this.speakerL[i].Pause();
		}
		for (int j = 0; j < this.speakerR.Length; j++)
		{
			this.speakerR[j].Pause();
		}
		this.syncAudio();
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001B7B4 File Offset: 0x000199B4
	public void AVPlay()
	{
		Debug.Log("Audio Video Pause");
		if (this.manager.videoPath != "")
		{
			this.videoplayer.Play();
		}
		this.manager.referenceSpeaker.Play();
		for (int i = 0; i < this.speakerL.Length; i++)
		{
			this.speakerL[i].Play();
		}
		for (int j = 0; j < this.speakerR.Length; j++)
		{
			this.speakerR[j].Play();
		}
		this.syncAudio();
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0001B844 File Offset: 0x00019A44
	public void FFSong(int input)
	{
		if (input == -1)
		{
			this.PitchBackward();
		}
		else if (input == 0)
		{
			this.manager.referenceSpeaker.pitch = 1f;
		}
		else
		{
			this.PitchForward();
		}
		for (int i = 0; i < this.speakerL.Length; i++)
		{
			this.speakerL[i].pitch = this.manager.referenceSpeaker.pitch;
		}
		for (int j = 0; j < this.speakerR.Length; j++)
		{
			this.speakerR[j].pitch = this.manager.referenceSpeaker.pitch;
		}
		if (this.manager.videoPath != "")
		{
			this.videoplayer.playbackSpeed = this.manager.referenceSpeaker.pitch;
		}
		this.syncAudio();
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0001B918 File Offset: 0x00019B18
	public void PitchForward()
	{
		if (!this.manager.playMovements)
		{
			this.manager.referenceSpeaker.pitch = 0f;
			this.manager.Play(true, true);
		}
		float pitch = this.manager.referenceSpeaker.pitch;
		if (pitch <= -0.5f)
		{
			if (pitch <= -5f)
			{
				if (pitch != -100f)
				{
					if (pitch != -10f)
					{
						if (pitch == -5f)
						{
							this.manager.referenceSpeaker.pitch = -2f;
						}
					}
					else
					{
						this.manager.referenceSpeaker.pitch = -5f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = -10f;
				}
			}
			else if (pitch != -2f)
			{
				if (pitch != -1f)
				{
					if (pitch == -0.5f)
					{
						this.manager.referenceSpeaker.pitch = 0.5f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = -0.5f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = -1f;
			}
		}
		else if (pitch <= 1f)
		{
			if (pitch != 0f)
			{
				if (pitch != 0.5f)
				{
					if (pitch == 1f)
					{
						this.manager.referenceSpeaker.pitch = 2f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = 1f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = 0.5f;
			}
		}
		else if (pitch != 2f)
		{
			if (pitch != 5f)
			{
				if (pitch == 10f)
				{
					this.manager.referenceSpeaker.pitch = 100f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = 10f;
			}
		}
		else
		{
			this.manager.referenceSpeaker.pitch = 5f;
		}
		this.manager.syncTvsAndSpeakers.Invoke();
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001BB4C File Offset: 0x00019D4C
	public void PitchBackward()
	{
		if (!this.manager.playMovements)
		{
			this.manager.referenceSpeaker.pitch = 0f;
			this.manager.Play(true, true);
		}
		float pitch = this.manager.referenceSpeaker.pitch;
		if (pitch <= 0f)
		{
			if (pitch <= -2f)
			{
				if (pitch != -10f)
				{
					if (pitch != -5f)
					{
						if (pitch == -2f)
						{
							this.manager.referenceSpeaker.pitch = -5f;
						}
					}
					else
					{
						this.manager.referenceSpeaker.pitch = -10f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = -100f;
				}
			}
			else if (pitch != -1f)
			{
				if (pitch != -0.5f)
				{
					if (pitch == 0f)
					{
						this.manager.referenceSpeaker.pitch = -0.5f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = -1f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = -2f;
			}
		}
		else if (pitch <= 2f)
		{
			if (pitch != 0.5f)
			{
				if (pitch != 1f)
				{
					if (pitch == 2f)
					{
						this.manager.referenceSpeaker.pitch = 1f;
					}
				}
				else
				{
					this.manager.referenceSpeaker.pitch = 0.5f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = -0.5f;
			}
		}
		else if (pitch != 5f)
		{
			if (pitch != 10f)
			{
				if (pitch == 100f)
				{
					this.manager.referenceSpeaker.pitch = 10f;
				}
			}
			else
			{
				this.manager.referenceSpeaker.pitch = 5f;
			}
		}
		else
		{
			this.manager.referenceSpeaker.pitch = 2f;
		}
		if (this.manager.videoPath != "")
		{
			this.videoplayer.playbackSpeed = this.manager.referenceSpeaker.pitch;
		}
		this.manager.syncTvsAndSpeakers.Invoke();
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0001BDA4 File Offset: 0x00019FA4
	public void UpdateTickets()
	{
		this.ticketText.text = "x" + PlayerPrefs.GetInt("TicketCount").ToString();
	}

	// Token: 0x04000395 RID: 917
	[Header("Stage / Characters")]
	public StageSelector[] stages;

	// Token: 0x04000396 RID: 918
	[HideInInspector]
	public int currentStage;

	// Token: 0x04000397 RID: 919
	public CharacterSelector[] characters;

	// Token: 0x04000398 RID: 920
	public FloatEvent characterEvent = new FloatEvent();

	// Token: 0x04000399 RID: 921
	public bool CharSwapCheck;

	// Token: 0x0400039A RID: 922
	[HideInInspector]
	public bool swap;

	// Token: 0x0400039B RID: 923
	[Space(20f)]
	[Header("Inspector Objects")]
	public AudioSource[] speakerR;

	// Token: 0x0400039C RID: 924
	public AudioSource[] speakerL;

	// Token: 0x0400039D RID: 925
	public Sprite[] icons;

	// Token: 0x0400039E RID: 926
	public GameObject characterHolder;

	// Token: 0x0400039F RID: 927
	[HideInInspector]
	public GameObject mackValves;

	// Token: 0x040003A0 RID: 928
	[HideInInspector]
	public UI_RshwCreator creator;

	// Token: 0x040003A1 RID: 929
	public GameObject playMultiText;

	// Token: 0x040003A2 RID: 930
	public UI_SidePanel sidePanel;

	// Token: 0x040003A3 RID: 931
	public Text ffSpeed;

	// Token: 0x040003A4 RID: 932
	public Text AddSource;

	// Token: 0x040003A5 RID: 933
	public Text Uncompress;

	// Token: 0x040003A6 RID: 934
	public Text ticketText;

	// Token: 0x040003A7 RID: 935
	[HideInInspector]
	public VideoPlayer videoplayer;

	// Token: 0x040003A8 RID: 936
	[Space(20f)]
	[Header("Show Data")]
	private Mack_Valves mack;

	// Token: 0x040003A9 RID: 937
	private InputHandler inputHandlercomp;

	// Token: 0x040003AA RID: 938
	[HideInInspector]
	public GameObject thePlayer;

	// Token: 0x040003AB RID: 939
	private bool ticketCheck;

	// Token: 0x040003AC RID: 940
	private bool ticketCheck2;

	// Token: 0x040003AD RID: 941
	public UI_PlayRecord.SpotlightsOverride spotlightsOverride;

	// Token: 0x040003AE RID: 942
	public UI_PlayRecord.DimOverride dimOverride;

	// Token: 0x040003AF RID: 943
	public UI_PlayRecord.LightingOverride lightingOverride;

	// Token: 0x040003B0 RID: 944
	public UI_PlayRecord.KlunkBitsChange klunkBitsChange;

	// Token: 0x040003B1 RID: 945
	public UI_PlayRecord.ServiceLights serviceLights;

	// Token: 0x040003B2 RID: 946
	public UI_PlayRecord.SignalChange signalChange;

	// Token: 0x040003B3 RID: 947
	public UI_ShowtapeManager manager;

	// Token: 0x0200010B RID: 267
	public enum SignalChange
	{
		// Token: 0x0400074F RID: 1871
		normal,
		// Token: 0x04000750 RID: 1872
		PreCU,
		// Token: 0x04000751 RID: 1873
		PrePTT
	}

	// Token: 0x0200010C RID: 268
	public enum SpotlightsOverride
	{
		// Token: 0x04000753 RID: 1875
		normal,
		// Token: 0x04000754 RID: 1876
		RAE,
		// Token: 0x04000755 RID: 1877
		CU
	}

	// Token: 0x0200010D RID: 269
	public enum DimOverride
	{
		// Token: 0x04000757 RID: 1879
		normal,
		// Token: 0x04000758 RID: 1880
		RAE,
		// Token: 0x04000759 RID: 1881
		CU
	}

	// Token: 0x0200010E RID: 270
	public enum LightingOverride
	{
		// Token: 0x0400075B RID: 1883
		normal,
		// Token: 0x0400075C RID: 1884
		DimsOff,
		// Token: 0x0400075D RID: 1885
		DimsOn
	}

	// Token: 0x0200010F RID: 271
	public enum KlunkBitsChange
	{
		// Token: 0x0400075F RID: 1887
		normal,
		// Token: 0x04000760 RID: 1888
		Rockafire,
		// Token: 0x04000761 RID: 1889
		Dreamfactory
	}

	// Token: 0x02000110 RID: 272
	public enum ServiceLights
	{
		// Token: 0x04000763 RID: 1891
		normal,
		// Token: 0x04000764 RID: 1892
		Off,
		// Token: 0x04000765 RID: 1893
		On
	}
}
