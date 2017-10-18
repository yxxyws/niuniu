using com.max.JiXiangNiuNiu;
using System;
using UnityEngine;
namespace NiuNiu
{
	public class PlayerBase : MonoBehaviour
	{
		protected string username = string.Empty;
		protected string nick = string.Empty;
		protected double coin;
		protected double diamond;
		protected string sex = "0";
		protected string headUrl = string.Empty;
		public UIButton btn_head;
		public UITexture pic_head;
		public UILabel label_nick;
		public UILabel label_coin;
		public Texture head;
		protected bool isInit;
		public string Nick
		{
			get
			{
				return this.nick;
			}
		}
		private void Awake()
		{
			EventDelegate.Set(this.btn_head.onClick, new EventDelegate.Callback(this.OnPlayerBtnClick));
			this.AwakeInit();
		}
		public virtual void AwakeInit()
		{
		}
		public void SetPlayerInfo(PlayerInfo info)
		{
			this.username = info.username;
			this.nick = info.nick;
			this.coin = info.coin;
			this.diamond = info.diamond;
			this.sex = info.sex;
			this.headUrl = info.headUrl;
			this.ShowPlayer();
		}
		public void ShowPlayer()
		{
			this.label_nick.text = this.nick;
			this.label_coin.text = DataManager.ChangeDanWei(this.coin);
			if (!this.isInit)
			{
				this.isInit = true;
				this.pic_head.mainTexture = this.head;
				AsyncImageDownload.Instance.SetAsyncImage(this.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
			}
			this.btn_head.gameObject.SetActive(true);
			this.pic_head.gameObject.SetActive(true);
			this.label_coin.gameObject.SetActive(true);
			this.label_nick.gameObject.SetActive(true);
		}
		protected void AsyncGetHeadCallback(Texture2D tex)
		{
			this.pic_head.mainTexture = tex;
		}
		public void HidePlayer()
		{
			this.isInit = false;
			this.btn_head.gameObject.SetActive(false);
			this.pic_head.gameObject.SetActive(false);
			this.label_coin.gameObject.SetActive(false);
			this.label_nick.gameObject.SetActive(false);
		}
		public void UpdatePlayer()
		{
			this.label_nick.text = this.nick;
			this.label_coin.text = DataManager.ChangeDanWei(this.coin);
		}
		private void OnPlayerBtnClick()
		{
			if (this.username != SingletonMono<DataManager, AllScene>.Instance.username)
			{
				FriendManager.Instance.ShowFriendInfoBar(this.username, this.nick, this.headUrl, this.coin, this.diamond);
			}
		}
	}
}
