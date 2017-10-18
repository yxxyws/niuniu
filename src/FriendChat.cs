using com.max.JiXiangLobby;
using System;
using System.Collections.Generic;
using UnityEngine;
public class FriendChat : MonoBehaviour
{
	public TweenPosition tween_friendChat;
	public UIButton btn_mask;
	public UIButton btn_back;
	public UILabel label_friendNick;
	public UIInput input_chatText;
	public UIButton btn_send;
	public UIButton btn_voice;
	public UIScrollView scrollView_chat;
	public UIScrollBar scrollBar_chat;
	public UITable table_chat;
	public PoolManager pool_chatText;
	private Texture2D friendHead;
	private UserInfo userInfo;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		EventDelegate.Set(this.btn_send.onClick, new EventDelegate.Callback(this.OnSendBtnClick));
		EventDelegate.Set(this.btn_voice.onClick, new EventDelegate.Callback(this.OnVoiceBtnClick));
		UIEventListener expr_7F = this.input_chatText.GetComponent<UIEventListener>();
		expr_7F.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(expr_7F.onSelect, new UIEventListener.BoolDelegate(this.InputOnSelect));
	}
	private void Start()
	{
	}
	public void InitShow(UserInfo info)
	{
		this.userInfo = info;
		this.label_friendNick.text = info.nick;
		List<ChatText> list = FriendManager.Instance.ForIdGetChatRecord(info.username);
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].sendId == SingletonMono<DataManager, AllScene>.Instance.username)
				{
					this.AddSelfSendChatInfo(list[i].chatInfo);
				}
				else
				{
					this.AddFriendSendChatInfo(list[i].chatInfo);
				}
			}
		}
		this.tween_friendChat.PlayForward();
		base.Invoke("UpdateTablePos", this.tween_friendChat.duration);
	}
	private void UpdateTablePos()
	{
		this.table_chat.repositionNow = true;
		this.UpdateScrollViewShow();
	}
	public void AddSelfSendChatInfo(string str)
	{
		GameObject nGUIItem = this.pool_chatText.GetNGUIItem();
		nGUIItem.GetComponent<Prefab_chatText>().InitShow(true, str, string.Empty);
		this.table_chat.repositionNow = true;
		base.Invoke("UpdateScrollViewShow", Time.deltaTime * 2f);
	}
	public void AddFriendSendChatInfo(string str)
	{
		GameObject nGUIItem = this.pool_chatText.GetNGUIItem();
		nGUIItem.GetComponent<Prefab_chatText>().InitShow(false, str, this.userInfo.headUrl);
		this.table_chat.repositionNow = true;
		base.Invoke("UpdateScrollViewShow", Time.deltaTime * 2f);
	}
	private void UpdateScrollViewShow()
	{
		if (this.scrollView_chat.shouldMoveVertically)
		{
			this.scrollBar_chat.value = 1f;
		}
	}
	private void OnMaskBtnClick()
	{
		FriendManager.Instance.ResetCurOpenChatId();
		this.tween_friendChat.PlayReverse();
		base.Invoke("AnimCallback", this.tween_friendChat.duration);
	}
	private void AnimCallback()
	{
		this.pool_chatText.ResetAllIdleItem();
		base.gameObject.SetActive(false);
	}
	private void OnBackBtnClick()
	{
		this.OnMaskBtnClick();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnSendBtnClick()
	{
		if (this.input_chatText.value != string.Empty)
		{
			SingletonMono<NetManager, AllScene>.Instance.SendChatText(this.userInfo.username, this.input_chatText.value);
			this.AddSelfSendChatInfo(this.input_chatText.value);
			this.input_chatText.value = string.Empty;
		}
		else
		{
			TipManager.Instance.ShowTips("不能发送空信息...", 2f);
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnVoiceBtnClick()
	{
		TipManager.Instance.ShowTips("功能开发中,请以后再尝试...", 2f);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void InputOnSelect(GameObject obj, bool isSelect)
	{
		SingletonMono<DataManager, AllScene>.Instance.IsSelectInput = isSelect;
	}
}
