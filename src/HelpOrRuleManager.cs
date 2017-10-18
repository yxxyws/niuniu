using System;
using UnityEngine;
public class HelpOrRuleManager : MonoBehaviour
{
	public UIButton btn_mask;
	public TweenPosition tween_pos;
	public GameObject obj_help;
	public UIButton btn_qq;
	public UIButton btn_joinQqGroup01;
	public UIButton btn_joinQqGroup02;
	public UIButton btn_recharge;
	public GameObject obj_rule;
	public GameObject obj_ruleSelect;
	public UIButton btn_niuniu;
	public GameObject obj_niuniuRule;
	public UIButton btn_niuniu_mask;
	private void Awake()
	{
		this.tween_pos.ResetToBeginning();
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_qq.onClick, new EventDelegate.Callback(this.OnQQBtnClick));
		EventDelegate.Set(this.btn_joinQqGroup01.onClick, new EventDelegate.Callback(this.OnJoinQQGroup01BtnClick));
		EventDelegate.Set(this.btn_joinQqGroup02.onClick, new EventDelegate.Callback(this.OnJoinQQGroup02BtnClick));
		EventDelegate.Set(this.btn_recharge.onClick, new EventDelegate.Callback(this.OnRechargeBtnClick));
		EventDelegate.Set(this.btn_niuniu.onClick, new EventDelegate.Callback(this.OnNiuNiuBtnClick));
		EventDelegate.Set(this.btn_niuniu_mask.onClick, new EventDelegate.Callback(this.OnNiuNiuRuleMaskBtnClick));
	}
	public void ShowBar(bool isOpenHelp)
	{
		if (isOpenHelp)
		{
			this.obj_help.SetActive(true);
			this.obj_rule.SetActive(false);
		}
		else
		{
			this.obj_help.SetActive(false);
			this.obj_rule.SetActive(true);
		}
		this.btn_mask.gameObject.SetActive(true);
		this.tween_pos.PlayForward();
	}
	private void OnMaskBtnClick()
	{
		this.tween_pos.PlayReverse();
		base.Invoke("AnimCallback", this.tween_pos.duration);
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_close");
	}
	private void AnimCallback()
	{
		this.btn_mask.gameObject.SetActive(false);
	}
	private void OnQQBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnJoinQQGroup01BtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnJoinQQGroup02BtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnRechargeBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnNiuNiuBtnClick()
	{
		this.obj_ruleSelect.SetActive(false);
		this.obj_niuniuRule.SetActive(true);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnNiuNiuRuleMaskBtnClick()
	{
		this.obj_ruleSelect.SetActive(true);
		this.obj_niuniuRule.SetActive(false);
	}
}
