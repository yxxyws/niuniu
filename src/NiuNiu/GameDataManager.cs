using com.max.JiXiangNiuNiu;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace NiuNiu
{
	public class GameDataManager : MonoBehaviour
	{
		public static GameDataManager Instance;
		[HideInInspector]
		public bool isBanker;
		[HideInInspector]
		public int bankerTimes;
		[HideInInspector]
		public double bankerCurCoin;
		[HideInInspector]
		public double bankerMaxStake;
		[HideInInspector]
		public int offsetSeat;
		[HideInInspector]
		public double yetBetCount;
		[HideInInspector]
		public double totalDownBet;
		[HideInInspector]
		public List<Card> allCards = new List<Card>();
		[HideInInspector]
		public double score;
		[HideInInspector]
		public double bankerScore;
		[HideInInspector]
		public string[] allDialogue = new string[]
		{
			"大家好，很高兴见到各位.",
			"初来乍到，请大家手下留情.",
			"快点吧，等得我花儿都谢了.",
			"别拼了，没牛就是没牛.",
			"不好意思，又赢了！",
			"底裤都要输掉了.",
			"别高兴太早，等我回来！"
		};
		[HideInInspector]
		public bool isCanShowChangeTable;
		private int bankerSeat;
		private int seat = 6;
		private double[] allBetNumsCount = new double[4];
		private double[] selfBetNumCount = new double[4];
		private bool[] isWins = new bool[4];
		private int[] chipsNum = new int[]
		{
			100,
			1000,
			10000,
			100000,
			1000000
		};
		private int selectBetIndex;
		public int BankerSeat
		{
			get
			{
				return this.bankerSeat;
			}
			set
			{
				this.bankerSeat = value;
			}
		}
		public int Seat
		{
			get
			{
				return this.seat;
			}
			set
			{
				this.seat = value;
			}
		}
		public double[] AllBetNumsCount
		{
			get
			{
				return this.allBetNumsCount;
			}
			set
			{
				this.allBetNumsCount = value;
			}
		}
		public double[] SelfBetNumCount
		{
			get
			{
				return this.selfBetNumCount;
			}
			set
			{
				this.selfBetNumCount = value;
			}
		}
		public bool[] IsWins
		{
			get
			{
				return this.isWins;
			}
			set
			{
				this.isWins = value;
			}
		}
		public int[] ChipsNum
		{
			get
			{
				return this.chipsNum;
			}
		}
		public int SelectBetIndex
		{
			get
			{
				return this.selectBetIndex;
			}
			set
			{
				this.selectBetIndex = value;
			}
		}
		private void Awake()
		{
			GameDataManager.Instance = this;
		}
		public void InitInfo()
		{
			this.bankerSeat = 0;
		}
		public void ResStart()
		{
			for (int i = 0; i < this.allBetNumsCount.Length; i++)
			{
				this.allBetNumsCount[i] = 0.0;
				this.selfBetNumCount[i] = 0.0;
			}
			this.yetBetCount = 0.0;
			this.totalDownBet = 0.0;
			this.score = 0.0;
			this.bankerScore = 0.0;
		}
		public void SetBankerData(BankerInfo info)
		{
			if (info.bankerName == SingletonMono<DataManager, AllScene>.Instance.username)
			{
				this.isBanker = true;
			}
			else
			{
				this.isBanker = false;
			}
			this.bankerSeat = info.bankerSeat;
			this.bankerTimes = info.bankerTimes;
			GameMain.Instance.uiManager.UpdateBankerNum();
			this.bankerCurCoin = info.curCoin;
			this.bankerMaxStake = info.bankerMaxStake;
		}
		public int GetOffsetSeat(int seat)
		{
			if (seat > 3)
			{
				return seat;
			}
			return (seat + this.offsetSeat) % 4;
		}
	}
}
