using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class AddFriendBar : MonoBehaviour
{
	public TweenPosition tween_addFriendBar;
	public UIButton btn_mask;
	public UIButton btn_back;
	public UIButton btn_friend;
	public UIButton btn_add;
	public UITexture pic_head;
	public UILabel label_nick;
	private UserInfo curAddUserInfo;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		EventDelegate.Set(this.btn_friend.onClick, new EventDelegate.Callback(this.OnFriendBtnClick));
		EventDelegate.Set(this.btn_add.onClick, new EventDelegate.Callback(this.OnAddBtnClick));
	}
	public void InitShow(UserInfo info)
	{
		this.curAddUserInfo = info;
		AsyncImageDownload.Instance.SetAsyncImage(info.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
		this.label_nick.text = info.nick;
		if (info.isAdd == 0 || info.isAdd == 1)
		{
			this.btn_add.gameObject.SetActive(false);
		}
		else
		{
			this.btn_add.gameObject.SetActive(true);
		}
		this.tween_addFriendBar.PlayForward();
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
	}
	private void AsyncGetHeadCallback(Texture2D tex)
	{
		this.pic_head.mainTexture = tex;
	}
	private void OnMaskBtnClick()
	{
		this.tween_addFriendBar.PlayReverse();
		base.Invoke("AnimCallback", this.tween_addFriendBar.duration);
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_close");
	}
	private void OnBackBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.OnMaskBtnClick();
	}
	private void AnimCallback()
	{
		base.gameObject.SetActive(false);
	}
	public void OnAddBtnClick()
	{
		this.btn_add.gameObject.SetActive(false);
		TipManager.Instance.ShowTips("请等待对方的回应", 2f);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SingletonMono<NetManager, AllScene>.Instance.SendAddFriend(this.curAddUserInfo.username);
	}
	public void OnFriendBtnClick()
	{
		FriendManager.Instance.ShowFriendInfoBar(this.curAddUserInfo);
	}
}
