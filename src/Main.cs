using System;
using UnityEngine;
public class Main : SingletonMono<Main, AllScene>
{
	private string appVer = string.Empty;
	private FSM<LobbyState> m_fsm;
	public LobbyState cutState = LobbyState.Login;
	public string AppVer
	{
		get
		{
			return this.appVer;
		}
	}
	public FSM<LobbyState> GetFSM()
	{
		return this.m_fsm;
	}
	protected override void Init()
	{
		Screen.sleepTimeout = -1;
		Application.targetFrameRate = 30;
		Application.runInBackground = true;
		this.appVer = Application.version;
		if (!PlayerPrefs.HasKey("AppVer"))
		{
			PlayerPrefs.SetString("AppVer", this.appVer);
		}
		this.m_fsm = new FSM<LobbyState>();
		this.m_fsm.InsertState(LobbyState.Login, new FsmFunc(this.LoginEnter), delegate
		{
		}, delegate
		{
		});
		this.m_fsm.InsertState(LobbyState.GameSelect, new FsmFunc(this.GameSelectEnter), delegate
		{
		}, delegate
		{
		});
		this.m_fsm.InsertState(LobbyState.SiteSelect, delegate
		{
		}, delegate
		{
		}, delegate
		{
		});
		this.m_fsm.InsertState(LobbyState.PlayGame, delegate
		{
		}, delegate
		{
		}, delegate
		{
		});
		this.cutState = LobbyState.Login;
	}
	private void Start()
	{
	}
	private void Update()
	{
		if (this.m_fsm != null)
		{
			this.m_fsm.Update();
		}
	}
	protected override void Quit()
	{
	}
	public void SetGameStates(LobbyState states)
	{
		this.cutState = states;
		this.m_fsm.SetCurrentState(states);
	}
	private void LoginEnter()
	{
		UIManager.Instance.ShowPanel(PUID.Login);
		if (PlayerPrefs.HasKey("username"))
		{
			TipManager.Instance.ShowWaitTip("正在登录...");
			SingletonMono<DataManager, AllScene>.Instance.loginType = 1;
			SingletonMono<DataManager, AllScene>.Instance.username = PlayerPrefs.GetString("username");
			SingletonMono<DataManager, AllScene>.Instance.nick = PlayerPrefs.GetString("nick");
			SingletonMono<DataManager, AllScene>.Instance.sex = PlayerPrefs.GetString("sex");
			SingletonMono<NetManager, AllScene>.Instance.Connect();
		}
	}
	private void GameSelectEnter()
	{
		Panel_Lobby panel_Lobby = (Panel_Lobby)UIManager.Instance.GetPanel(PUID.Lobby);
		if (panel_Lobby != null)
		{
			panel_Lobby.InitShow();
			UIManager.Instance.ShowPanel(PUID.Lobby);
		}
		else
		{
			Debug.LogError("不存在该ID的Panel");
		}
	}
}
