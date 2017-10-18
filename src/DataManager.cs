using com.max.JiXiangLobby;
using System;
using System.Collections.Generic;
using UnityEngine;
public class DataManager : SingletonMono<DataManager, AllScene>
{
	public string isGlobalCanGive = "1";
	public string isRecordOperation = "0";
	public long serverTime;
	public long clientTime;
	public List<OperationRecord> curAllOperation = new List<OperationRecord>();
	public string username = string.Empty;
	public int loginType = 1;
	public string wechatId = "dwa45584115464";
	public string headUrl = "http://120.77.60.113:6528/touxiang.png";
	public Texture2D headTexture;
	public string sex = "0";
	public string nick = string.Empty;
	public double coin;
	public double diamond;
	public double safeCoin;
	public string vip = "0";
	public double signCoin = 200.0;
	public int signCount;
	public bool isCanSign;
	public bool isLockPhone;
	public double tuiGuangPeopleCount;
	public double tuiGuangLeiJiCoin;
	public double tuiGuangLeiJiDiamond;
	public string phone = string.Empty;
	public string agentname = string.Empty;
	public string accountstate = "0";
	public bool isPlaying;
	public bool isGotGiveRecord;
	public string isCanGive = "1";
	public List<Record> allGiveRecord = new List<Record>();
	public List<Record> allReceiveRecord = new List<Record>();
	public List<Notice> allNotice = new List<Notice>();
	private float bgSoundValue = 1f;
	private float effectSoundValue = 1f;
	public SiteInfo[] allSiteInfo;
	private int curSiteIndex;
	private bool isSelectInput;
	private bool isChangeRoom;
	private bool isQuitRoom;
	public float BGSound
	{
		get
		{
			return this.bgSoundValue;
		}
	}
	public float EffectSound
	{
		get
		{
			return this.effectSoundValue;
		}
	}
	public int CurSiteIndex
	{
		get
		{
			return this.curSiteIndex;
		}
		set
		{
			this.curSiteIndex = value;
		}
	}
	public bool IsSelectInput
	{
		get
		{
			return this.isSelectInput;
		}
		set
		{
			this.isSelectInput = value;
		}
	}
	public bool IsChangeRoom
	{
		get
		{
			return this.isChangeRoom;
		}
		set
		{
			this.isChangeRoom = value;
		}
	}
	public bool IsQuitRoom
	{
		get
		{
			return this.isQuitRoom;
		}
		set
		{
			this.isQuitRoom = value;
		}
	}
	protected override void Init()
	{
	}
	private void Start()
	{
		if (PlayerPrefs.HasKey("bgSoundValue"))
		{
			this.bgSoundValue = PlayerPrefs.GetFloat("bgSoundValue");
			this.effectSoundValue = PlayerPrefs.GetFloat("effectSoundValue");
		}
		else
		{
			PlayerPrefs.SetFloat("bgSoundValue", this.bgSoundValue);
			PlayerPrefs.SetFloat("effectSoundValue", this.effectSoundValue);
		}
		SoundManager.Instance.ChangeSoundValue(this.bgSoundValue, this.effectSoundValue);
	}
	protected override void Quit()
	{
	}
	public static string ChangeDanWei(double count)
	{
		string result = string.Empty;
		if (count > 10000.0)
		{
			result = Mathf.FloorToInt((float)(count / 10000.0)).ToString() + "万";
		}
		else
		{
			result = count.ToString();
		}
		return result;
	}
	public static long ConvertDateTimeToInt(DateTime time)
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
		return (time.Ticks - dateTime.Ticks) / 10000L;
	}
	public static string ConvertTimeToDate(long time)
	{
		return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds((double)time).ToString("yyyy/MM/dd HH:mm:ss:ffff");
	}
	public void SaveSoundValue(float bg, float effect)
	{
		this.bgSoundValue = bg;
		this.effectSoundValue = effect;
		PlayerPrefs.SetFloat("bgSoundValue", this.bgSoundValue);
		PlayerPrefs.SetFloat("effectSoundValue", this.effectSoundValue);
		SoundManager.Instance.ChangeSoundValue(this.bgSoundValue, this.effectSoundValue);
	}
	public void DelectOneNotice(Notice info)
	{
		for (int i = 0; i < this.allNotice.Count; i++)
		{
			if (this.allNotice[i].id == info.id)
			{
				this.allNotice.RemoveAt(i);
				break;
			}
		}
	}
	public bool IsRecordOperation()
	{
		return this.isRecordOperation.Equals("1");
	}
}
