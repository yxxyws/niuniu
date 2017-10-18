using System;
using UnityEngine;
public class GeneralizeManager : MonoBehaviour
{
	public UIButton btn_mask;
	public UIButton btn_close;
	public UIToggle toggle_flag1;
	public UIToggle toggle_flag2;
	public GameObject obj_tuiGuang;
	public GameObject obj_yanZheng;
	public UIButton btn_wechat;
	public UIButton btn_pengYouQuan;
	public UIButton btn_get;
	public UIButton btn_answer;
	public UILabel label_tuiGuangMa;
	public UILabel label_AlreadyReceiveCoin;
	public UILabel label_AlreadyReceiveDiamond;
	public UILabel label_tuiGuangPeopleCount;
	public UILabel label_coinCount;
	public UILabel label_diamondCount;
	public UIInput input_tuiGuangMa;
	public UIButton btn_check;
	private void Awake()
	{
		EventDelegate.Set(this.toggle_flag1.onChange, new EventDelegate.Callback(this.OnFlag1ToggleChange));
		EventDelegate.Set(this.toggle_flag2.onChange, new EventDelegate.Callback(this.OnFlag2ToggleChange));
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_get.onClick, new EventDelegate.Callback(this.OnGetBtnClick));
		EventDelegate.Set(this.btn_wechat.onClick, new EventDelegate.Callback(this.OnWeChatBtnClick));
		EventDelegate.Set(this.btn_check.onClick, new EventDelegate.Callback(this.OnCheckBtnClick));
	}
	public void InitShow()
	{
	}
	private void OnMaskBtnClick()
	{
		base.gameObject.SetActive(false);
	}
	private void OnCloseBtnClick()
	{
		base.gameObject.SetActive(false);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnFlag1ToggleChange()
	{
		if (this.toggle_flag1.value)
		{
			this.obj_tuiGuang.SetActive(true);
			this.obj_yanZheng.SetActive(false);
		}
	}
	private void OnFlag2ToggleChange()
	{
		if (this.toggle_flag2.value)
		{
			this.obj_tuiGuang.SetActive(false);
			this.obj_yanZheng.SetActive(true);
		}
	}
	private void OnGetBtnClick()
	{
		if (SingletonMono<DataManager, AllScene>.Instance.isPlaying)
		{
			TipManager.Instance.ShowFeedbackTipsBar("请在结束当前回合游戏后再进行领取...");
		}
	}
	private void OnWeChatBtnClick()
	{
	}
	private void OnCheckBtnClick()
	{
		if (this.input_tuiGuangMa.value == string.Empty)
		{
			TipManager.Instance.ShowTips("请输入正确的验证码", 2f);
		}
	}
}
