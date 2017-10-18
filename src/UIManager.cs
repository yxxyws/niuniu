using com.max.JiXiangLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	public Panel_Basic[] allPanels;
	public Panel_Basic curPanel;
	private Hashtable mPanelTable = new Hashtable();
	private Stack mPopupPanelStack = new Stack();
	public Action UpdateUserInfoShow;
	public Action RefreshSafeShow;
	public Action InitAllRecord;
	public Action<List<Record>> LoadingRechargeRecord;
	public Action<Record> AddGiveRecord;
	public Action<Record> AddReceiveRecord;
	public Action<bool> IsShowDaiLiBar;
	public Action ShowTaskBar;
	public Action ShowSafeBar;
	public Action UpdateSignShow;
	public Action<string, string> ShowNoticeContent;
	private void Awake()
	{
		UIManager.Instance = this;
		this.Init();
	}
	private void Start()
	{
		SingletonMono<Main, AllScene>.Instance.SetGameStates(SingletonMono<Main, AllScene>.Instance.cutState);
	}
	public void Init()
	{
		this.CloseAllPopupPanel();
		this.mPanelTable.Clear();
		for (int i = 0; i < this.allPanels.Length; i++)
		{
			if (this.allPanels[i] == null)
			{
				Debug.LogError(string.Format("allPanels {0} cannot be null", i));
			}
			else
			{
				this.allPanels[i].Init(this);
				this.allPanels[i].Hide();
				if (this.allPanels[i].uID == PUID.NotSet)
				{
					Debug.LogError(string.Format("{0} UID not set", this.allPanels[i].GetType()));
				}
				else
				{
					if (this.mPanelTable.ContainsKey(this.allPanels[i].uID))
					{
						Debug.LogError(string.Format("{0} UID is repeated between {1} and {2}", this.allPanels[i].uID, this.allPanels[i].gameObject, ((Panel_Basic)this.mPanelTable[this.allPanels[i].uID]).gameObject));
					}
					else
					{
						this.mPanelTable.Add(this.allPanels[i].uID, this.allPanels[i]);
					}
				}
			}
		}
	}
	public void ShowPanel(PUID u_id)
	{
		if (this.mPanelTable.ContainsKey(u_id))
		{
			this.curPanel = (Panel_Basic)this.mPanelTable[u_id];
			if (this.curPanel.type == PanelType.Normal)
			{
				for (int i = 0; i < this.allPanels.Length; i++)
				{
					if (this.allPanels[i].IsShowing() && this.allPanels[i].type == PanelType.Normal)
					{
						this.allPanels[i].Hide();
					}
				}
			}
			else
			{
				if (this.curPanel.type == PanelType.Popup)
				{
					this.mPopupPanelStack.Push(this.curPanel);
				}
			}
			this.curPanel.Show();
		}
	}
	public void HidePanel(PUID u_id)
	{
		if (this.mPanelTable.ContainsKey(u_id))
		{
			((Panel_Basic)this.mPanelTable[u_id]).Hide();
		}
	}
	public Panel_Basic GetPanel(PUID u_id)
	{
		if (this.mPanelTable.ContainsKey(u_id))
		{
			return (Panel_Basic)this.mPanelTable[u_id];
		}
		return null;
	}
	public void ApplyQuit()
	{
		Application.Quit();
	}
	public void CloseAllPopupPanel()
	{
		while (this.mPopupPanelStack.Count > 0)
		{
			Panel_Basic panel_Basic = (Panel_Basic)this.mPopupPanelStack.Pop();
			this.HidePanel(panel_Basic.uID);
		}
		this.mPopupPanelStack.Clear();
	}
}
