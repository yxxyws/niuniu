using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class Prefab_NoticeItem : MonoBehaviour
{
	public UIButton btn_noticeItem;
	public UILabel label_title;
	public UILabel label_time;
	public UILabel label_text;
	private Notice notice;
	private void Awake()
	{
		EventDelegate.Set(this.btn_noticeItem.onClick, new EventDelegate.Callback(this.OnNoticeItemBtnClick));
	}
	public void InitShow(Notice info)
	{
		this.notice = info;
		this.label_title.text = info.title;
		this.label_time.text = info.dateTime;
		this.label_text.text = info.content;
	}
	public void OnNoticeItemBtnClick()
	{
		UIManager.Instance.ShowNoticeContent(this.notice.title, this.notice.content);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
