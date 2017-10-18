using System;
using UnityEngine;
public class ShopManager : MonoBehaviour
{
	public TweenPosition tween_Pos;
	public UIButton btn_back;
	public UIToggle toggle_coinShop;
	public UIToggle toggle_diamondShop;
	public GameObject obj_mask;
	public UIButton[] btn_allCoinShop;
	private void Awake()
	{
		EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		EventDelegate.Set(this.toggle_coinShop.onChange, new EventDelegate.Callback(this.OnCoinShopToggleChange));
		EventDelegate.Set(this.toggle_diamondShop.onChange, new EventDelegate.Callback(this.OnDiamondToggleChange));
		EventDelegate.Set(this.tween_Pos.onFinished, new EventDelegate.Callback(this.OnTweenPosCallbacck));
		for (int i = 0; i < this.btn_allCoinShop.Length; i++)
		{
			EventDelegate.Set(this.btn_allCoinShop[i].onClick, new EventDelegate.Callback(this.OnCoinShopBtnClick));
		}
	}
	private void Start()
	{
		this.tween_Pos.ResetToBeginning();
	}
	public void PlayAnim()
	{
		this.obj_mask.SetActive(true);
		this.tween_Pos.PlayForward();
	}
	private void OnBackBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_mask.SetActive(true);
		this.tween_Pos.PlayReverse();
	}
	private void OnCoinShopToggleChange()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnDiamondToggleChange()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnTweenPosCallbacck()
	{
		this.obj_mask.SetActive(!this.obj_mask.activeSelf);
	}
	private void OnCoinShopBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("系统繁忙...");
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
