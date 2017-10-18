using System;
using UnityEngine;
public class Panel_Tips : MonoBehaviour
{
	public static Panel_Tips Instance;
	public UILabel label_tip;
	public UILabel label_tipBar;
	public UILabel label_wait;
	public UILabel label_confirmBar;
	public UIButton btn_confirmBar_confirm;
	public UIButton btn_confirmBar_cancle;
	public UIButton btn_tipBar_confirm;
	private float showTipTimer;
	private Action tipBarBtnCallback;
	private Action confirmBarBtnCallback;
	private void Awake()
	{
		Panel_Tips.Instance = this;
		EventDelegate.Set(this.btn_tipBar_confirm.onClick, new EventDelegate.Callback(this.OnTipBarConfimBtnClick));
		EventDelegate.Set(this.btn_confirmBar_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBarConfirmClick));
		EventDelegate.Set(this.btn_confirmBar_cancle.onClick, new EventDelegate.Callback(this.OnConfirmBarCancleClick));
	}
	private void Start()
	{
	}
	private void Update()
	{
		if (this.showTipTimer > 0f)
		{
			this.showTipTimer -= Time.deltaTime;
			if (this.showTipTimer <= 0f)
			{
				this.label_tip.gameObject.SetActive(false);
			}
		}
	}
	public void ShowTips(string info, float time)
	{
		this.label_tip.text = info;
		this.showTipTimer = time;
		if (time <= 0f)
		{
			this.label_tip.gameObject.SetActive(false);
			return;
		}
		this.label_tip.gameObject.SetActive(true);
	}
	public void ShowTipsBar(string info, Action callback = null)
	{
		this.label_tipBar.text = info;
		this.tipBarBtnCallback = callback;
		this.label_tipBar.gameObject.SetActive(true);
	}
	public void ShowConfirmBar(string info, Action callback)
	{
		this.label_confirmBar.text = info;
		this.confirmBarBtnCallback = callback;
		this.label_confirmBar.gameObject.SetActive(true);
	}
	public void HideTipBar()
	{
		this.label_tipBar.gameObject.SetActive(false);
	}
	public void ShowWaitTip(string str)
	{
		this.label_wait.text = str;
		this.label_wait.gameObject.SetActive(true);
	}
	public void HideWaitTip()
	{
		this.label_wait.gameObject.SetActive(false);
	}
	private void OnTipBarConfimBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.tipBarBtnCallback != null)
		{
			this.tipBarBtnCallback();
		}
		else
		{
			UIManager.Instance.ApplyQuit();
		}
	}
	public void OnConfirmBarConfirmClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.confirmBarBtnCallback();
		this.label_confirmBar.gameObject.SetActive(false);
	}
	public void OnConfirmBarCancleClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.label_confirmBar.gameObject.SetActive(false);
	}
}
