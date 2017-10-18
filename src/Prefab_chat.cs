using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class Prefab_chat : MonoBehaviour
{
	public UITexture pic_head;
	public GameObject obj_hintPoint;
	public UILabel lable_nick;
	public UILabel label_chatText;
	public UILabel lable_time;
	public UIButton btn_chat;
	private UserInfo curUserInfo;
	private void Awake()
	{
		EventDelegate.Set(this.btn_chat.onClick, new EventDelegate.Callback(this.OnChatBtnClick));
	}
	public void InitShow(UserInfo userInfo, ChatText chatText)
	{
		this.curUserInfo = userInfo;
		AsyncImageDownload.Instance.SetAsyncImage(userInfo.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
		this.lable_nick.text = this.curUserInfo.nick;
		this.UpdateShow(chatText);
	}
	private void AsyncGetHeadCallback(Texture2D tex)
	{
		this.pic_head.mainTexture = tex;
	}
	public void UpdateShow(ChatText chatText)
	{
		this.label_chatText.text = chatText.chatInfo;
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds((double)chatText.time);
		this.IsShowTipPoint(true);
		this.lable_time.text = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
	}
	public void IsShowTipPoint(bool isShow)
	{
		this.obj_hintPoint.SetActive(isShow);
	}
	public bool TipPointIsShow()
	{
		return this.obj_hintPoint.activeSelf;
	}
	private void OnChatBtnClick()
	{
		FriendManager.Instance.ShowFriendChat(this.curUserInfo);
	}
}
