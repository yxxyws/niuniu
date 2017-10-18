using com.max.JiXiangNiuNiu;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
namespace NiuNiu
{
	public class GameMain : MonoBehaviour
	{
		public static GameMain Instance;
		public GameUIManager uiManager;
		public GameDataManager gameData;
		public NiuNiuNetManager gameNet;
		private FSM<GameStates> m_fsm;
		private void Awake()
		{
			GameMain.Instance = this;
		}
		private void Start()
		{
			this.m_fsm = new FSM<GameStates>();
			this.m_fsm.InsertState(GameStates.Init, new FsmFunc(this.InitEnter), delegate
			{
			}, delegate
			{
			});
			this.m_fsm.InsertState(GameStates.Wait, new FsmFunc(this.WaitEnter), delegate
			{
			}, new FsmFunc(this.WaitExit));
			this.m_fsm.InsertState(GameStates.BankerOperate, new FsmFunc(this.BankerOperateEnter), delegate
			{
			}, new FsmFunc(this.BankerOperateExit));
			this.m_fsm.InsertState(GameStates.DownBet, new FsmFunc(this.DownBetEnter), delegate
			{
			}, new FsmFunc(this.DownBetExit));
			this.m_fsm.InsertState(GameStates.ShowResult, new FsmFunc(this.ShowResultEnter), delegate
			{
			}, new FsmFunc(this.ShowResultExit));
			this.m_fsm.InsertState(GameStates.End, new FsmFunc(this.EndEnter), delegate
			{
			}, new FsmFunc(this.EndExit));
			this.m_fsm.SetCurrentState(GameStates.Init);
			SoundManager.Instance.PlaySound(SoundType.BG, "niuNiuBg");
		}
		private void OnDestroy()
		{
			base.StopAllCoroutines();
		}
		public void SetCurGameState(GameStates state)
		{
			this.m_fsm.SetCurrentState(state);
		}
		public GameStates GetGameState()
		{
			return this.m_fsm.GetCurrentState();
		}
		public void SyncServer(Msg msg)
		{
			base.StartCoroutine(this.SyncToServer(msg));
			this.m_fsm.SetCurrentState(msg.gameState);
			base.StartCoroutine(this.uiManager.ShowChangeTableBtn());
		}
		[DebuggerHidden]
		private IEnumerator SyncToServer(Msg msg)
		{
			GameMain.<SyncToServer>c__Iterator0 <SyncToServer>c__Iterator = new GameMain.<SyncToServer>c__Iterator0();
			<SyncToServer>c__Iterator.msg = msg;
			<SyncToServer>c__Iterator.$this = this;
			return <SyncToServer>c__Iterator;
		}
		private void InitEnter()
		{
			this.uiManager.InitShow();
			this.gameNet.SendGetSyncInfo();
		}
		private void WaitEnter()
		{
			this.gameData.bankerTimes = 0;
			this.uiManager.UpdateBankerNum();
			this.uiManager.IsShowScore(false);
			this.uiManager.UpdateTimeText(this.m_fsm.GetCurrentState());
			if (this.uiManager.IsSeatNull())
			{
				this.uiManager.IsShowNullSeatTip(true);
			}
			this.uiManager.IsShowRightBottomBtn(true);
		}
		private void WaitExit()
		{
			this.uiManager.IsShowNullSeatTip(false);
			if (this.gameData.Seat < 4)
			{
				this.uiManager.IsShowRightBottomBtn(false);
				this.uiManager.IsShowFengZhuangBtn(false);
			}
		}
		private void BankerOperateEnter()
		{
			this.uiManager.UpdateTimeText(this.m_fsm.GetCurrentState());
			this.uiManager.IsShowBankerBtn(this.gameData.isBanker);
		}
		private void BankerOperateExit()
		{
			this.gameData.bankerTimes++;
			this.uiManager.UpdateBankerNum();
		}
		private void DownBetEnter()
		{
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "start_DownBet");
			this.uiManager.UpdateTimeText(this.m_fsm.GetCurrentState());
			if (!this.gameData.isBanker)
			{
				this.uiManager.IsShowSelectChip(true);
			}
		}
		private void DownBetExit()
		{
			this.uiManager.IsShowSelectChip(false);
			if (GameDataManager.Instance.isBanker)
			{
				this.uiManager.IsShowPlayDiceBtn(true);
			}
		}
		private void ShowResultEnter()
		{
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "start_showResult");
			this.uiManager.UpdateTimeText(this.m_fsm.GetCurrentState());
		}
		private void ShowResultExit()
		{
			this.uiManager.IsShowRightBottomBtn(true);
		}
		private void EndEnter()
		{
			this.uiManager.UpdateTimeText(this.m_fsm.GetCurrentState());
		}
		private void EndExit()
		{
			this.uiManager.IsShowNullSeatTip(false);
			if (this.gameData.Seat < 4)
			{
				this.uiManager.IsShowRightBottomBtn(false);
				this.uiManager.IsShowFengZhuangBtn(false);
			}
			this.uiManager.ResetStart();
			if (this.uiManager.pokerNum < 20)
			{
				this.uiManager.pokerNum = 104;
				this.uiManager.UpdatePokerNum();
			}
			if (this.m_fsm.GetNextState() == GameStates.DownBet)
			{
				this.gameData.bankerTimes++;
				this.uiManager.UpdateBankerNum();
			}
		}
	}
}
