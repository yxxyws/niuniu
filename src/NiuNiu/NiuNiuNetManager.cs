using com.max.JiXiangNiuNiu;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Util;
using System;
using System.IO;
using UnityEngine;
namespace NiuNiu
{
	public class NiuNiuNetManager : MonoBehaviour
	{
		public static NiuNiuNetManager Instance;
		private Room CurRoom
		{
			get
			{
				return SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.LastJoinedRoom;
			}
		}
		private void Awake()
		{
			NiuNiuNetManager.Instance = this;
		}
		private ISFSObject GetSendMsg(Msg msg)
		{
			ISFSObject iSFSObject = new SFSObject();
			NiuNiuProtobufSerializer niuNiuProtobufSerializer = new NiuNiuProtobufSerializer();
			MemoryStream memoryStream = new MemoryStream();
			niuNiuProtobufSerializer.Serialize(memoryStream, msg);
			byte[] data = memoryStream.ToArray();
			ByteArray byteArray = new ByteArray();
			byteArray.WriteBytes(data);
			iSFSObject.PutByteArray("data", byteArray);
			return iSFSObject;
		}
		public void SendGetSyncInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.Sync", new SFSObject(), this.CurRoom));
		}
		public void SendSitDownInfo(int index)
		{
			ISFSObject iSFSObject = new SFSObject();
			iSFSObject.PutInt("seat", index);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.SitDown", iSFSObject, this.CurRoom));
		}
		public void SendShangZhuangInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.ShangZhuang", new SFSObject(), this.CurRoom));
		}
		public void SendTongZhuangInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.TongZhuang", new SFSObject(), this.CurRoom));
		}
		public void SendFanZhuangInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.FanZhuang", new SFSObject(), this.CurRoom));
		}
		public void SendBuFanZhuangInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.BuFan", new SFSObject(), this.CurRoom));
		}
		public void SendXiaZhuangInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.XiaZhuang", new SFSObject(), this.CurRoom));
		}
		public void SendFengZhuangInfo(bool isFengZhuang)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutBool("data", isFengZhuang);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.FengZhuang", sFSObject, this.CurRoom));
		}
		public void SendPlayDiceInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.PlayDice", new SFSObject(), this.CurRoom));
		}
		public void SendOpenPokerInfo()
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("data", GameDataManager.Instance.Seat);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.OpenPoker", sFSObject, this.CurRoom));
		}
		public void SendStandUpInfo()
		{
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.StandUp", new SFSObject(), this.CurRoom));
		}
		public void SendDownBetInfo(int betNums, int downBetCount, int chipIndex)
		{
			Msg msg = new Msg();
			msg.msgType = Msg.MsgType.DownBetInfo;
			DownBetInfo downBetInfo = new DownBetInfo();
			downBetInfo.username = SingletonMono<DataManager, AllScene>.Instance.username;
			downBetInfo.betNums = betNums;
			downBetInfo.downBetCount = (double)downBetCount;
			downBetInfo.chipIndex = chipIndex;
			downBetInfo.seat = GameDataManager.Instance.Seat;
			msg.downBetInfo.Add(downBetInfo);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.DownBet", this.GetSendMsg(msg), this.CurRoom));
		}
		public void SendLeaveRoom()
		{
			SingletonMono<DataManager, AllScene>.Instance.IsQuitRoom = true;
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new LeaveRoomRequest());
		}
		public void SendExpressionToRoom(int expressionIndex)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("seat", GameDataManager.Instance.Seat);
			sFSObject.PutInt("expressionIndex", expressionIndex);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.SendExpression", sFSObject, this.CurRoom));
		}
		public void SendDialogueToRoom(int dialogueIndex)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("seat", GameDataManager.Instance.Seat);
			sFSObject.PutInt("dialogueIndex", dialogueIndex);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.Dialogue", sFSObject, this.CurRoom));
		}
		public void SendChatTextToRoom(string chatText)
		{
			SFSObject sFSObject = new SFSObject();
			sFSObject.PutInt("seat", GameDataManager.Instance.Seat);
			sFSObject.PutUtfString("nick", SingletonMono<DataManager, AllScene>.Instance.nick);
			sFSObject.PutUtfString("headUrl", SingletonMono<DataManager, AllScene>.Instance.headUrl);
			sFSObject.PutUtfString("chatText", chatText);
			SingletonMono<NetManager, AllScene>.Instance.GetSmartFox.Send(new ExtensionRequest("CR.ChatText", sFSObject, this.CurRoom));
		}
		public void OnExtensionRespones(ISFSObject sfsObj)
		{
			byte[] bytes = sfsObj.GetByteArray("data").Bytes;
			NiuNiuProtobufSerializer niuNiuProtobufSerializer = new NiuNiuProtobufSerializer();
			MemoryStream source = new MemoryStream(bytes);
			Msg msg = niuNiuProtobufSerializer.Deserialize(source, null, typeof(Msg)) as Msg;
			switch (msg.msgType)
			{
			case Msg.MsgType.SyncInfo:
				GameMain.Instance.uiManager.pokerNum = sfsObj.GetInt("pokerNum");
				GameMain.Instance.uiManager.UpdatePokerNum();
				GameMain.Instance.uiManager.ShowRoomInfo(sfsObj.GetUtfString("site"), sfsObj.GetUtfString("roomIndex"));
				GameMain.Instance.uiManager.minDownBet = sfsObj.GetDouble("minDownBet");
				GameMain.Instance.uiManager.seatCoin = sfsObj.GetDouble("seatCoin");
				GameMain.Instance.SyncServer(msg);
				break;
			case Msg.MsgType.GameState:
				GameMain.Instance.SetCurGameState(msg.gameState);
				break;
			case Msg.MsgType.Time:
				GameMain.Instance.uiManager.UpdateTime(msg.time);
				break;
			case Msg.MsgType.SitDownInfo:
				GameMain.Instance.uiManager.ShowSitDownPlayer(msg.allPlayer[0]);
				break;
			case Msg.MsgType.BankerInfo:
				GameMain.Instance.uiManager.ShowBankerTip(msg.bankerInfo.bankerSeat);
				GameDataManager.Instance.SetBankerData(msg.bankerInfo);
				GameMain.Instance.uiManager.UpdateBankerDownBet(msg.bankerInfo.curCoin, false);
				break;
			case Msg.MsgType.DownBetInfo:
				GameMain.Instance.uiManager.AcceptServerUpdateBetNums(msg.downBetInfo[0]);
				break;
			case Msg.MsgType.Card:
				GameDataManager.Instance.allCards = msg.allCards;
				break;
			case Msg.MsgType.UpdatePlayer:
				GameMain.Instance.uiManager.UpdatePlayerShow(msg.allPlayer[0]);
				break;
			case Msg.MsgType.BankerDownBet:
				GameMain.Instance.uiManager.UpdateBankerDownBet(msg.bankerDownBetCoin, true);
				break;
			case Msg.MsgType.PlayDice:
				GameMain.Instance.uiManager.PlayDiceAnim(msg.allDiceInfo[0], msg.allDiceInfo[1]);
				break;
			case Msg.MsgType.OpenPoker:
				for (int i = 0; i < msg.openPokerSeat.Count; i++)
				{
					GameMain.Instance.uiManager.IsShowCowCount(true, msg.openPokerSeat[i], true);
				}
				break;
			case Msg.MsgType.ShowWin:
				for (int j = 0; j < msg.isWin.Count; j++)
				{
					GameDataManager.Instance.IsWins[j] = msg.isWin[j];
				}
				GameMain.Instance.uiManager.IsShowCowMultiple(true);
				if (GameDataManager.Instance.isBanker)
				{
					GameMain.Instance.uiManager.IsShowFengZhuangBtn(true);
				}
				break;
			case Msg.MsgType.RemovePlayer:
				GameMain.Instance.uiManager.RemovePlayer(msg.allPlayer[0]);
				break;
			case Msg.MsgType.JoinRoomPlayer:
				GameMain.Instance.uiManager.SetPlayerInfo(msg.allPlayer);
				break;
			case Msg.MsgType.Score:
				GameDataManager.Instance.score = msg.allScore.selfScore;
				GameDataManager.Instance.bankerScore = msg.allScore.bankerScore;
				GameMain.Instance.uiManager.IsShowScore(true);
				break;
			case Msg.MsgType.BankerTip:
				GameMain.Instance.uiManager.IsShowBankerTip(true, msg.bankerTipNum);
				break;
			case Msg.MsgType.HideBanker:
				GameMain.Instance.uiManager.HideBankerShow();
				break;
			case Msg.MsgType.FeedbackTip:
				TipManager.Instance.ShowFeedbackTipsBar(msg.feedbackTip);
				break;
			case Msg.MsgType.GameChat:
				GameMain.Instance.uiManager.ShowGameChat(msg);
				break;
			}
		}
	}
}
