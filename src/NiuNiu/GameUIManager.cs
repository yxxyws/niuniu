using com.max.JiXiangNiuNiu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace NiuNiu
{
	public class GameUIManager : MonoBehaviour
	{
		public UIButton btn;
		public GameDataManager gameData;
		public NiuNiuNetManager gameNet;
		public UILabel label_site;
		public UILabel label_roomIndex;
		public UIButton btn_back;
		public UIButton btn_setting;
		public UILabel label_bankerNum;
		public UILabel label_pokerNum;
		public Player[] allPlayer;
		public Vector3[] seatPos;
		public PoolManager chip_poolManager;
		public GameObject obj_selectChip;
		public UIButton[] btn_SelectBetNum;
		public GameObject obj_selectChipLight;
		public UISprite pic_timeText;
		public UILabel lable_time;
		public GameObject obj_BankerBtn;
		public UIButton btn_shangZhuang;
		public UIButton btn_xiaZhuang;
		public UIButton btn_fanZhuang;
		public UIButton btn_bufan;
		public UIButton btn_tongZhuang;
		public UIButton btn_dice;
		public UILabel label_shangZhuang_count;
		public UILabel label_fanZhuang_count;
		public UILabel label_tongZhuang_count;
		public UILabel label_selfScore;
		public GameObject obj_diceAnim;
		public UISprite pic_dice01;
		public UISprite pic_dice02;
		public UIToggle btn_FengZhuang;
		public GameObject pic_nullSeatTip;
		public GameObject pic_startTip;
		public UISprite pic_bankerTip;
		public UIFont bankerFont;
		public UIFont winFont;
		public UIFont loseFont;
		public double minDownBet = 100.0;
		public double seatCoin;
		public UILabel label_niuNiu_Tip;
		public UILabel label_coin;
		public UILabel label_selfTotalBet;
		public UIButton btn_gameChat;
		public UIButton btn_openFriend;
		public UIButton btn_lookPlayer;
		public UIButton btn_standUp;
		public UIButton btn_changeTable;
		public LookOnPlayerManager lookPlayerMana;
		public GameObject obj_friendTip;
		public ExpressionShow[] allExpress;
		public UIGrid grid_expression;
		public UILabel label_chatText;
		private Queue<GameObject> allShowExpress = new Queue<GameObject>();
		private int curIndex;
		private float timer = 3f;
		public PoolManager pool_chatText;
		public UIGrid grid_chatText;
		private Queue<Prefab_ChatText> allChatText = new Queue<Prefab_ChatText>();
		private float niuNiuTipTimer;
		private List<GameObject> allDownBetChip = new List<GameObject>();
		private List<GameObject> allBankerChip = new List<GameObject>();
		private PlayerInfo[] allPlayerInfo = new PlayerInfo[4];
		public int pokerNum
		{
			get;
			set;
		}
		private void Awake()
		{
			for (int i = 0; i < this.btn_SelectBetNum.Length; i++)
			{
				this.SetButtonCallBack(this.btn_SelectBetNum[i], new Action<int>(this.OnSelectBetNumClick), i);
			}
			EventDelegate.Set(this.btn_bufan.onClick, new EventDelegate.Callback(this.OnBuFanBtnClick));
			EventDelegate.Set(this.btn_fanZhuang.onClick, new EventDelegate.Callback(this.OnFanZhuangBtnClick));
			EventDelegate.Set(this.btn_shangZhuang.onClick, new EventDelegate.Callback(this.OnShangZhuangBtnClick));
			EventDelegate.Set(this.btn_tongZhuang.onClick, new EventDelegate.Callback(this.OnTongZhuangBtnClick));
			EventDelegate.Set(this.btn_xiaZhuang.onClick, new EventDelegate.Callback(this.OnXiaZhuangBtnClick));
			EventDelegate.Set(this.btn_dice.onClick, new EventDelegate.Callback(this.OnDiceBtnClick));
			EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
			EventDelegate.Set(this.btn_setting.onClick, new EventDelegate.Callback(this.OnSettingBtnClick));
			EventDelegate.Set(this.btn_lookPlayer.onClick, new EventDelegate.Callback(this.OnPangGuangBtnClick));
			EventDelegate.Set(this.btn_standUp.onClick, new EventDelegate.Callback(this.OnStandUpBtnClick));
			EventDelegate.Set(this.btn_changeTable.onClick, new EventDelegate.Callback(this.OnChangeTableBtnClick));
			EventDelegate.Set(this.btn_FengZhuang.onChange, new EventDelegate.Callback(this.OnFengZhuangBtnClick));
			EventDelegate.Set(this.btn_openFriend.onClick, new EventDelegate.Callback(this.OnOpenFriendBtnClick));
			EventDelegate.Set(this.btn_gameChat.onClick, new EventDelegate.Callback(this.OnGameChatBtnClick));
			EventDelegate.Set(this.pic_bankerTip.GetComponent<TweenScale>().onFinished, delegate
			{
				this.pic_bankerTip.gameObject.SetActive(false);
			});
			EventDelegate.Set(this.btn.onClick, new EventDelegate.Callback(this.OnBtnClick));
		}
		private void OnBtnClick()
		{
			SingletonMono<NetManager, AllScene>.Instance.SendSaveSafeCoin(-300000.0, 300000.0);
		}
		private void Start()
		{
			FriendManager.UpdateFriendTip = (Action<bool>)Delegate.Combine(FriendManager.UpdateFriendTip, new Action<bool>(this.UpdateFriendTipShow));
			FriendManager.Instance.friendBar.UpdateFriendTip();
			TipManager.Instance.HideWaitTip();
		}
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			FriendManager.UpdateFriendTip = (Action<bool>)Delegate.Remove(FriendManager.UpdateFriendTip, new Action<bool>(this.UpdateFriendTipShow));
		}
		private void Update()
		{
			if (this.niuNiuTipTimer > 0f)
			{
				this.niuNiuTipTimer -= Time.deltaTime;
				if (this.niuNiuTipTimer <= 0f)
				{
					this.label_niuNiu_Tip.gameObject.SetActive(false);
				}
			}
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
				if (this.timer <= 0f)
				{
					this.label_chatText.gameObject.SetActive(false);
				}
			}
		}
		private void UpdateFriendTipShow(bool isShow)
		{
			this.obj_friendTip.SetActive(isShow);
		}
		public void ShowRoomInfo(string site, string roomIndex)
		{
			this.label_site.text = "场区：" + site;
			this.label_roomIndex.text = "桌号：" + roomIndex;
		}
		public void UpdateBankerNum()
		{
			this.label_bankerNum.text = "连庄次数：" + this.gameData.bankerTimes.ToString();
		}
		public void ShowNiuNiuTips(string info, float time)
		{
			this.label_niuNiu_Tip.text = info;
			this.niuNiuTipTimer = time;
			if (this.niuNiuTipTimer <= 0f)
			{
				this.label_niuNiu_Tip.gameObject.SetActive(false);
				return;
			}
			this.label_niuNiu_Tip.gameObject.SetActive(true);
		}
		public void UpdatePokerNum()
		{
			this.label_pokerNum.text = "剩余牌数：" + this.pokerNum.ToString();
		}
		public void InitShow()
		{
			this.IsShowBankerBtn(false);
			this.IsShowSelectChip(false);
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				this.allPlayer[i].InitShow(i);
				this.allPlayerInfo[i] = null;
			}
			this.IsShowNullSeatTip(false);
			this.IsShowRightBottomBtn(false);
			this.IsShowFengZhuangBtn(false);
			this.IsShowPlayDiceBtn(false);
			this.IsShowBankerTip(false, 0);
			for (int j = 0; j < this.allExpress.Length; j++)
			{
				this.allExpress[j].gameObject.SetActive(false);
			}
			this.label_chatText.gameObject.SetActive(false);
			this.pool_chatText.ResetAllIdleItem();
			this.ResetStart();
		}
		public void ResetStart()
		{
			this.gameData.ResStart();
			this.IsShowScore(false);
			for (int i = 0; i < this.allDownBetChip.Count; i++)
			{
				this.chip_poolManager.ResetIdleItem(this.allDownBetChip[i]);
			}
			this.allDownBetChip.Clear();
			for (int j = 0; j < this.allPlayer.Length; j++)
			{
				this.allPlayer[j].ResetStart();
			}
			this.UpdateSelfInfoShow();
		}
		public bool IsSeatNull()
		{
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				if (this.allPlayer[i].IsNull)
				{
					return true;
				}
			}
			return false;
		}
		public void IsShowRightBottomBtn(bool isShow)
		{
			if (isShow && !this.gameData.isBanker)
			{
				if (this.gameData.Seat < 4)
				{
					this.btn_standUp.gameObject.SetActive(true);
					this.btn_changeTable.gameObject.SetActive(false);
				}
				else
				{
					this.btn_standUp.gameObject.SetActive(false);
					this.btn_changeTable.gameObject.SetActive(this.gameData.isCanShowChangeTable);
				}
			}
			else
			{
				this.btn_standUp.gameObject.SetActive(false);
				this.btn_changeTable.gameObject.SetActive(false);
			}
		}
		[DebuggerHidden]
		public IEnumerator ShowChangeTableBtn()
		{
			GameUIManager.<ShowChangeTableBtn>c__Iterator0 <ShowChangeTableBtn>c__Iterator = new GameUIManager.<ShowChangeTableBtn>c__Iterator0();
			<ShowChangeTableBtn>c__Iterator.$this = this;
			return <ShowChangeTableBtn>c__Iterator;
		}
		public void IsShowFengZhuangBtn(bool isShow)
		{
			this.btn_FengZhuang.value = false;
			this.btn_FengZhuang.gameObject.SetActive(isShow);
		}
		public void SetPlayerInfo(List<PlayerInfo> allInfo)
		{
			for (int i = 0; i < allInfo.Count; i++)
			{
				if (allInfo[i].seat < 4)
				{
					this.ShowSitDownPlayer(allInfo[i]);
				}
				else
				{
					this.lookPlayerMana.UpdatePlayer(allInfo[i]);
				}
				if (allInfo[i].username == SingletonMono<DataManager, AllScene>.Instance.username)
				{
					this.gameData.Seat = allInfo[i].seat;
				}
			}
		}
		public void UpdateSelfInfoShow()
		{
			this.label_coin.text = DataManager.ChangeDanWei(SingletonMono<DataManager, AllScene>.Instance.coin);
			this.label_selfTotalBet.text = this.gameData.yetBetCount.ToString();
		}
		public void UpdatePlayerShow(PlayerInfo info)
		{
			if (info.username == SingletonMono<DataManager, AllScene>.Instance.username)
			{
				SingletonMono<DataManager, AllScene>.Instance.coin = info.coin;
				this.UpdateSelfInfoShow();
			}
			if (info.seat < 4)
			{
				int offsetSeat = this.gameData.GetOffsetSeat(info.seat);
				this.allPlayer[offsetSeat].SetPlayerInfo(info);
				this.allPlayer[offsetSeat].UpdatePlayer();
			}
			else
			{
				this.lookPlayerMana.UpdatePlayer(info);
			}
		}
		public void RemovePlayer(PlayerInfo info)
		{
			if (info.seat < 4)
			{
				this.allPlayerInfo[info.seat] = null;
				int offsetSeat = this.gameData.GetOffsetSeat(info.seat);
				this.allPlayer[offsetSeat].HidePlayer();
				if (info.seat == this.gameData.Seat)
				{
					TipManager.Instance.HideWaitTip();
					this.gameData.Seat = 5;
					this.IsShowRightBottomBtn(true);
					this.IsShowFengZhuangBtn(false);
				}
			}
			else
			{
				this.lookPlayerMana.RemovePlayer(info);
			}
		}
		public void HideBankerShow()
		{
			if (this.gameData.isBanker)
			{
				this.gameData.isBanker = false;
			}
			int offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
			this.allPlayer[offsetSeat].IsShowBankerTip(false);
			for (int i = 0; i < this.allBankerChip.Count; i++)
			{
				this.chip_poolManager.ResetIdleItem(this.allBankerChip[i]);
			}
			this.allBankerChip.Clear();
		}
		public void ShowSitDownPlayer(PlayerInfo info)
		{
			this.allPlayerInfo[info.seat] = info;
			this.lookPlayerMana.RemovePlayer(info);
			if (info.username == SingletonMono<DataManager, AllScene>.Instance.username)
			{
				this.gameData.Seat = info.seat;
				this.gameData.offsetSeat = (1 - info.seat + 4) % 4;
				this.HideNullSeatTip();
				for (int i = 0; i < this.allPlayer.Length; i++)
				{
					int offsetSeat = this.gameData.GetOffsetSeat(i);
					if (this.allPlayerInfo[i] != null)
					{
						this.allPlayer[offsetSeat].SetIsInit(false);
						this.allPlayer[offsetSeat].SetPlayerInfo(this.allPlayerInfo[i]);
					}
					else
					{
						this.allPlayer[offsetSeat].HidePlayer();
					}
					this.allPlayer[i].UpdateDownBetShow();
				}
				TipManager.Instance.HideWaitTip();
				this.IsShowRightBottomBtn(true);
			}
			else
			{
				int offsetSeat2 = this.gameData.GetOffsetSeat(info.seat);
				this.allPlayer[offsetSeat2].SetPlayerInfo(info);
			}
			this.IsShowNullSeatTip(this.IsSeatNull());
		}
		public void UpdateTime(int time)
		{
			if (!this.lable_time.gameObject.activeSelf)
			{
				this.lable_time.gameObject.SetActive(true);
			}
			else
			{
				if (time <= 5 && time > 0)
				{
					SoundManager.Instance.PlaySound(SoundType.EFFECT, "daoJiShi");
				}
			}
			this.lable_time.text = time.ToString();
			if (this.obj_BankerBtn.activeSelf && time == 0)
			{
				this.BankerTimeoutHandle();
			}
		}
		public void UpdateTimeText(GameStates state)
		{
			if (state == GameStates.DownBet)
			{
				this.pic_timeText.spriteName = "pic_text_08";
			}
			else
			{
				if (state == GameStates.ShowResult)
				{
					this.pic_timeText.spriteName = "pic_text_09";
				}
				else
				{
					this.pic_timeText.spriteName = "pic_text_10";
				}
			}
		}
		public void IsShowNullSeatTip(bool isShow)
		{
			this.pic_timeText.spriteName = "pic_text_10";
			this.pic_timeText.gameObject.SetActive(!isShow);
			if (this.gameData.Seat > 3)
			{
				this.pic_nullSeatTip.SetActive(isShow);
			}
			this.pic_startTip.SetActive(isShow);
		}
		public void HideNullSeatTip()
		{
			this.pic_nullSeatTip.SetActive(false);
		}
		public void IsShowBankerBtn(bool isShow)
		{
			if (isShow)
			{
				if (this.gameData.bankerTimes == 0)
				{
					this.btn_xiaZhuang.gameObject.SetActive(false);
					this.btn_fanZhuang.gameObject.SetActive(false);
					this.btn_bufan.gameObject.SetActive(false);
					this.btn_shangZhuang.gameObject.SetActive(true);
					this.label_shangZhuang_count.text = this.gameData.bankerMaxStake.ToString();
				}
				else
				{
					if (this.gameData.bankerCurCoin < this.gameData.bankerMaxStake / 2.0)
					{
						this.btn_xiaZhuang.gameObject.SetActive(true);
						this.btn_bufan.gameObject.SetActive(false);
					}
					else
					{
						this.btn_xiaZhuang.gameObject.SetActive(false);
						this.btn_bufan.gameObject.SetActive(true);
					}
					if (SingletonMono<DataManager, AllScene>.Instance.coin + this.gameData.bankerCurCoin >= this.gameData.bankerMaxStake && this.gameData.bankerMaxStake / 2.0 <= this.seatCoin)
					{
						this.btn_fanZhuang.gameObject.SetActive(true);
					}
					else
					{
						this.btn_fanZhuang.gameObject.SetActive(false);
					}
					this.btn_shangZhuang.gameObject.SetActive(false);
					this.label_fanZhuang_count.text = this.gameData.bankerMaxStake.ToString();
				}
				this.label_tongZhuang_count.text = (SingletonMono<DataManager, AllScene>.Instance.coin + this.gameData.bankerCurCoin).ToString();
				this.obj_BankerBtn.SetActive(true);
			}
			else
			{
				this.obj_BankerBtn.SetActive(false);
			}
		}
		public void IsShowSelectChip(bool isShow)
		{
			if (isShow)
			{
				this.IsCanDownBet(true);
				this.InitSelectChipShow();
				this.obj_selectChip.SetActive(true);
			}
			else
			{
				this.IsCanDownBet(false);
				this.obj_selectChip.SetActive(false);
			}
		}
		public void IsCanDownBet(bool isCan)
		{
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				this.allPlayer[i].IsCanDownBet(isCan);
			}
			if (isCan)
			{
				int offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
				this.allPlayer[offsetSeat].IsCanDownBet(false);
			}
		}
		public void BankerTimeoutHandle()
		{
			if (this.gameData.bankerTimes != 0)
			{
				if (this.gameData.bankerCurCoin < this.gameData.bankerMaxStake / 2.0)
				{
					this.OnXiaZhuangBtnClick();
				}
				else
				{
					this.OnBuFanBtnClick();
				}
			}
			else
			{
				this.OnShangZhuangBtnClick();
			}
			this.IsShowBankerBtn(false);
		}
		private void SetButtonCallBack(UIButton btn, Action<int> callback, int index)
		{
			EventDelegate.Set(btn.onClick, delegate
			{
				callback(index);
			});
		}
		public void InitSelectChipShow()
		{
			for (int i = 0; i < this.btn_SelectBetNum.Length; i++)
			{
				if ((double)this.gameData.ChipsNum[i] < this.minDownBet)
				{
					this.gameData.SelectBetIndex = i + 1;
					this.btn_SelectBetNum[i].isEnabled = false;
				}
				else
				{
					if ((SingletonMono<DataManager, AllScene>.Instance.coin + this.gameData.yetBetCount - this.gameData.yetBetCount * 15.0) / 15.0 < (double)this.gameData.ChipsNum[i])
					{
						this.btn_SelectBetNum[i].isEnabled = false;
						if ((double)this.gameData.ChipsNum[i] <= this.minDownBet)
						{
							this.IsCanDownBet(false);
							this.gameData.SelectBetIndex = -1;
							this.obj_selectChipLight.SetActive(false);
						}
					}
					else
					{
						this.btn_SelectBetNum[i].isEnabled = true;
						if (i == this.gameData.SelectBetIndex)
						{
							this.obj_selectChipLight.SetActive(true);
							this.obj_selectChipLight.transform.position = this.btn_SelectBetNum[i].transform.position;
						}
					}
				}
			}
		}
		public void UpdateSelectChipShow()
		{
			for (int i = this.btn_SelectBetNum.Length - 1; i >= 0; i--)
			{
				if ((SingletonMono<DataManager, AllScene>.Instance.coin + this.gameData.yetBetCount - this.gameData.yetBetCount * 15.0) / 15.0 >= (double)this.gameData.ChipsNum[i])
				{
					break;
				}
				this.btn_SelectBetNum[i].isEnabled = false;
				if ((double)this.gameData.ChipsNum[i] <= this.minDownBet)
				{
					this.IsCanDownBet(false);
					this.obj_selectChipLight.SetActive(false);
					return;
				}
				if (this.gameData.SelectBetIndex == i)
				{
					this.gameData.SelectBetIndex--;
					this.obj_selectChipLight.transform.position = this.btn_SelectBetNum[i - 1].transform.position;
				}
			}
		}
		public void PlayDiceAnim(int dice1, int dice2)
		{
			this.IsShowPlayDiceBtn(false);
			this.obj_diceAnim.SetActive(true);
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "playDice");
			base.StartCoroutine(this.ShowDice(dice1, dice2));
		}
		public void ShowBankerTip(int seat)
		{
			int offsetSeat;
			if (seat != this.gameData.BankerSeat)
			{
				offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
				this.allPlayer[offsetSeat].IsShowBankerTip(false);
			}
			this.gameData.BankerSeat = seat;
			offsetSeat = this.gameData.GetOffsetSeat(seat);
			this.allPlayer[offsetSeat].IsShowBankerTip(true);
		}
		public void ShowAllPoker()
		{
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				this.allPlayer[i].ShowAllPoker();
			}
		}
		public void IsShowAllCowCount(bool isShow, bool isPlayAnim)
		{
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				this.allPlayer[i].IsShowCowCount(isShow, isPlayAnim);
			}
		}
		public void IsShowCowCount(bool isShow, int index, bool isPlayAnim)
		{
			int offsetSeat = this.gameData.GetOffsetSeat(index);
			this.allPlayer[offsetSeat].IsShowCowCount(isShow, isPlayAnim);
		}
		public void IsShowCowMultiple(bool isShow)
		{
			for (int i = 0; i < this.allPlayer.Length; i++)
			{
				this.allPlayer[i].IsShowCowMultiple(isShow);
			}
		}
		[DebuggerHidden]
		private IEnumerator ShowDice(int dice1, int dice2)
		{
			GameUIManager.<ShowDice>c__Iterator1 <ShowDice>c__Iterator = new GameUIManager.<ShowDice>c__Iterator1();
			<ShowDice>c__Iterator.dice1 = dice1;
			<ShowDice>c__Iterator.dice2 = dice2;
			<ShowDice>c__Iterator.$this = this;
			return <ShowDice>c__Iterator;
		}
		public void IsShowPlayDiceBtn(bool isShow)
		{
			if (isShow)
			{
				this.btn_dice.gameObject.SetActive(true);
			}
			else
			{
				this.btn_dice.gameObject.SetActive(false);
			}
		}
		public void IsShowScore(bool IsShow)
		{
			if (IsShow)
			{
				if (this.gameData.isBanker)
				{
					this.label_selfScore.bitmapFont = this.bankerFont;
					if (this.gameData.bankerScore > 0.0)
					{
						this.label_selfScore.text = "+" + this.gameData.bankerScore.ToString();
					}
					else
					{
						this.label_selfScore.text = this.gameData.bankerScore.ToString();
					}
					this.label_selfScore.gameObject.SetActive(true);
				}
				else
				{
					if (this.gameData.score > 0.0)
					{
						this.label_selfScore.bitmapFont = this.winFont;
						this.label_selfScore.text = "+" + this.gameData.score.ToString();
						this.label_selfScore.gameObject.SetActive(true);
					}
					else
					{
						if (this.gameData.score < 0.0)
						{
							this.label_selfScore.bitmapFont = this.loseFont;
							this.label_selfScore.text = this.gameData.score.ToString();
							this.label_selfScore.gameObject.SetActive(true);
						}
					}
					int offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
					this.allPlayer[offsetSeat].IsShowScore(true, this.gameData.bankerScore);
				}
			}
			else
			{
				this.label_selfScore.gameObject.SetActive(false);
				int offsetSeat2 = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
				for (int i = 0; i < this.allPlayer.Length; i++)
				{
					this.allPlayer[i].IsShowScore(false, 0.0);
				}
			}
		}
		public void IsShowBankerTip(bool isShow, int num = 0)
		{
			if (isShow)
			{
				int offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
				if (num == 0)
				{
					this.pic_bankerTip.spriteName = "pic_text_05";
					this.ShowNiuNiuTips("玩家：" + this.allPlayer[offsetSeat].Nick + " 上庄", 3f);
				}
				else
				{
					if (num == 1)
					{
						this.pic_bankerTip.spriteName = "pic_text_01";
						this.ShowNiuNiuTips("玩家：" + this.allPlayer[offsetSeat].Nick + " 翻庄", 3f);
					}
					else
					{
						if (num == 2)
						{
							this.pic_bankerTip.spriteName = "pic_text_07";
							this.ShowNiuNiuTips("玩家：" + this.allPlayer[offsetSeat].Nick + " 统庄", 3f);
						}
						else
						{
							this.pic_bankerTip.spriteName = "pic_text_06";
							SoundManager.Instance.PlaySound(SoundType.EFFECT, "tongSha");
						}
					}
				}
				this.pic_bankerTip.gameObject.SetActive(true);
				this.pic_bankerTip.GetComponent<TweenScale>().ResetToBeginning();
				this.pic_bankerTip.GetComponent<TweenScale>().PlayForward();
			}
			else
			{
				this.pic_bankerTip.gameObject.SetActive(false);
			}
		}
		public void UpdateBankerDownBet(double coin, bool isPlayAnim)
		{
			for (int i = 0; i < this.allBankerChip.Count; i++)
			{
				this.chip_poolManager.ResetIdleItem(this.allBankerChip[i]);
			}
			this.allBankerChip.Clear();
			int offsetSeat = this.gameData.GetOffsetSeat(this.gameData.BankerSeat);
			if (coin > 0.0)
			{
				this.allPlayer[offsetSeat].IsShowBankerCount(true, coin);
				SoundManager.Instance.PlaySound(SoundType.EFFECT, "bankerDownBet");
				this.gameData.bankerCurCoin = coin;
			}
			else
			{
				this.allPlayer[offsetSeat].IsShowBankerCount(false, 0.0);
			}
			for (int j = this.gameData.ChipsNum.Length - 1; j >= 0; j--)
			{
				while (coin >= (double)this.gameData.ChipsNum[j])
				{
					coin -= (double)this.gameData.ChipsNum[j];
					GameObject nGUIItem = this.chip_poolManager.GetNGUIItem();
					nGUIItem.GetComponent<UISprite>().spriteName = string.Format("btn_chip_0{0}", j + 1);
					Vector3 b = new Vector3((float)UnityEngine.Random.Range(-100, 100), (float)UnityEngine.Random.Range(-50, 50), 0f);
					if (isPlayAnim)
					{
						nGUIItem.GetComponent<TweenPosition>().from = this.seatPos[offsetSeat];
						nGUIItem.GetComponent<TweenPosition>().to = this.allPlayer[offsetSeat].btn_downBet.transform.localPosition + b;
						nGUIItem.GetComponent<TweenPosition>().ResetToBeginning();
						nGUIItem.GetComponent<TweenPosition>().PlayForward();
					}
					else
					{
						nGUIItem.transform.localPosition = this.allPlayer[offsetSeat].btn_downBet.transform.localPosition + b;
					}
					this.allBankerChip.Add(nGUIItem);
				}
			}
		}
		public void SyncServerDownBet(DownBetInfo info)
		{
			GameObject nGUIItem = this.chip_poolManager.GetNGUIItem();
			nGUIItem.GetComponent<UISprite>().spriteName = string.Format("btn_chip_0{0}", info.chipIndex + 1);
			this.gameData.totalDownBet += info.downBetCount;
			this.gameData.AllBetNumsCount[info.betNums] += info.downBetCount;
			this.allDownBetChip.Add(nGUIItem);
			if (info.username.Equals(SingletonMono<DataManager, AllScene>.Instance.username))
			{
				this.gameData.yetBetCount += info.downBetCount;
				this.gameData.SelfBetNumCount[info.betNums] += info.downBetCount;
				this.UpdateSelfInfoShow();
			}
			int offsetSeat = this.gameData.GetOffsetSeat(info.betNums);
			Vector3 b = new Vector3((float)UnityEngine.Random.Range(-100, 100), (float)UnityEngine.Random.Range(-50, 50), 0f);
			nGUIItem.transform.localPosition = this.allPlayer[offsetSeat].btn_downBet.transform.localPosition + b;
			this.allPlayer[offsetSeat].UpdateBetNums();
		}
		public void AcceptServerUpdateBetNums(DownBetInfo info)
		{
			GameObject nGUIItem = this.chip_poolManager.GetNGUIItem();
			nGUIItem.GetComponent<UISprite>().spriteName = string.Format("btn_chip_0{0}", info.chipIndex + 1);
			this.allDownBetChip.Add(nGUIItem);
			int offsetSeat = this.gameData.GetOffsetSeat(info.seat);
			if (info.username.Equals(SingletonMono<DataManager, AllScene>.Instance.username))
			{
				SingletonMono<DataManager, AllScene>.Instance.coin -= info.downBetCount;
				this.gameData.yetBetCount += info.downBetCount;
				this.gameData.SelfBetNumCount[info.betNums] += info.downBetCount;
				Vector3 from = this.btn_SelectBetNum[info.chipIndex].transform.localPosition + this.btn_SelectBetNum[0].transform.parent.localPosition;
				nGUIItem.GetComponent<TweenPosition>().from = from;
				this.UpdateSelectChipShow();
				this.IsShowRightBottomBtn(false);
			}
			else
			{
				nGUIItem.GetComponent<TweenPosition>().from = this.seatPos[offsetSeat];
			}
			this.gameData.AllBetNumsCount[info.betNums] += info.downBetCount;
			this.gameData.totalDownBet += info.downBetCount;
			offsetSeat = this.gameData.GetOffsetSeat(info.betNums);
			Vector3 b = new Vector3((float)UnityEngine.Random.Range(-100, 100), (float)UnityEngine.Random.Range(-55, 55), 0f);
			nGUIItem.GetComponent<TweenPosition>().to = this.allPlayer[offsetSeat].btn_downBet.transform.localPosition + b;
			nGUIItem.GetComponent<TweenPosition>().ResetToBeginning();
			nGUIItem.GetComponent<TweenPosition>().PlayForward();
			this.allPlayer[offsetSeat].UpdateBetNums();
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "downBet");
		}
		public void ShowGameChat(Msg msg)
		{
			ChatType chatType = msg.gameChat.chatType;
			int offsetSeat = this.gameData.GetOffsetSeat(msg.gameChat.seat);
			if (chatType == ChatType.Expression)
			{
				int expressionIndex = msg.gameChat.expressionIndex;
				if (offsetSeat < 4)
				{
					this.allPlayer[offsetSeat].ShowExpression(expressionIndex);
				}
				else
				{
					this.ShowExpression(expressionIndex);
				}
			}
			else
			{
				if (chatType == ChatType.Dialogue)
				{
					int dialogueIndex = msg.gameChat.dialogueIndex;
					if (offsetSeat < 4)
					{
						this.allPlayer[offsetSeat].ShowDialogue(dialogueIndex);
					}
					else
					{
						this.ShowDialogue(dialogueIndex);
					}
				}
				else
				{
					string chatText = msg.gameChat.chatText;
					if (offsetSeat < 4)
					{
						this.allPlayer[offsetSeat].ShowChatText(chatText);
					}
					else
					{
						this.ShowChatText(chatText);
					}
					if (this.allChatText.Count < 30)
					{
						Prefab_ChatText component = this.pool_chatText.GetNGUIItem().GetComponent<Prefab_ChatText>();
						component.InitShow(msg);
						this.allChatText.Enqueue(component);
					}
					else
					{
						Prefab_ChatText prefab_ChatText = this.allChatText.Dequeue();
						prefab_ChatText.InitShow(msg);
						prefab_ChatText.transform.SetAsLastSibling();
						this.allChatText.Enqueue(prefab_ChatText);
					}
					this.grid_chatText.Reposition();
					bool shouldMoveVertically = this.grid_chatText.transform.parent.GetComponent<UIScrollView>().shouldMoveVertically;
					if (shouldMoveVertically)
					{
						this.grid_chatText.transform.parent.GetComponent<UIScrollView>().verticalScrollBar.value = 1f;
					}
					else
					{
						this.grid_chatText.transform.parent.GetComponent<UIScrollView>().verticalScrollBar.value = 0f;
					}
				}
			}
		}
		private void ShowExpression(int index)
		{
			this.allExpress[this.curIndex].transform.localPosition = new Vector3(this.grid_expression.cellWidth, 0f, 0f);
			this.allExpress[this.curIndex].transform.SetAsLastSibling();
			this.allExpress[this.curIndex].time = 3f;
			this.allExpress[this.curIndex].callback = new Action(this.ShowExpressionCallback);
			this.allExpress[this.curIndex].InitShow(index);
			this.allExpress[this.curIndex].gameObject.SetActive(true);
			this.allShowExpress.Enqueue(this.allExpress[this.curIndex].gameObject);
			this.curIndex++;
			this.curIndex %= this.allExpress.Length;
			this.grid_expression.repositionNow = true;
			if (this.allShowExpress.Count > 3)
			{
				this.ShowExpressionCallback();
			}
		}
		private void ShowExpressionCallback()
		{
			GameObject gameObject = this.allShowExpress.Dequeue();
			gameObject.SetActive(false);
		}
		public void ShowDialogue(int index)
		{
			this.timer = 3f;
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "dialogue_0" + index);
			this.label_chatText.text = GameDataManager.Instance.allDialogue[index];
			this.label_chatText.gameObject.SetActive(true);
		}
		public void ShowChatText(string str)
		{
			this.timer = 3f;
			this.label_chatText.text = str;
			this.label_chatText.gameObject.SetActive(true);
		}
		private void OnXiaZhuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendXiaZhuangInfo();
			this.IsShowBankerBtn(false);
		}
		private void OnShangZhuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendShangZhuangInfo();
			this.IsShowBankerBtn(false);
		}
		private void OnBuFanBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendBuFanZhuangInfo();
			this.IsShowBankerBtn(false);
		}
		private void OnFanZhuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendFanZhuangInfo();
			this.IsShowBankerBtn(false);
		}
		private void OnTongZhuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendTongZhuangInfo();
			this.IsShowBankerBtn(false);
		}
		private void OnDiceBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			this.IsShowPlayDiceBtn(false);
			NiuNiuNetManager.Instance.SendPlayDiceInfo();
		}
		private void OnSelectBetNumClick(int index)
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "click");
			this.gameData.SelectBetIndex = index;
			this.obj_selectChipLight.transform.position = this.btn_SelectBetNum[index].transform.position;
		}
		private void OnBackBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			TipManager.Instance.IsShowQuitRoomTipBar(true);
		}
		private void OnSettingBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
			SettingManager.Instance.PlayAnim();
		}
		private void OnPangGuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			this.lookPlayerMana.GetComponent<UIPanel>().alpha = 1f;
		}
		private void OnFengZhuangBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "click");
			NiuNiuNetManager.Instance.SendFengZhuangInfo(this.btn_FengZhuang.value);
		}
		private void OnStandUpBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendStandUpInfo();
			TipManager.Instance.ShowWaitTip(string.Empty);
		}
		private void OnChangeTableBtnClick()
		{
			int curSiteIndex = SingletonMono<DataManager, AllScene>.Instance.CurSiteIndex;
			if (SingletonMono<DataManager, AllScene>.Instance.coin >= SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[curSiteIndex].joinRoomMinCoin)
			{
				if (SingletonMono<DataManager, AllScene>.Instance.coin > SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[curSiteIndex].joinRoomMaxCoin)
				{
					TipManager.Instance.ShowConfirmBar("您拥有的金币数已经超过" + SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[curSiteIndex].joinRoomMaxCoin.ToString() + "的金币,不能加入房间!", delegate
					{
						NiuNiuNetManager.Instance.SendLeaveRoom();
						TipManager.Instance.ShowWaitTip("正在退出房间...");
					}, null);
				}
				else
				{
					SingletonMono<DataManager, AllScene>.Instance.IsChangeRoom = true;
					SingletonMono<NetManager, AllScene>.Instance.SendJoinRoom();
					TipManager.Instance.ShowWaitTip("正在切换房间...");
				}
			}
			else
			{
				TipManager.Instance.ShowConfirmBar("加入房间至少需要" + SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[curSiteIndex].joinRoomMinCoin.ToString() + "的金币,您的金币不够,不能加入!", delegate
				{
					NiuNiuNetManager.Instance.SendLeaveRoom();
					TipManager.Instance.ShowWaitTip("正在退出房间...");
				}, null);
			}
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
		}
		private void OnOpenFriendBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			SingletonMono<NetManager, AllScene>.Instance.SendGetBuddyList();
			TipManager.Instance.ShowWaitTip(string.Empty);
		}
		private void OnGameChatBtnClick()
		{
			GameChatManager.Instance.ShowGameChat();
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
		}
	}
}
