using com.max.JiXiangNiuNiu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace NiuNiu
{
	public class PlayerPokerManager : MonoBehaviour
	{
		public Player player;
		public UISprite[] allPokers;
		public UIButton btn_openPoker;
		public TweenScale tween_OpenPoker;
		private List<Card> allPokerInfo = new List<Card>();
		private int cowCount;
		public int CowCount
		{
			get
			{
				return this.cowCount;
			}
		}
		private void Awake()
		{
			EventDelegate.Set(this.btn_openPoker.onClick, new EventDelegate.Callback(this.OnOpenPokerBtnClick));
		}
		public void PlaySendPokerAnim(float delay)
		{
			for (int i = 0; i < this.allPokers.Length; i++)
			{
				this.allPokers[i].GetComponent<TweenPosition>().ResetToBeginning();
				this.allPokers[i].GetComponent<TweenScale>().ResetToBeginning();
				this.allPokers[i].GetComponent<TweenScale>().duration = 0.1f;
				this.allPokers[this.allPokers.Length - 1].GetComponent<TweenScale>().from = new Vector3(0f, 0f, 0f);
				this.allPokers[this.allPokers.Length - 1].GetComponent<TweenScale>().to = new Vector3(1f, 1f, 1f);
				this.allPokers[i].GetComponent<TweenRotation>().ResetToBeginning();
			}
			base.StartCoroutine(this.SendPoker(delay));
		}
		[DebuggerHidden]
		private IEnumerator SendPoker(float delay)
		{
			PlayerPokerManager.<SendPoker>c__Iterator0 <SendPoker>c__Iterator = new PlayerPokerManager.<SendPoker>c__Iterator0();
			<SendPoker>c__Iterator.delay = delay;
			<SendPoker>c__Iterator.$this = this;
			return <SendPoker>c__Iterator;
		}
		public void ShowPoker()
		{
			for (int i = 0; i < this.allPokers.Length; i++)
			{
				this.allPokers[i].transform.localPosition = this.allPokers[i].GetComponent<TweenPosition>().to;
				this.allPokers[i].transform.localScale = this.allPokers[i].GetComponent<TweenScale>().to;
				this.allPokers[i].transform.localEulerAngles = this.allPokers[i].GetComponent<TweenRotation>().to;
			}
		}
		public void UpdateAllPokerShow(int index)
		{
			for (int i = 0; i < this.allPokers.Length; i++)
			{
				this.allPokerInfo.Add(GameDataManager.Instance.allCards[index * 5 + i]);
				string text = string.Empty;
				text += this.allPokerInfo[i].value.ToString();
				switch (this.allPokerInfo[i].color)
				{
				case 0:
					text += "D";
					break;
				case 1:
					text += "C";
					break;
				case 2:
					text += "B";
					break;
				case 3:
					text += "A";
					break;
				}
				if (i == this.allPokers.Length - 1)
				{
					this.allPokers[i].spriteName = "PokerA";
				}
				else
				{
					this.allPokers[i].spriteName = text;
				}
				this.allPokers[i].gameObject.SetActive(true);
			}
		}
		public void HideAllPokers()
		{
			for (int i = 0; i < this.allPokers.Length; i++)
			{
				this.allPokers[i].gameObject.SetActive(false);
			}
			this.allPokerInfo.Clear();
		}
		public int GetCowCount()
		{
			this.cowCount = 0;
			for (int i = 0; i < this.allPokerInfo.Count - 2; i++)
			{
				for (int j = i + 1; j < this.allPokerInfo.Count - 1; j++)
				{
					for (int k = j + 1; k < this.allPokerInfo.Count; k++)
					{
						int cardCount = this.GetCardCount(i);
						int cardCount2 = this.GetCardCount(j);
						int cardCount3 = this.GetCardCount(k);
						List<int> list = new List<int>();
						int num = cardCount + cardCount2 + cardCount3;
						int num2 = 0;
						if (num % 10 == 0)
						{
							list.Add(i);
							list.Add(j);
							list.Add(k);
							for (int l = 0; l < this.allPokerInfo.Count; l++)
							{
								if (!list.Contains(l))
								{
									list.Add(l);
									num2 += this.GetCardCount(l);
								}
							}
							if (num2 % 10 == 0)
							{
								for (int m = 0; m < this.allPokerInfo.Count; m++)
								{
									if (this.allPokerInfo[m].value < 10)
									{
										num2 = 10;
										break;
									}
									if (m == this.allPokerInfo.Count - 1)
									{
										num2 = 11;
									}
								}
							}
							else
							{
								num2 %= 10;
							}
							if (num2 > this.cowCount)
							{
								this.cowCount = num2;
							}
						}
					}
				}
			}
			return this.cowCount;
		}
		public int GetMultiple()
		{
			if (this.cowCount == 0)
			{
				return 1;
			}
			if (this.cowCount == 11)
			{
				return 15;
			}
			return this.cowCount;
		}
		private int GetCardCount(int index)
		{
			return (this.allPokerInfo[index].value < 10) ? this.allPokerInfo[index].value : 10;
		}
		public void PlayOpenPokerAnim()
		{
			this.tween_OpenPoker.delay = 0f;
			this.tween_OpenPoker.duration = 0.2f;
			this.tween_OpenPoker.to = new Vector3(1f, 1f, 1f);
			this.tween_OpenPoker.from = new Vector3(0f, 1f, 1f);
			this.tween_OpenPoker.PlayReverse();
			base.Invoke("AnimCallback", this.tween_OpenPoker.duration);
		}
		private void AnimCallback()
		{
			this.OpenPoker();
			this.tween_OpenPoker.PlayForward();
			base.Invoke("ShowCowCount", this.tween_OpenPoker.duration);
		}
		public void OpenPoker()
		{
			string text = string.Empty;
			text += this.allPokerInfo[this.allPokers.Length - 1].value.ToString();
			switch (this.allPokerInfo[this.allPokers.Length - 1].color)
			{
			case 0:
				text += "D";
				break;
			case 1:
				text += "C";
				break;
			case 2:
				text += "B";
				break;
			case 3:
				text += "A";
				break;
			}
			this.allPokers[this.allPokers.Length - 1].spriteName = text;
			this.IsShowOpenPokerBtn(false);
		}
		private void ShowCowCount()
		{
			this.player.ShowCowCount();
		}
		public void IsShowOpenPokerBtn(bool isShow)
		{
			this.btn_openPoker.gameObject.SetActive(isShow);
		}
		private void OnOpenPokerBtnClick()
		{
			this.PlayOpenPokerAnim();
			NiuNiuNetManager.Instance.SendOpenPokerInfo();
			this.IsShowOpenPokerBtn(false);
		}
	}
}
