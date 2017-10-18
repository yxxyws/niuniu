using System;
using UnityEngine;
public class NoticeManager : MonoBehaviour
{
	public GameObject obj_noticeBar;
	public GameObject obj_noticeContent;
	public UIButton btn_mask;
	public UIButton btn_close;
	public PoolManager pool_notice;
	public UIGrid grid;
	public UIButton btn_content_close;
	public UILabel label_title;
	public UILabel label_content;
	private void Awake()
	{
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_content_close.onClick, new EventDelegate.Callback(this.OnContentCloseBtnClick));
	}
	private void Start()
	{
		UIManager expr_05 = UIManager.Instance;
		expr_05.ShowNoticeContent = (Action<string, string>)Delegate.Combine(expr_05.ShowNoticeContent, new Action<string, string>(this.ShowNoticeContent));
	}
	public void InitShow()
	{
		this.obj_noticeBar.SetActive(true);
		this.obj_noticeContent.SetActive(false);
		this.pool_notice.ResetAllIdleItem();
		for (int i = 0; i < SingletonMono<DataManager, AllScene>.Instance.allNotice.Count; i++)
		{
			Prefab_NoticeItem component = this.pool_notice.GetNGUIItem().GetComponent<Prefab_NoticeItem>();
			component.InitShow(SingletonMono<DataManager, AllScene>.Instance.allNotice[i]);
		}
		this.grid.repositionNow = true;
	}
	public void ShowNoticeContent(string title, string content)
	{
		this.label_title.text = title;
		this.label_content.text = content;
		this.obj_noticeBar.SetActive(false);
		this.obj_noticeContent.SetActive(true);
	}
	private void OnMaskBtnClick()
	{
		base.gameObject.SetActive(false);
	}
	private void OnCloseBtnClick()
	{
		this.OnMaskBtnClick();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnContentCloseBtnClick()
	{
		this.obj_noticeBar.SetActive(true);
		this.obj_noticeContent.SetActive(false);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
