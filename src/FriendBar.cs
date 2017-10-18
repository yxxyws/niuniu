using com.max.JiXiangLobby;
using System;
using System.Collections.Generic;
using UnityEngine;
public class FriendBar : MonoBehaviour
{
	public TweenPosition tween_friendBar;
	public UIButton btn_mask;
	public UIToggle toggle_add;
	public UIToggle toggle_friend;
	public UIToggle toggle_chat;
	public GameObject obj_addFriend;
	public GameObject obj_friend;
	public GameObject obj_chat;
	public GameObject obj_addFriendHintPoint;
	public GameObject obj_chatHintPoint;
	public UILabel label_selfID;
	public UIButton btn_search;
	public UIInput input_username;
	public UIButton btn_facing_add;
	public UIButton btn_recommend;
	public GameObject prefab_friend;
	public PoolManager pool_friend;
	public UIGrid grid_friend;
	public Dictionary<string, Prefab_Friend> allFriends = new Dictionary<string, Prefab_Friend>();
	public PoolManager pool_chat;
	public UIGrid grid_chat;
	public Dictionary<string, Prefab_chat> allFriendChat = new Dictionary<string, Prefab_chat>();
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.toggle_add.onChange, new EventDelegate.Callback(this.OnAddToggleChange));
		EventDelegate.Set(this.toggle_friend.onChange, new EventDelegate.Callback(this.OnFriendToggleChange));
		EventDelegate.Set(this.toggle_chat.onChange, new EventDelegate.Callback(this.OnChatToggleChange));
		EventDelegate.Set(this.btn_search.onClick, new EventDelegate.Callback(this.OnSearchBtnClick));
		EventDelegate.Set(this.btn_facing_add.onClick, new EventDelegate.Callback(this.OnFacingAddBtnClick));
		EventDelegate.Set(this.btn_recommend.onClick, new EventDelegate.Callback(this.OnRecommendBtnClick));
		UIEventListener expr_D6 = this.input_username.GetComponent<UIEventListener>();
		expr_D6.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(expr_D6.onSelect, new UIEventListener.BoolDelegate(this.InputOnSelect));
	}
	public void InitShow(List<UserInfo> allFriendInfo)
	{
		this.label_selfID.text = "告诉好友我的ID是：" + SingletonMono<DataManager, AllScene>.Instance.username;
		this.UpdateFriendShow(allFriendInfo);
		this.toggle_friend.Set(false, true);
		this.toggle_chat.Set(false, true);
		this.toggle_add.Set(true, true);
		this.tween_friendBar.PlayForward();
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
	}
	public void UpdateFriendShow(List<UserInfo> allFriendInfo)
	{
		for (int i = 0; i < allFriendInfo.Count; i++)
		{
			UserInfo userInfo = allFriendInfo[i];
			if (this.allFriends.ContainsKey(userInfo.username))
			{
				this.allFriends[userInfo.username].UpdateShow(userInfo);
			}
			else
			{
				Prefab_Friend component = this.pool_friend.GetNGUIItem().GetComponent<Prefab_Friend>();
				component.UpdateShow(userInfo);
				if (userInfo.isAdd == 1)
				{
					component.transform.SetAsFirstSibling();
				}
				this.allFriends.Add(userInfo.username, component);
			}
		}
		this.obj_addFriendHintPoint.SetActive(this.CheckIsShowAddFriendTipPoint());
		this.UpdateFriendTip();
		this.grid_friend.repositionNow = true;
	}
	public void UpdateFriendTip()
	{
		if (this.CheckIsShowAddFriendTipPoint() || this.CheckIsShowChatTipPoint())
		{
			FriendManager.UpdateFriendTip(true);
		}
		else
		{
			FriendManager.UpdateFriendTip(false);
		}
	}
	public void RemoveFriend(List<UserInfo> allFriendInfo)
	{
		for (int i = 0; i < allFriendInfo.Count; i++)
		{
			UserInfo userInfo = allFriendInfo[i];
			if (this.allFriends.ContainsKey(userInfo.username))
			{
				GameObject gameObject = this.allFriends[userInfo.username].gameObject;
				this.pool_friend.ResetIdleItem(gameObject);
				this.allFriends.Remove(userInfo.username);
			}
			if (this.allFriendChat.ContainsKey(userInfo.username))
			{
				this.pool_chat.ResetIdleItem(this.allFriendChat[userInfo.username].gameObject);
				this.allFriendChat.Remove(userInfo.username);
			}
		}
		this.obj_addFriendHintPoint.SetActive(this.CheckIsShowAddFriendTipPoint());
		this.obj_chatHintPoint.SetActive(this.CheckIsShowChatTipPoint());
		this.UpdateFriendTip();
		this.grid_friend.repositionNow = true;
		TipManager.Instance.HideWaitTip();
	}
	public bool PlayerIsFriend(string id)
	{
		return this.allFriends.ContainsKey(id) && this.allFriends[id].PlayerInfo.isAdd != 1;
	}
	public void AddFriendChat(string id, ChatText chatText)
	{
		if (this.allFriends.ContainsKey(id))
		{
			if (this.allFriendChat.ContainsKey(id))
			{
				this.allFriendChat[id].transform.SetAsFirstSibling();
				this.allFriendChat[id].UpdateShow(chatText);
			}
			else
			{
				Prefab_chat component = this.pool_chat.GetNGUIItem().GetComponent<Prefab_chat>();
				component.InitShow(this.allFriends[id].PlayerInfo, chatText);
				component.transform.SetAsFirstSibling();
				this.allFriendChat.Add(id, component);
			}
		}
		else
		{
			this.RemoveFriendChat(id);
		}
		this.obj_chatHintPoint.SetActive(this.CheckIsShowChatTipPoint());
		this.UpdateFriendTip();
		this.grid_chat.repositionNow = true;
	}
	public void RemoveFriendChat(string id)
	{
		if (this.allFriendChat.ContainsKey(id))
		{
			GameObject gameObject = this.allFriendChat[id].gameObject;
			this.allFriendChat.Remove(id);
			this.pool_chat.ResetIdleItem(gameObject);
		}
	}
	public void HideChatTipPoint(string id)
	{
		if (this.allFriendChat.ContainsKey(id))
		{
			this.allFriendChat[id].IsShowTipPoint(false);
		}
		this.obj_chatHintPoint.SetActive(this.CheckIsShowChatTipPoint());
		this.UpdateFriendTip();
	}
	public bool CheckIsShowChatTipPoint()
	{
		foreach (string current in this.allFriendChat.Keys)
		{
			if (this.allFriendChat[current].TipPointIsShow())
			{
				return true;
			}
		}
		return false;
	}
	public bool CheckIsShowAddFriendTipPoint()
	{
		foreach (string current in this.allFriends.Keys)
		{
			if (this.allFriends[current].TipPointIsShow())
			{
				return true;
			}
		}
		return false;
	}
	private void OnMaskBtnClick()
	{
		this.tween_friendBar.PlayReverse();
		base.Invoke("AnimCallback", this.tween_friendBar.duration);
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_close");
	}
	private void AnimCallback()
	{
		base.gameObject.SetActive(false);
	}
	public void OnAddToggleChange()
	{
		if (this.toggle_add.value)
		{
			this.obj_addFriend.SetActive(true);
			this.obj_friend.SetActive(false);
			this.obj_chat.SetActive(false);
		}
	}
	public void OnFriendToggleChange()
	{
		if (this.toggle_friend.value)
		{
			this.obj_addFriend.SetActive(false);
			this.obj_friend.SetActive(true);
			this.obj_chat.SetActive(false);
		}
	}
	public void OnChatToggleChange()
	{
		if (this.toggle_chat.value)
		{
			this.obj_addFriend.SetActive(false);
			this.obj_friend.SetActive(false);
			this.obj_chat.SetActive(true);
		}
	}
	public void OnSearchBtnClick()
	{
		if (this.input_username.value != string.Empty && this.input_username.value != SingletonMono<DataManager, AllScene>.Instance.username)
		{
			TipManager.Instance.ShowWaitTip(string.Empty);
			SingletonMono<NetManager, AllScene>.Instance.SendSearchUserInfo(this.input_username.value);
		}
		else
		{
			TipManager.Instance.ShowTips("请输入其他玩家的ID", 2f);
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	public void OnRecommendBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("暂未开放此功能");
	}
	public void OnFacingAddBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("暂未开放此功能");
	}
	private void InputOnSelect(GameObject obj, bool isSelect)
	{
		SingletonMono<DataManager, AllScene>.Instance.IsSelectInput = isSelect;
	}
}
