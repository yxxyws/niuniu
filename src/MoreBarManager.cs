using System;
using UnityEngine;
public class MoreBarManager : MonoBehaviour
{
	public UIButton btn_mask;
	public UIButton btn_help;
	public UIButton btn_rule;
	public TweenPosition tween_pos;
	public HelpOrRuleManager helpOrRuleManager;
	private void Awake()
	{
		this.tween_pos.ResetToBeginning();
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_help.onClick, new EventDelegate.Callback(this.OnHelpBtnClick));
		EventDelegate.Set(this.btn_rule.onClick, new EventDelegate.Callback(this.OnRuleBtnClick));
	}
	public void PlayAnim()
	{
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
	private void OnHelpBtnClick()
	{
		this.helpOrRuleManager.ShowBar(true);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnRuleBtnClick()
	{
		this.helpOrRuleManager.ShowBar(false);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
