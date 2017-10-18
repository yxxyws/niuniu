using System;
using UnityEngine;
public class DaiLiBar : MonoBehaviour
{
	public UIButton btn_confirm;
	public UIButton btn_quit;
	public UIInput input_id;
	private void Awake()
	{
		EventDelegate.Set(this.btn_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBtnClick));
		EventDelegate.Set(this.btn_quit.onClick, new EventDelegate.Callback(this.OnQuitBtnClick));
	}
	public void OnConfirmBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.input_id.value == string.Empty)
		{
			TipManager.Instance.ShowTips("代理帐号不能为null", 2f);
		}
		else
		{
			SingletonMono<DataManager, AllScene>.Instance.agentname = this.input_id.value;
			SingletonMono<NetManager, AllScene>.Instance.SendDaiLiInfo(this.input_id.value);
		}
	}
	public void OnQuitBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		Application.Quit();
	}
}
