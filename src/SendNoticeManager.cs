using System;
using UnityEngine;
public class SendNoticeManager : MonoBehaviour
{
	public UIButton btn_releaseNotice_01;
	public UIButton btn_releaseNotice_02;
	public GameObject obj_sendNotice;
	public UIButton btn_mask;
	public UIButton btn_close;
	public UIButton btn_send;
	public UIInput input_noticeText;
	private float timer;
	private float defaultSendTime = 180f;
	private void Awake()
	{
		EventDelegate.Set(this.btn_releaseNotice_01.onClick, new EventDelegate.Callback(this.OnReleaseNoticeBtnClick));
		EventDelegate.Set(this.btn_releaseNotice_02.onClick, new EventDelegate.Callback(this.OnReleaseNoticeBtnClick));
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_send.onClick, new EventDelegate.Callback(this.OnSendBtnClick));
	}
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.timer = 0f;
			}
		}
	}
	public void OnReleaseNoticeBtnClick()
	{
		this.obj_sendNotice.SetActive(true);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	public void OnMaskBtnClick()
	{
		this.obj_sendNotice.SetActive(false);
	}
	public void OnCloseBtnClick()
	{
		this.OnMaskBtnClick();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	public void OnSendBtnClick()
	{
		if (int.Parse(SingletonMono<DataManager, AllScene>.Instance.vip) >= 5)
		{
			if (this.input_noticeText.value == string.Empty)
			{
				TipManager.Instance.ShowFeedbackTipsBar("请输入您想发送的内容");
			}
			else
			{
				if (this.timer <= 0f)
				{
					SingletonMono<NetManager, AllScene>.Instance.SendNoticeText(this.input_noticeText.value);
					TipManager.Instance.ShowTips("发送成功", 2f);
					this.input_noticeText.value = string.Empty;
					this.timer = this.defaultSendTime;
				}
				else
				{
					TipManager.Instance.ShowFeedbackTipsBar("您发送公告操作太频繁,请稍候再发送！");
				}
			}
		}
		else
		{
			TipManager.Instance.ShowFeedbackTipsBar("会员等级大于5级才能发言哦！");
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
