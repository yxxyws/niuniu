using System;
using UnityEngine;
public class TaskManager : MonoBehaviour
{
	public UILabel label_signText;
	public UIButton btn_mask;
	public UIButton btn_close;
	public UIButton btn_locking;
	public UIButton btn_sign;
	public UIButton btn_wechatShare;
	public GameObject obj_yetSign;
	public GameObject obj_complete;
	public GameObject obj_checkPhone;
	public UIInput input_phone;
	public UIInput input_yanZhengMa;
	public UIButton btn_get_YanZhengMa;
	public UIButton btn_confirm;
	public UIButton btn_chenkPhone_mask;
	public UIButton btn_checkPhone_close;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_locking.onClick, new EventDelegate.Callback(this.OnLockingBtnClick));
		EventDelegate.Set(this.btn_wechatShare.onClick, new EventDelegate.Callback(this.OnWechatShareBtnClick));
		EventDelegate.Set(this.btn_sign.onClick, new EventDelegate.Callback(this.OnSignBtnClick));
		EventDelegate.Set(this.btn_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBtnClick));
		EventDelegate.Set(this.btn_checkPhone_close.onClick, new EventDelegate.Callback(this.OnCheckPhoneCloseBtnClick));
		EventDelegate.Set(this.btn_chenkPhone_mask.onClick, new EventDelegate.Callback(this.OnCheckPhoneMaskBtnClick));
		EventDelegate.Set(this.btn_get_YanZhengMa.onClick, new EventDelegate.Callback(this.OnGetYanZhengMaBtnClick));
	}
	private void Start()
	{
		UIManager expr_05 = UIManager.Instance;
		expr_05.UpdateSignShow = (Action)Delegate.Combine(expr_05.UpdateSignShow, new Action(this.UpdateSignShow));
	}
	public void UpdateSignShow()
	{
		if (SingletonMono<DataManager, AllScene>.Instance.isCanSign)
		{
			this.label_signText.text = string.Concat(new object[]
			{
				"你已经连续签到",
				SingletonMono<DataManager, AllScene>.Instance.signCount,
				"天，今日签到可获得",
				SingletonMono<DataManager, AllScene>.Instance.signCoin,
				"金币"
			});
			this.btn_sign.gameObject.SetActive(true);
			this.obj_yetSign.SetActive(false);
		}
		else
		{
			this.label_signText.text = string.Concat(new object[]
			{
				"你已经连续签到",
				SingletonMono<DataManager, AllScene>.Instance.signCount,
				"天，明日签到可获得",
				SingletonMono<DataManager, AllScene>.Instance.signCoin,
				"金币"
			});
			this.btn_sign.gameObject.SetActive(false);
			this.obj_yetSign.SetActive(true);
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}
	private void OnCloseBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		base.gameObject.SetActive(false);
	}
	private void OnLockingBtnClick()
	{
		this.obj_checkPhone.SetActive(true);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnSignBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (SingletonMono<DataManager, AllScene>.Instance.isPlaying)
		{
			TipManager.Instance.ShowTips("你还在游戏中,不可以签到...", 2f);
		}
		else
		{
			SingletonMono<NetManager, AllScene>.Instance.SendSignInfo();
			TipManager.Instance.ShowWaitTip(string.Empty);
		}
	}
	private void OnWechatShareBtnClick()
	{
	}
	private void OnCheckPhoneMaskBtnClick()
	{
		this.obj_checkPhone.SetActive(false);
	}
	private void OnCheckPhoneCloseBtnClick()
	{
		this.OnCheckPhoneMaskBtnClick();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnGetYanZhengMaBtnClick()
	{
		if (this.input_phone.value == string.Empty)
		{
			TipManager.Instance.ShowFeedbackTipsBar("请输入正确的手机号码");
		}
		else
		{
			if (SingletonMono<DataManager, AllScene>.Instance.isPlaying)
			{
				TipManager.Instance.ShowFeedbackTipsBar("请完成当前正在游戏的回合在进行手机绑定");
			}
		}
	}
	private void OnConfirmBtnClick()
	{
		if (this.input_yanZhengMa.value == string.Empty)
		{
			TipManager.Instance.ShowFeedbackTipsBar("请输入正确的验证码");
		}
		else
		{
			if (SingletonMono<DataManager, AllScene>.Instance.isPlaying)
			{
				TipManager.Instance.ShowFeedbackTipsBar("请完成当前正在游戏的回合在进行手机绑定");
			}
		}
	}
}
