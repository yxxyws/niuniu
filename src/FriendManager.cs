using com.max.JiXiangLobby;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class FriendManager : MonoBehaviour
{
	public static FriendManager Instance;
	public static Action<bool> UpdateFriendTip;
	public FriendBar friendBar;
	public AddFriendBar addFriendbar;
	public FriendInfoBar friendInfoBar;
	public FriendChat chatBar;
	public TweenPosition tween_FriendBar;
	public TweenPosition tween_AddFriendBar;
	public TweenPosition tween_FriendInfoBar;
	public TweenPosition tween_Chat;
	public Dictionary<string, List<ChatText>> allFriendChat = new Dictionary<string, List<ChatText>>();
	private string curOpenChatId = string.Empty;
	private static object m_lockBoo = new object();
	private void Awake()
	{
		object lockBoo = FriendManager.m_lockBoo;
		Monitor.Enter(lockBoo);
		try
		{
			if (FriendManager.Instance == null)
			{
				FriendManager.Instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		finally
		{
			Monitor.Exit(lockBoo);
		}
	}
	private void Start()
	{
		this.tween_FriendBar.ResetToBeginning();
		this.tween_AddFriendBar.ResetToBeginning();
		this.tween_FriendInfoBar.ResetToBeginning();
		this.tween_Chat.ResetToBeginning();
		this.friendBar.gameObject.SetActive(false);
		this.addFriendbar.gameObject.SetActive(false);
		this.friendInfoBar.gameObject.SetActive(false);
		this.chatBar.gameObject.SetActive(false);
	}
	public void ShowFriendBar(List<UserInfo> allFriendInfo)
	{
		this.friendBar.gameObject.SetActive(true);
		this.friendBar.InitShow(allFriendInfo);
	}
	public void ShowAddFriendBar(UserInfo info)
	{
		this.addFriendbar.gameObject.SetActive(true);
		this.addFriendbar.InitShow(info);
	}
	public void ShowFriendInfoBar(UserInfo info)
	{
		this.friendInfoBar.gameObject.SetActive(true);
		this.friendInfoBar.InitShow(info);
	}
	public void ShowFriendInfoBar(string username, string nick, string headUrl, double coin, double diamond)
	{
		this.friendInfoBar.gameObject.SetActive(true);
		this.friendInfoBar.InitShow(username, nick, headUrl, coin, diamond);
	}
	public void ShowFriendChat(UserInfo info)
	{
		this.curOpenChatId = info.username;
		this.chatBar.gameObject.SetActive(true);
		this.chatBar.InitShow(info);
	}
	public void ResetCurOpenChatId()
	{
		this.friendBar.HideChatTipPoint(this.curOpenChatId);
		this.curOpenChatId = string.Empty;
	}
	public void AddFriendChatText(string id, ChatText chatText)
	{
		this.friendBar.AddFriendChat(id, chatText);
		if (this.allFriendChat.ContainsKey(id))
		{
			if (this.allFriendChat[id].Count > 30)
			{
				this.allFriendChat[id].RemoveAt(0);
			}
			this.allFriendChat[id].Add(chatText);
		}
		else
		{
			this.allFriendChat.Add(id, new List<ChatText>());
			this.allFriendChat[id].Add(chatText);
		}
		if (this.curOpenChatId.Equals(id) && chatText.sendId != SingletonMono<DataManager, AllScene>.Instance.username)
		{
			this.chatBar.AddFriendSendChatInfo(chatText.chatInfo);
		}
	}
	public List<ChatText> ForIdGetChatRecord(string id)
	{
		if (this.allFriendChat.ContainsKey(id))
		{
			return this.allFriendChat[id];
		}
		return null;
	}
}
