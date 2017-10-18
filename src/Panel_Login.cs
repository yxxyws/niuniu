using cn.sharesdk.unity3d;
using System;
using System.Collections;
public class Panel_Login : Panel_Basic
{
	public UIButton btn_login_wechat;
	public ShareSDK ssdk;
	protected override void setPanelType()
	{
		this.mType = PanelType.Normal;
	}
	protected override void init()
	{
		EventDelegate.Set(this.btn_login_wechat.onClick, new EventDelegate.Callback(this.OnWeChatLoginBtnClick));
		this.ssdk.authHandler = new ShareSDK.EventHandler(this.AuthResultHandler);
		this.ssdk.showUserHandler = new ShareSDK.EventHandler(this.GetUserInfoResultHandler);
	}
	protected override void setUID()
	{
		this.mUID = PUID.Login;
	}
	public void OnWeChatLoginBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SingletonMono<DataManager, AllScene>.Instance.loginType = 0;
		TipManager.Instance.ShowWaitTip("正在登录...");
		this.ssdk.Authorize(PlatformType.WeChat);
	}
	private void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			this.ssdk.GetUserInfo(PlatformType.WeChat);
		}
		else
		{
			if (state == ResponseState.Fail)
			{
				TipManager.Instance.HideWaitTip();
				TipManager.Instance.ShowTips((string)result["msg"], 3f);
			}
			else
			{
				if (state == ResponseState.Cancel)
				{
					TipManager.Instance.HideWaitTip();
					TipManager.Instance.ShowTips("取消微信登录", 3f);
				}
			}
		}
	}
	private void GetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			SingletonMono<DataManager, AllScene>.Instance.wechatId = result["openid"].ToString();
			SingletonMono<DataManager, AllScene>.Instance.nick = result["nickname"].ToString();
			SingletonMono<DataManager, AllScene>.Instance.sex = result["sex"].ToString();
			if (SingletonMono<DataManager, AllScene>.Instance.sex == "2")
			{
				SingletonMono<DataManager, AllScene>.Instance.sex = "0";
			}
			string text = result["headimgurl"].ToString();
			if (text != string.Empty)
			{
				SingletonMono<DataManager, AllScene>.Instance.headUrl = text;
			}
			if (SingletonMono<NetManager, AllScene>.Instance.IsConnect)
			{
				SingletonMono<NetManager, AllScene>.Instance.Login();
			}
			else
			{
				SingletonMono<NetManager, AllScene>.Instance.Connect();
			}
		}
		else
		{
			if (state != ResponseState.Fail)
			{
				if (state == ResponseState.Cancel)
				{
				}
			}
		}
	}
}
