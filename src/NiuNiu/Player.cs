using com.max.JiXiangNiuNiu;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace NiuNiu
{
	public class Player : PlayerBase
	{
		private bool isBanker;
		private int playerIndex;
		private int seat;
		private bool isNull = true;
		private int curIndex;
		private Queue<GameObject> allShowExpress = new Queue<GameObject>();
		private float timer = 3f;
		public GameDataManager gameData;
		public UILabel label_bankerScore;
		public UISprite pic_bankerTip;
		public UIButton btn_sitDown;
		public UIButton btn_downBet;
		public UILabel label_banker_downbet;
		public UILabel label_total_downbet;
		public UILabel label_self_downbet;
		public UISprite pic_cowCount;
		public UISprite pic_cowMultiple;
		public GameObject obj_downbet_light;
		public PlayerPokerManager pokerManager;
		public ExpressionShow[] allExpress;
		public UIGrid grid_expression;
		public UILabel label_chat;
		public bool IsBanker
		{
			get
			{
				return this.isBanker;
			}
			set
			{
				this.isBanker = value;
			}
		}
		public bool IsNull
		{
			get
			{
				return this.isNull;
			}
		}
		public override void AwakeInit()
		{
			EventDelegate.Set(this.btn_sitDown.onClick, new EventDelegate.Callback(this.OnSitDownBtnClick));
			EventDelegate.Set(this.btn_downBet.onClick, new EventDelegate.Callback(this.OnDownBetBtnClick));
		}
		private void Update()
		{
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
				if (this.timer <= 0f)
				{
					this.label_chat.gameObject.SetActive(false);
				}
			}
		}
		public void InitShow(int index)
		{
			this.HidePlayer();
			this.playerIndex = index;
			this.UpdateDownBetShow();
			for (int i = 0; i < this.allExpress.Length; i++)
			{
				this.allExpress[i].gameObject.SetActive(false);
			}
			this.label_chat.gameObject.SetActive(false);
		}
		public void ResetStart()
		{
			this.pokerManager.HideAllPokers();
			this.obj_downbet_light.SetActive(false);
			this.IsShowCowMultiple(false);
			this.IsShowCowCount(false, false);
			this.HideDownBetCountShow();
		}
		public void UpdateDownBetShow()
		{
			this.seat = (this.playerIndex - this.gameData.offsetSeat + 4) % 4;
			this.btn_downBet.GetComponent<UISprite>().spriteName = "pic_dict_0" + this.seat.ToString();
			this.btn_downBet.normalSprite = "pic_dict_0" + this.seat.ToString();
		}
		public new void SetPlayerInfo(PlayerInfo info)
		{
			this.username = info.username;
			this.nick = info.nick;
			this.coin = info.coin;
			this.diamond = info.diamond;
			this.sex = info.sex;
			this.headUrl = info.headUrl;
			this.isBanker = info.isBanker;
			this.ShowPlayer();
		}
		public new void ShowPlayer()
		{
			this.isNull = false;
			base.ShowPlayer();
			this.IsShowBankerTip(this.isBanker);
			this.btn_sitDown.gameObject.SetActive(false);
		}
		public void SetIsInit(bool _isInit)
		{
			this.isInit = false;
		}
		public new void HidePlayer()
		{
			base.HidePlayer();
			this.isNull = true;
			this.IsShowBankerTip(false);
			this.btn_sitDown.gameObject.SetActive(true);
		}
		public void IsShowBankerTip(bool isShow)
		{
			this.isBanker = isShow;
			this.pic_bankerTip.gameObject.SetActive(isShow);
			this.label_banker_downbet.gameObject.SetActive(isShow);
		}
		public void IsShowScore(bool isShow, double score = 0.0)
		{
			if (isShow)
			{
				if (score > 0.0)
				{
					this.label_bankerScore.text = "+" + score.ToString();
				}
				else
				{
					this.label_bankerScore.text = score.ToString();
				}
			}
			this.label_bankerScore.gameObject.SetActive(isShow);
		}
		public void IsCanDownBet(bool isCan)
		{
			this.btn_downBet.enabled = isCan;
		}
		public void IsShowBankerCount(bool isShow, double _coin = 0.0)
		{
			this.label_banker_downbet.text = _coin.ToString();
			this.label_banker_downbet.gameObject.SetActive(isShow);
		}
		public void UpdateBetNums()
		{
			if (this.gameData.SelfBetNumCount[this.seat] > 0.0)
			{
				this.label_self_downbet.text = this.gameData.SelfBetNumCount[this.seat].ToString();
				if (!this.label_self_downbet.gameObject.activeSelf)
				{
					this.label_self_downbet.gameObject.SetActive(true);
				}
			}
			if (this.gameData.AllBetNumsCount[this.seat] > 0.0)
			{
				this.label_total_downbet.text = this.gameData.AllBetNumsCount[this.seat].ToString();
				if (!this.label_total_downbet.gameObject.activeSelf)
				{
					this.label_total_downbet.gameObject.SetActive(true);
				}
			}
		}
		public void ShowAllPoker()
		{
			this.pokerManager.UpdateAllPokerShow(this.seat);
			this.pokerManager.ShowPoker();
		}
		public void IsShowCowCount(bool isShow, bool isPlayAnim)
		{
			if (isShow)
			{
				if (isPlayAnim)
				{
					this.pic_cowCount.GetComponent<TweenScale>().ResetToBeginning();
					this.pic_cowCount.GetComponent<TweenScale>().PlayForward();
					this.pokerManager.PlayOpenPokerAnim();
				}
				else
				{
					this.ShowCowCount();
					this.pokerManager.OpenPoker();
				}
			}
			else
			{
				this.pic_cowCount.gameObject.SetActive(false);
			}
		}
		public void ShowCowCount()
		{
			if (this.seat == this.gameData.BankerSeat)
			{
				this.pic_cowCount.spriteName = "pic_banker_" + this.pokerManager.GetCowCount();
			}
			else
			{
				this.pic_cowCount.spriteName = "pic_win_" + this.pokerManager.GetCowCount();
			}
			SoundManager.Instance.PlaySound(SoundType.EFFECT, "niu_" + this.pokerManager.GetCowCount());
			this.pic_cowCount.gameObject.SetActive(true);
		}
		public void IsShowCowMultiple(bool isShow)
		{
			if (isShow)
			{
				if (this.seat == this.gameData.BankerSeat)
				{
					this.pic_cowMultiple.spriteName = "pic_banker_x" + this.pokerManager.GetMultiple();
					this.pic_cowMultiple.gameObject.SetActive(true);
				}
				else
				{
					if (this.gameData.IsWins[this.seat])
					{
						this.pic_cowMultiple.spriteName = "pic_win_x" + this.pokerManager.GetMultiple();
						this.pic_cowMultiple.gameObject.SetActive(true);
						this.obj_downbet_light.gameObject.SetActive(true);
					}
				}
			}
			else
			{
				this.pic_cowMultiple.gameObject.SetActive(false);
				this.obj_downbet_light.gameObject.SetActive(false);
			}
		}
		public void HideDownBetCountShow()
		{
			this.label_self_downbet.gameObject.SetActive(false);
			this.label_total_downbet.gameObject.SetActive(false);
			if (this.gameData.BankerSeat == this.seat && this.gameData.bankerCurCoin <= 0.0 && this.gameData.BankerSeat != -1)
			{
				this.label_banker_downbet.gameObject.SetActive(false);
			}
		}
		public void UpdateAllPokerShow()
		{
			this.pokerManager.UpdateAllPokerShow(this.seat);
		}
		public void ShowExpression(int index)
		{
			if (this.playerIndex == 0 || this.playerIndex == 1)
			{
				this.allExpress[this.curIndex].transform.localPosition = new Vector3(this.grid_expression.cellWidth * -1f, 0f, 0f);
				this.allExpress[this.curIndex].transform.SetAsFirstSibling();
			}
			else
			{
				this.allExpress[this.curIndex].transform.localPosition = new Vector3(this.grid_expression.cellWidth, 0f, 0f);
				this.allExpress[this.curIndex].transform.SetAsLastSibling();
			}
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
			this.label_chat.text = GameDataManager.Instance.allDialogue[index];
			this.label_chat.gameObject.SetActive(true);
		}
		public void ShowChatText(string str)
		{
			this.timer = 3f;
			this.label_chat.text = str;
			this.label_chat.gameObject.SetActive(true);
		}
		private void OnSitDownBtnClick()
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			NiuNiuNetManager.Instance.SendSitDownInfo(this.seat);
			TipManager.Instance.ShowWaitTip(string.Empty);
		}
		private void OnDownBetBtnClick()
		{
			if (this.gameData.AllBetNumsCount[this.seat] + (double)this.gameData.ChipsNum[this.gameData.SelectBetIndex] >= this.gameData.bankerCurCoin * 0.15000000596046448)
			{
				TipManager.Instance.ShowFeedbackTipsBar("当前下注盘的下注量已经达到庄家分数的15%,不可继续下注");
			}
			else
			{
				NiuNiuNetManager.Instance.SendDownBetInfo(this.seat, this.gameData.ChipsNum[this.gameData.SelectBetIndex], this.gameData.SelectBetIndex);
			}
		}
	}
}
