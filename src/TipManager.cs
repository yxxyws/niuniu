using NiuNiu;
using System;
using System.Threading;
using UnityEngine;
public class TipManager : MonoBehaviour
{
	public static TipManager Instance = null;
	public UILabel label_tip;
	public UILabel label_tipBar;
	public UIButton btn_confirm;
	public UILabel label_wait;
	public UILabel label_confirmBar;
	public UIButton btn_confirmBar_confirm;
	public UIButton btn_confirmBar_cancle;
	public UILabel lable_feedbackTip;
	public UIButton btn_feedbackConfirm;
	public GameObject obj_quitRoom;
	public UIButton btn_confirmQuit;
	public UIButton btn_cancleQuit;
	private float tipTimer;
	private Action btnCallBack;
	private Action confirmBarConfirmBtnCallback;
	private Action confirmBarCancleBtncallback;
	private static object m_lockBoo = new object();
	private void Awake()
	{
		object lockBoo = TipManager.m_lockBoo;
		Monitor.Enter(lockBoo);
		try
		{
			if (TipManager.Instance == null)
			{
				TipManager.Instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		finally
		{
			Monitor.Exit(lockBoo);
		}
		EventDelegate.Set(this.btn_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBtnClick));
		EventDelegate.Set(this.btn_feedbackConfirm.onClick, new EventDelegate.Callback(this.OnFeedbackConfirmBtnClick));
		EventDelegate.Set(this.btn_confirmQuit.onClick, new EventDelegate.Callback(this.OnConfirmQuitRoomBtnClick));
		EventDelegate.Set(this.btn_cancleQuit.onClick, new EventDelegate.Callback(this.OnCancleQuitRoomBtnClick));
		EventDelegate.Set(this.btn_confirmBar_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBarConfirmClick));
		EventDelegate.Set(this.btn_confirmBar_cancle.onClick, new EventDelegate.Callback(this.OnConfirmBarCancleClick));
	}
	private void Update()
	{
		if (this.tipTimer > 0f)
		{
			this.tipTimer -= Time.deltaTime;
			if (this.tipTimer <= 0f)
			{
				this.label_tip.gameObject.SetActive(false);
			}
		}
	}
	public void ShowTips(string info, float time)
	{
		this.label_tip.text = info;
		this.tipTimer = time;
		if (time <= 0f)
		{
			this.label_tip.gameObject.SetActive(false);
			return;
		}
		this.label_tip.gameObject.SetActive(true);
	}
	public void ShowConfirmBar(string info, Action confirmCallback, Action cancleCallback = null)
	{
		this.label_confirmBar.text = info;
		this.confirmBarConfirmBtnCallback = confirmCallback;
		this.confirmBarCancleBtncallback = cancleCallback;
		this.label_confirmBar.gameObject.SetActive(true);
	}
	public void HideConfirmBar()
	{
		this.label_confirmBar.gameObject.SetActive(false);
	}
	public void ShowFeedbackTipsBar(string info)
	{
		if (this.label_wait.gameObject.activeSelf)
		{
			this.label_wait.gameObject.SetActive(false);
		}
		this.lable_feedbackTip.text = info;
		this.lable_feedbackTip.gameObject.SetActive(true);
	}
	public void ShowTipsBar(string info, Action callback = null)
	{
		this.label_tipBar.text = info;
		this.btnCallBack = callback;
		this.label_tipBar.gameObject.SetActive(true);
	}
	public void HideTipBar()
	{
		this.label_tipBar.gameObject.SetActive(false);
	}
	public void ShowWaitTip(string str = "")
	{
		this.label_wait.text = str;
		this.label_wait.gameObject.SetActive(true);
	}
	public void HideWaitTip()
	{
		this.label_wait.gameObject.SetActive(false);
	}
	public void IsShowQuitRoomTipBar(bool isShow)
	{
		this.obj_quitRoom.gameObject.SetActive(isShow);
	}
	private void OnConfirmBtnClick()
	{
		if (this.btnCallBack != null)
		{
			this.btnCallBack();
		}
		else
		{
			Application.Quit();
		}
	}
	private void OnFeedbackConfirmBtnClick()
	{
		this.lable_feedbackTip.gameObject.SetActive(false);
	}
	private void OnConfirmQuitRoomBtnClick()
	{
		this.IsShowQuitRoomTipBar(false);
		if (GameDataManager.Instance.isBanker)
		{
			SingletonMono<DataManager, AllScene>.Instance.coin += GameDataManager.Instance.bankerCurCoin;
		}
		this.ShowWaitTip("正在退出房间...");
		NiuNiuNetManager.Instance.SendLeaveRoom();
	}
	private void OnCancleQuitRoomBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.IsShowQuitRoomTipBar(false);
	}
	public void OnConfirmBarConfirmClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.confirmBarConfirmBtnCallback();
		this.label_confirmBar.gameObject.SetActive(false);
	}
	public void OnConfirmBarCancleClick()
	{
		if (this.confirmBarCancleBtncallback != null)
		{
			this.confirmBarCancleBtncallback();
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.label_confirmBar.gameObject.SetActive(false);
	}
}
