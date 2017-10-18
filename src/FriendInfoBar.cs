using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class FriendInfoBar : MonoBehaviour
{
	public TweenPosition tween_FriendInfo;
	public UIButton btn_mask;
	public UIToggle toggle_info;
	public UIToggle toggle_prop;
	public UIToggle toggle_archives;
	public GameObject obj_InfoBar;
	public GameObject obj_PropBar;
	public GameObject obj_Archives;
	public UITexture pic_head;
	public UILabel label_nick;
	public UILabel label_coin;
	public UILabel label_diamond;
	public UILabel label_charm;
	public UILabel label_levle;
	public UILabel label_exp;
	public UISprite pic_exp;
	public UIButton btn_daiChong;
	public UIButton btn_priateChat;
	public UIButton btn_appreciate;
	public UIButton btn_despise;
	public UIButton btn_addFriend;
	public UIToggle[] toggle_allProp;
	public GameObject obj_Btn;
	public UIButton btn_give;
	public UIButton btn_giveTable;
	private UserInfo userInfo;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.toggle_info.onChange, new EventDelegate.Callback(this.OnInfoToggleChange));
		EventDelegate.Set(this.toggle_prop.onChange, new EventDelegate.Callback(this.OnPropToggleChange));
		EventDelegate.Set(this.toggle_archives.onChange, new EventDelegate.Callback(this.OnArchivesToggleChange));
		EventDelegate.Set(this.btn_priateChat.onClick, new EventDelegate.Callback(this.OnPrivateChatBtnClick));
		EventDelegate.Set(this.btn_addFriend.onClick, new EventDelegate.Callback(this.OnAddFriendBtnClick));
	}
	public void InitShow(UserInfo info)
	{
		this.userInfo = info;
		AsyncImageDownload.Instance.SetAsyncImage(info.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
		this.label_nick.text = info.nick;
		this.label_coin.text = DataManager.ChangeDanWei(info.coin);
		this.label_diamond.text = DataManager.ChangeDanWei(info.diamond);
		this.tween_FriendInfo.PlayForward();
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
		bool flag = FriendManager.Instance.friendBar.PlayerIsFriend(info.username);
		if (flag)
		{
			this.btn_priateChat.isEnabled = true;
			this.btn_addFriend.gameObject.SetActive(false);
		}
		else
		{
			this.btn_priateChat.isEnabled = false;
			this.btn_addFriend.gameObject.SetActive(true);
		}
		this.toggle_archives.Set(false, true);
		this.toggle_prop.Set(false, true);
		this.toggle_info.Set(true, true);
	}
	public void InitShow(string username, string nick, string headUrl, double coin, double diamond)
	{
		this.InitShow(new UserInfo
		{
			username = username,
			nick = nick,
			headUrl = headUrl,
			coin = coin,
			diamond = diamond,
			userState = 1
		});
	}
	private void AsyncGetHeadCallback(Texture2D tex)
	{
		this.pic_head.mainTexture = tex;
	}
	private void OnMaskBtnClick()
	{
		this.tween_FriendInfo.PlayReverse();
		base.Invoke("AnimCallback", this.tween_FriendInfo.duration);
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_close");
	}
	private void AnimCallback()
	{
		base.gameObject.SetActive(false);
	}
	private void OnInfoToggleChange()
	{
		if (this.toggle_info.value)
		{
			this.obj_InfoBar.SetActive(true);
			this.obj_PropBar.SetActive(false);
			this.obj_Archives.SetActive(false);
		}
	}
	private void OnPropToggleChange()
	{
		if (this.toggle_prop.value)
		{
			this.obj_InfoBar.SetActive(false);
			this.obj_PropBar.SetActive(true);
			this.obj_Archives.SetActive(false);
		}
	}
	private void OnArchivesToggleChange()
	{
		if (this.toggle_archives.value)
		{
			this.obj_InfoBar.SetActive(false);
			this.obj_PropBar.SetActive(false);
			this.obj_Archives.SetActive(true);
		}
	}
	private void OnDaiChongBtnClick()
	{
	}
	private void OnPrivateChatBtnClick()
	{
		FriendManager.Instance.ShowFriendChat(this.userInfo);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnAppreciateBtnClick()
	{
	}
	private void OnDespiseBtnClick()
	{
	}
	private void OnAddFriendBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.userInfo.username == SingletonMono<DataManager, AllScene>.Instance.username)
		{
			TipManager.Instance.ShowTips("不能添加自己为好友", 2f);
		}
		else
		{
			SingletonMono<NetManager, AllScene>.Instance.SendAddFriend(this.userInfo.username);
			TipManager.Instance.ShowTips("请等待对方的回应", 2f);
		}
	}
	private void OnPropBtnClick()
	{
	}
	private void OnGiveBtnClick()
	{
	}
	private void OnGiveTableBtnClick()
	{
	}
}
