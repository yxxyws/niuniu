using com.max.JiXiangLobby;
using NiuNiu;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NetManager : SingletonMono<NetManager, AllScene>
{
	private SmartFox smartfox;
	private string ip = "119.23.70.111";
	private int port = 9933;
	private string zone = "JiXiangLobby";
	private bool isConnect;
	private bool inZone;
	private bool inRoom;
	private float heartbeat = 10f;
	public SmartFox GetSmartFox
	{
		get
		{
			if (this.smartfox == null)
			{
				this.InitSmartFox();
			}
			return this.smartfox;
		}
	}
	public bool IsConnect
	{
		get
		{
			return this.isConnect;
		}
	}
	public bool InRoom
	{
		get
		{
			return this.inRoom;
		}
	}
	protected override void Init()
	{
		this.InitSmartFox();
	}
	private void Start()
	{
		this.heartbeat = 10f;
		base.InvokeRepeating("GetHeartbeat", 0f, 5f);
	}
	private void Update()
	{
		if (this.smartfox != null)
		{
			this.smartfox.ProcessEvents();
			if (this.inZone)
			{
				this.heartbeat -= Time.deltaTime;
				if (this.heartbeat < 0f)
				{
					this.DisConnect();
				}
			}
		}
	}
	public void InitSmartFox()
	{
		this.smartfox = new SmartFox();
		this.smartfox.AddEventListener(SFSEvent.CONNECTION, new EventListenerDelegate(this.OnConnect));
		this.smartfox.AddEventListener(SFSEvent.CONNECTION_LOST, new EventListenerDelegate(this.OnConnectionLost));
		this.smartfox.AddEventListener(SFSEvent.LOGIN, new EventListenerDelegate(this.OnLogin));
		this.smartfox.AddEventListener(SFSEvent.LOGIN_ERROR, new EventListenerDelegate(this.OnLoginError));
		this.smartfox.AddEventListener(SFSEvent.LOGOUT, new EventListenerDelegate(this.OnLogout));
		this.smartfox.AddEventListener(SFSEvent.ROOM_JOIN, new EventListenerDelegate(this.OnJoinRoom));
		this.smartfox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, new EventListenerDelegate(this.OnJoinRoomError));
		this.smartfox.AddEventListener(SFSEvent.USER_EXIT_ROOM, new EventListenerDelegate(this.OnLeaveRoom));
		this.smartfox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, new EventListenerDelegate(this.OnExtensionRespones));
	}
	public void Connect()
	{
		if (this.smartfox == null)
		{
			this.InitSmartFox();
		}
		ConfigData configData = new ConfigData();
		configData.Host = this.ip;
		configData.Port = this.port;
		configData.Zone = this.zone;
		this.smartfox.Connect(configData);
	}
	private void GetHeartbeat()
	{
		if (this.smartfox != null)
		{
			SFSObject sFSObject = new SFSObject();
			if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation() && SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Count > 0)
			{
				List<string> list = new List<string>();
				List<long> list2 = new List<long>();
				for (int i = 0; i < SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Count; i++)
				{
					list.Add(SingletonMono<DataManager, AllScene>.Instance.curAllOperation[i].Operator);
					list2.Add(SingletonMono<DataManager, AllScene>.Instance.curAllOperation[i].OperationTime);
				}
				sFSObject.PutUtfStringArray("operationArr", list.ToArray());
				sFSObject.PutLongArray("operationTimeArr", list2.ToArray());
			}
			SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Clear();
			if (this.inRoom)
			{
				this.smartfox.Send(new ExtensionRequest("CR.GetHeartbeat", sFSObject, this.smartfox.LastJoinedRoom));
			}
			else
			{
				if (this.inZone)
				{
					this.smartfox.Send(new ExtensionRequest("CR.GetHeartbeat", sFSObject));
				}
			}
		}
	}
	public void DisConnect()
	{
		if (this.smartfox != null)
		{
			this.smartfox.Disconnect();
		}
	}
	public void Login()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("platform", 1);
		sFSObject.PutInt("loginType", SingletonMono<DataManager, AllScene>.Instance.loginType);
		if (SingletonMono<DataManager, AllScene>.Instance.loginType == 0)
		{
			sFSObject.PutUtfString("openid", SingletonMono<DataManager, AllScene>.Instance.wechatId);
			sFSObject.PutUtfString("nick", SingletonMono<DataManager, AllScene>.Instance.nick);
			sFSObject.PutUtfString("sex", SingletonMono<DataManager, AllScene>.Instance.sex);
			sFSObject.PutUtfString("headurl", SingletonMono<DataManager, AllScene>.Instance.headUrl);
			this.smartfox.Send(new LoginRequest(SingletonMono<DataManager, AllScene>.Instance.wechatId, string.Empty, this.zone, sFSObject));
		}
		else
		{
			if (SingletonMono<DataManager, AllScene>.Instance.loginType == 1)
			{
				sFSObject.PutUtfString("usernum", SingletonMono<DataManager, AllScene>.Instance.username);
				this.smartfox.Send(new LoginRequest(SingletonMono<DataManager, AllScene>.Instance.username, string.Empty, this.zone, sFSObject));
			}
			else
			{
				Debug.LogError("请检查当前第三方登录的类型, 创建其登录......");
			}
		}
	}
	public void Logout()
	{
		this.smartfox.Send(new LogoutRequest());
	}
	public void SendSignInfo()
	{
		this.smartfox.Send(new ExtensionRequest("CR.Sign", new SFSObject()));
	}
	public void SendDaiLiInfo(string str)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("agentname", str);
		this.smartfox.Send(new ExtensionRequest("CR.DaiLi", sFSObject));
	}
	public void SendGetTaskInfo()
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutInt("signCount", SingletonMono<DataManager, AllScene>.Instance.signCount);
		this.smartfox.Send(new ExtensionRequest("CR.GetTaskInfo", sFSObject));
	}
	public void SendNoticeText(string str)
	{
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutUtfString("noticeText", str);
		this.smartfox.Send(new ExtensionRequest("CR.SendNoticeText", iSFSObject));
	}
	public void SendSaveSafeCoin(double offsetCoin, double offsetSafeCoin)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutDouble("offsetCoin", offsetCoin);
		sFSObject.PutDouble("offsetSafeCoin", offsetSafeCoin);
		this.smartfox.Send(new ExtensionRequest("CR.SafeInfo", sFSObject));
	}
	public void SendGetGiveRecord()
	{
		this.smartfox.Send(new ExtensionRequest("CR.GetGiveRecord", new SFSObject()));
	}
	public void SendGiveCoinIdGetNick(string id, double giveCoin)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("id", id);
		sFSObject.PutDouble("giveCoin", giveCoin);
		this.smartfox.Send(new ExtensionRequest("CR.GiveCoinIdGetNick", sFSObject));
	}
	public void SendIdGiveInfo(string id, string nick, double giveCoin)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("id", id);
		sFSObject.PutUtfString("nick", nick);
		sFSObject.PutDouble("giveCoin", giveCoin);
		this.smartfox.Send(new ExtensionRequest("CR.IdGive", sFSObject));
	}
	public void SendOpenSafeClick()
	{
		SFSObject parameters = new SFSObject();
		this.smartfox.Send(new ExtensionRequest("CR.OpenSafe", parameters));
	}
	public void SendGetRechargeRecord(string startTime, string endTime)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("usernum", SingletonMono<DataManager, AllScene>.Instance.username);
		sFSObject.PutUtfString("earlytime", startTime);
		sFSObject.PutUtfString("latetime", endTime);
		this.smartfox.Send(new ExtensionRequest("CR.GetRechargeRecord", sFSObject));
	}
	public void SendSearchUserInfo(string id)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("id", id);
		this.smartfox.Send(new ExtensionRequest("CR.SearchUserInfo", sFSObject));
	}
	public void SendGetBuddyList()
	{
		this.smartfox.Send(new ExtensionRequest("CR.GetBuddyList", new SFSObject()));
	}
	public void SendAddFriend(string addId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("addId", addId);
		this.smartfox.Send(new ExtensionRequest("CR.AddFriend", sFSObject));
	}
	public void SendAgreeAddFriend(string agreeAddId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("selfId", SingletonMono<DataManager, AllScene>.Instance.username);
		sFSObject.PutUtfString("agreeAddId", agreeAddId);
		this.smartfox.Send(new ExtensionRequest("CR.AgreeAdd", sFSObject));
	}
	public void SendRefuseAddFriend(string delectId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("selfId", SingletonMono<DataManager, AllScene>.Instance.username);
		sFSObject.PutUtfString("delectId", delectId);
		this.smartfox.Send(new ExtensionRequest("CR.RefuseAdd", sFSObject));
	}
	public void SendDelectFriend(string delectId)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("selfId", SingletonMono<DataManager, AllScene>.Instance.username);
		sFSObject.PutUtfString("delectId", delectId);
		this.smartfox.Send(new ExtensionRequest("CR.DelectFriend", sFSObject));
	}
	public void SendChatText(string receiveId, string str)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("sendId", SingletonMono<DataManager, AllScene>.Instance.username);
		sFSObject.PutUtfString("receiveId", receiveId);
		sFSObject.PutUtfString("chatText", str);
		this.smartfox.Send(new ExtensionRequest("CR.Chat", sFSObject));
	}
	public void SendJoinRoom()
	{
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutInt("index", SingletonMono<DataManager, AllScene>.Instance.CurSiteIndex);
		iSFSObject.PutBool("isChangeRoom", SingletonMono<DataManager, AllScene>.Instance.IsChangeRoom);
		this.smartfox.Send(new ExtensionRequest("CR.JoinNiuNiuGame", iSFSObject));
	}
	private void OnConnect(BaseEvent evt)
	{
		if ((bool)evt.Params["success"])
		{
			this.isConnect = true;
			this.Login();
		}
		else
		{
			this.smartfox = null;
			TipManager.Instance.ShowTipsBar("连接失败,请检查网络", null);
		}
	}
	private void OnConnectionLost(BaseEvent evt)
	{
		this.smartfox = null;
		if (SingletonMono<DataManager, AllScene>.Instance.accountstate.Equals("1"))
		{
			TipManager.Instance.ShowTipsBar("您当前登录的帐号已被管理员封停,请联系客服了解相关情况！", null);
		}
		else
		{
			TipManager.Instance.ShowTipsBar("与服务器断开连接...", null);
		}
		Debug.Log("失去连接...");
	}
	private void OnLogin(BaseEvent evt)
	{
		this.inZone = true;
	}
	private void OnLoginError(BaseEvent evt)
	{
	}
	private void OnLogout(BaseEvent evt)
	{
		this.inZone = false;
		this.inRoom = false;
		if (SingletonMono<DataManager, AllScene>.Instance.loginType == 0)
		{
			SingletonMono<DataManager, AllScene>.Instance.loginType = 1;
			this.Login();
		}
		else
		{
			if (SingletonMono<DataManager, AllScene>.Instance.loginType == 1)
			{
				SettingManager.Instance.ChangeUserToReLoginPanel();
			}
		}
	}
	private void OnJoinRoom(BaseEvent evt)
	{
		this.inRoom = true;
		SingletonMono<DataManager, AllScene>.Instance.IsChangeRoom = false;
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
		SingletonMono<Main, AllScene>.Instance.SetGameStates(LobbyState.PlayGame);
	}
	private void OnJoinRoomError(BaseEvent evt)
	{
		this.inRoom = false;
		if (SingletonMono<Main, AllScene>.Instance.cutState != LobbyState.PlayGame)
		{
			TipManager.Instance.ShowTips("加入房间失败...", 2f);
			TipManager.Instance.HideWaitTip();
		}
	}
	private void OnLeaveRoom(BaseEvent evt)
	{
		User user = (User)evt.Params["user"];
		if (user.Name == SingletonMono<DataManager, AllScene>.Instance.username && SingletonMono<DataManager, AllScene>.Instance.IsQuitRoom)
		{
			this.inRoom = false;
			SingletonMono<DataManager, AllScene>.Instance.IsQuitRoom = false;
			SingletonMono<Main, AllScene>.Instance.cutState = LobbyState.GameSelect;
			SceneManager.LoadSceneAsync(0);
			TipManager.Instance.HideWaitTip();
			FriendManager.Instance.friendBar.UpdateFriendTip();
			if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
			{
				SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("lobby"));
			}
		}
	}
	private void OnExtensionRespones(BaseEvent evt)
	{
		string text = (string)evt.Params["cmd"];
		ISFSObject sfsObj = (SFSObject)evt.Params["params"];
		switch (text)
		{
		case "Msg":
			this.OnMsgMessageHandle(sfsObj.GetByteArray("data").Bytes);
			break;
		case "GetHeartbeat":
			this.heartbeat = (float)sfsObj.GetInt("data");
			break;
		case "username":
			SingletonMono<DataManager, AllScene>.Instance.username = sfsObj.GetUtfString("username");
			this.smartfox.Send(new LogoutRequest());
			PlayerPrefs.SetString("username", SingletonMono<DataManager, AllScene>.Instance.username);
			PlayerPrefs.SetString("nick", SingletonMono<DataManager, AllScene>.Instance.nick);
			PlayerPrefs.SetString("sex", SingletonMono<DataManager, AllScene>.Instance.sex);
			PlayerPrefs.SetString("headUrl", SingletonMono<DataManager, AllScene>.Instance.headUrl);
			break;
		case "LoginSuccess":
			if ((sfsObj.GetBool("isRepair") && sfsObj.GetBool("IsRepairUser")) || !sfsObj.GetBool("isRepair"))
			{
				if (sfsObj.GetUtfString("Version") == SingletonMono<Main, AllScene>.Instance.AppVer)
				{
					SingletonMono<DataManager, AllScene>.Instance.headUrl = sfsObj.GetUtfString("headUrl");
					SingletonMono<DataManager, AllScene>.Instance.isPlaying = sfsObj.GetBool("isPlaying");
					SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
					SingletonMono<DataManager, AllScene>.Instance.diamond = sfsObj.GetDouble("diamond");
					SingletonMono<DataManager, AllScene>.Instance.vip = sfsObj.GetUtfString("vip");
					SingletonMono<DataManager, AllScene>.Instance.safeCoin = sfsObj.GetDouble("safeCoin");
					SingletonMono<DataManager, AllScene>.Instance.signCount = sfsObj.GetInt("signCount");
					SingletonMono<DataManager, AllScene>.Instance.isCanSign = sfsObj.GetBool("isCanSign");
					SingletonMono<DataManager, AllScene>.Instance.tuiGuangPeopleCount = sfsObj.GetDouble("tuiGuangPeopleCount");
					SingletonMono<DataManager, AllScene>.Instance.tuiGuangLeiJiCoin = sfsObj.GetDouble("tuiGuangLeiJiCoin");
					SingletonMono<DataManager, AllScene>.Instance.tuiGuangLeiJiDiamond = sfsObj.GetDouble("tuiGuangLeiJiDiamond");
					SingletonMono<DataManager, AllScene>.Instance.phone = sfsObj.GetUtfString("phone");
					SingletonMono<DataManager, AllScene>.Instance.agentname = sfsObj.GetUtfString("agentname");
					SingletonMono<DataManager, AllScene>.Instance.accountstate = sfsObj.GetUtfString("accountstate");
					SingletonMono<DataManager, AllScene>.Instance.isCanGive = sfsObj.GetUtfString("isCanGive");
					SingletonMono<DataManager, AllScene>.Instance.isGlobalCanGive = sfsObj.GetUtfString("isGlobalCanGive");
					SingletonMono<DataManager, AllScene>.Instance.isRecordOperation = sfsObj.GetUtfString("isRecordOperation");
					SingletonMono<DataManager, AllScene>.Instance.serverTime = sfsObj.GetLong("serverTime");
					SingletonMono<DataManager, AllScene>.Instance.clientTime = DataManager.ConvertDateTimeToInt(DateTime.Now);
					if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
					{
						SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("login"));
					}
					if (SingletonMono<DataManager, AllScene>.Instance.accountstate.Equals("1"))
					{
						TipManager.Instance.ShowTipsBar("您当前登录的帐号已被管理员封停,请联系客服了解相关情况！", null);
					}
					else
					{
						byte[] bytes = sfsObj.GetByteArray("siteInfo").Bytes;
						this.OnMsgMessageHandle(bytes);
						byte[] bytes2 = sfsObj.GetByteArray("noticeInfo").Bytes;
						this.OnMsgMessageHandle(bytes2);
						byte[] bytes3 = sfsObj.GetByteArray("allFriend").Bytes;
						this.OnMsgMessageHandle(bytes3);
						byte[] bytes4 = sfsObj.GetByteArray("message").Bytes;
						this.OnMsgMessageHandle(bytes4);
						PlayerPrefs.SetString("AppVer", SingletonMono<Main, AllScene>.Instance.AppVer);
						TipManager.Instance.HideWaitTip();
						SingletonMono<Main, AllScene>.Instance.SetGameStates(LobbyState.GameSelect);
					}
				}
				else
				{
					TipManager.Instance.ShowTipsBar("请更新最新版本", delegate
					{
						Application.OpenURL(sfsObj.GetUtfString("AppUrl"));
						Application.Quit();
					});
				}
			}
			else
			{
				if (sfsObj.GetUtfString("isRepairReason") == string.Empty)
				{
					TipManager.Instance.ShowTipsBar("系统正在维护中,请稍后...", null);
				}
				else
				{
					TipManager.Instance.ShowTipsBar(sfsObj.GetUtfString("isRepairReason"), null);
				}
			}
			break;
		case "DaiLiFeedback":
			TipManager.Instance.ShowTips(sfsObj.GetUtfString("msg"), 2f);
			if (sfsObj.GetUtfString("state").Equals("true"))
			{
				UIManager.Instance.IsShowDaiLiBar(false);
			}
			break;
		case "OpenSafe":
			SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
			UIManager.Instance.UpdateUserInfoShow();
			if (sfsObj.GetBool("isSuccess"))
			{
				UIManager.Instance.ShowSafeBar();
			}
			else
			{
				TipManager.Instance.HideWaitTip();
				TipManager.Instance.ShowTips("你正在游戏中，不可操作保险箱...", 3f);
			}
			break;
		case "NoticeText":
			FloatNoticeManager.Instance.ShowNotice(sfsObj.GetUtfString("noticeText"));
			break;
		case "TaskInfo":
			TipManager.Instance.HideWaitTip();
			SingletonMono<DataManager, AllScene>.Instance.signCoin = sfsObj.GetDouble("signCoin");
			UIManager.Instance.ShowTaskBar();
			break;
		case "UpdateCoin":
			SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
			UIManager.Instance.UpdateUserInfoShow();
			break;
		case "IsPlaying":
			SingletonMono<DataManager, AllScene>.Instance.isPlaying = sfsObj.GetBool("isPlaying");
			break;
		case "SaveSafeFail":
			TipManager.Instance.HideWaitTip();
			TipManager.Instance.ShowTipsBar("数据异常", null);
			break;
		case "SaveSafeSuccess":
			TipManager.Instance.HideWaitTip();
			SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
			SingletonMono<DataManager, AllScene>.Instance.safeCoin = sfsObj.GetDouble("safeCoin");
			if (sfsObj.GetDouble("offsetCoin") < 0.0)
			{
				TipManager.Instance.ShowTips("你存入了" + sfsObj.GetDouble("offsetSafeCoin") + "金币", 2f);
			}
			else
			{
				TipManager.Instance.ShowTips("你取出了" + sfsObj.GetDouble("offsetCoin") + "金币", 2f);
			}
			UIManager.Instance.RefreshSafeShow();
			UIManager.Instance.UpdateUserInfoShow();
			break;
		case "GiveGetUserInfoFail":
			TipManager.Instance.HideWaitTip();
			TipManager.Instance.ShowFeedbackTipsBar("请输入正确的玩家Id");
			break;
		case "GiveGetUserInfo":
		{
			TipManager.Instance.HideWaitTip();
			double giveCoin = sfsObj.GetDouble("giveCoin");
			TipManager.Instance.ShowConfirmBar("你确定要赠送" + DataManager.ChangeDanWei(giveCoin) + "金币给玩家：" + sfsObj.GetUtfString("nick"), delegate
			{
				SingletonMono<NetManager, AllScene>.Instance.SendIdGiveInfo(sfsObj.GetUtfString("usernum"), sfsObj.GetUtfString("nick"), giveCoin);
				TipManager.Instance.ShowWaitTip(string.Empty);
			}, delegate
			{
				UIManager.Instance.UpdateUserInfoShow();
			});
			break;
		}
		case "GiveCoinFail":
			TipManager.Instance.HideWaitTip();
			TipManager.Instance.ShowTipsBar("数据异常", null);
			break;
		case "GiveCoinSuccess":
		{
			TipManager.Instance.HideWaitTip();
			TipManager.Instance.ShowTips("赠送成功...", 2f);
			SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
			UIManager.Instance.UpdateUserInfoShow();
			Record record = new Record();
			record.username = sfsObj.GetUtfString("id");
			record.nick = sfsObj.GetUtfString("nick");
			record.coin = sfsObj.GetDouble("giveCoin");
			record.time = sfsObj.GetUtfString("time");
			SingletonMono<DataManager, AllScene>.Instance.allGiveRecord.Add(record);
			UIManager.Instance.AddGiveRecord(record);
			break;
		}
		case "ReceiveGift":
		{
			SingletonMono<DataManager, AllScene>.Instance.safeCoin += sfsObj.GetDouble("giveCoin");
			Record record2 = new Record();
			if (SingletonMono<DataManager, AllScene>.Instance.isGotGiveRecord)
			{
				record2.username = sfsObj.GetUtfString("giveId");
				record2.nick = sfsObj.GetUtfString("giveNick");
				record2.coin = sfsObj.GetDouble("giveCoin");
				record2.time = sfsObj.GetUtfString("time");
				SingletonMono<DataManager, AllScene>.Instance.allReceiveRecord.Add(record2);
			}
			TipManager.Instance.ShowTips(string.Concat(new string[]
			{
				"你收到",
				sfsObj.GetUtfString("giveNick"),
				"赠送的",
				DataManager.ChangeDanWei(sfsObj.GetDouble("giveCoin")),
				"金币,注意查看保险箱"
			}), 2f);
			if (SingletonMono<Main, AllScene>.Instance.cutState != LobbyState.PlayGame)
			{
				if (UIManager.Instance.RefreshSafeShow != null)
				{
					UIManager.Instance.RefreshSafeShow();
				}
				if (SingletonMono<DataManager, AllScene>.Instance.isGotGiveRecord)
				{
					UIManager.Instance.AddReceiveRecord(record2);
				}
			}
			break;
		}
		case "SignSuccess":
			SingletonMono<DataManager, AllScene>.Instance.coin = sfsObj.GetDouble("coin");
			SingletonMono<DataManager, AllScene>.Instance.signCoin = sfsObj.GetDouble("signCoin");
			SingletonMono<DataManager, AllScene>.Instance.signCount = sfsObj.GetInt("signCount");
			SingletonMono<DataManager, AllScene>.Instance.isCanSign = false;
			UIManager.Instance.UpdateSignShow();
			TipManager.Instance.HideWaitTip();
			UIManager.Instance.UpdateUserInfoShow();
			break;
		case "RechargeMessage":
		{
			double @double = sfsObj.GetDouble("rechargeCoin");
			SingletonMono<DataManager, AllScene>.Instance.safeCoin += @double;
			if (SingletonMono<Main, AllScene>.Instance.cutState != LobbyState.PlayGame && UIManager.Instance.RefreshSafeShow != null)
			{
				UIManager.Instance.RefreshSafeShow();
			}
			TipManager.Instance.ShowTips("您充值的" + @double + "金币以存入保险箱,请注意查收", 2f);
			break;
		}
		case "UserFengTing":
			SingletonMono<DataManager, AllScene>.Instance.accountstate = "1";
			this.smartfox.Disconnect();
			break;
		case "ChangeVip":
			SingletonMono<DataManager, AllScene>.Instance.vip = sfsObj.GetUtfString("vip");
			if (SingletonMono<Main, AllScene>.Instance.cutState != LobbyState.PlayGame && UIManager.Instance.UpdateUserInfoShow != null)
			{
				UIManager.Instance.UpdateUserInfoShow();
			}
			break;
		case "ChangeGlobalCanGive":
			SingletonMono<DataManager, AllScene>.Instance.isGlobalCanGive = sfsObj.GetUtfString("isGlobalCanGive");
			break;
		case "ChangeUserIsCanGive":
			SingletonMono<DataManager, AllScene>.Instance.isCanGive = sfsObj.GetUtfString("isCanGive");
			break;
		case "NiuNiuData":
			if (NiuNiuNetManager.Instance != null)
			{
				NiuNiuNetManager.Instance.OnExtensionRespones(sfsObj);
			}
			break;
		}
	}
	public void OnMsgMessageHandle(byte[] data)
	{
		JiXiangLobbyProtobufSerializer jiXiangLobbyProtobufSerializer = new JiXiangLobbyProtobufSerializer();
		MemoryStream source = new MemoryStream(data);
		Msg msg = jiXiangLobbyProtobufSerializer.Deserialize(source, null, typeof(Msg)) as Msg;
		switch (msg.msg_type)
		{
		case Msg.MsgType.Friend:
			TipManager.Instance.HideWaitTip();
			FriendManager.Instance.ShowFriendBar(msg.allFriend);
			break;
		case Msg.MsgType.SiteInfo:
			SingletonMono<DataManager, AllScene>.Instance.allSiteInfo = msg.allSiteInfo.ToArray();
			break;
		case Msg.MsgType.Record:
			SingletonMono<DataManager, AllScene>.Instance.allGiveRecord = msg.allGiveRecord;
			SingletonMono<DataManager, AllScene>.Instance.allReceiveRecord = msg.allReceiveRecord;
			SingletonMono<DataManager, AllScene>.Instance.isGotGiveRecord = true;
			UIManager.Instance.InitAllRecord();
			break;
		case Msg.MsgType.SearchUserInfo:
			TipManager.Instance.HideWaitTip();
			if (msg.isExistUser)
			{
				FriendManager.Instance.ShowAddFriendBar(msg.allFriend[0]);
			}
			else
			{
				TipManager.Instance.ShowTipsBar("不存在您输入的ID用户,请确认您要搜索的ID", delegate
				{
					TipManager.Instance.HideTipBar();
				});
			}
			break;
		case Msg.MsgType.UpdateFriend:
			FriendManager.Instance.friendBar.UpdateFriendShow(msg.allFriend);
			break;
		case Msg.MsgType.RemoveFriend:
			FriendManager.Instance.friendBar.RemoveFriend(msg.allFriend);
			break;
		case Msg.MsgType.ChatText:
			for (int i = 0; i < msg.allChatText.Count; i++)
			{
				if (msg.allChatText[i].sendId == SingletonMono<DataManager, AllScene>.Instance.username)
				{
					FriendManager.Instance.AddFriendChatText(msg.allChatText[i].receiveId, msg.allChatText[i]);
				}
				else
				{
					FriendManager.Instance.AddFriendChatText(msg.allChatText[i].sendId, msg.allChatText[i]);
				}
			}
			break;
		case Msg.MsgType.RechargeRecord:
			if (msg.isExistRechargeRecord)
			{
				UIManager.Instance.LoadingRechargeRecord(msg.allGiveRecord);
			}
			else
			{
				TipManager.Instance.HideWaitTip();
				TipManager.Instance.ShowTips("该时间段内无任何充值记录", 2f);
			}
			break;
		case Msg.MsgType.Notice:
			for (int j = 0; j < msg.allNotice.Count; j++)
			{
				if (msg.allNotice[j].isDelect)
				{
					SingletonMono<DataManager, AllScene>.Instance.DelectOneNotice(msg.allNotice[j]);
				}
				else
				{
					SingletonMono<DataManager, AllScene>.Instance.allNotice.Add(msg.allNotice[j]);
				}
			}
			break;
		}
	}
	protected override void Quit()
	{
		this.DisConnect();
	}
}
