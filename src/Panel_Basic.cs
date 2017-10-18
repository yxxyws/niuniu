using System;
using UnityEngine;
public abstract class Panel_Basic : MonoBehaviour
{
	public const PUID RootUID = PUID.Login;
	protected UIManager mUIMgr;
	protected PUID mBackUID;
	protected PanelType mType;
	protected PUID mUID;
	public UITweener tween;
	private bool isShow;
	public PanelType type
	{
		get
		{
			return this.mType;
		}
	}
	public PUID uID
	{
		get
		{
			return this.mUID;
		}
	}
	public void Init(UIManager iUIMgr)
	{
		this.mUIMgr = iUIMgr;
		this.mUID = PUID.NotSet;
		this.setUID();
		this.setBackUID();
		this.setPanelType();
		this.init();
		if (this.tween != null)
		{
			EventDelegate.Set(this.tween.onFinished, delegate
			{
				if (!this.isShow)
				{
					base.gameObject.SetActive(false);
				}
			});
		}
	}
	protected abstract void setUID();
	protected virtual void setBackUID()
	{
		this.mBackUID = PUID.Login;
	}
	protected virtual void setPanelType()
	{
		this.mType = PanelType.Normal;
	}
	protected virtual void init()
	{
	}
	public virtual void Show()
	{
		this.isShow = true;
		base.gameObject.SetActive(true);
		if (this.tween != null)
		{
			this.tween.PlayForward();
		}
	}
	public virtual void Hide()
	{
		this.isShow = false;
		if (this.tween != null)
		{
			this.tween.PlayReverse();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
	public virtual void BackView()
	{
		this.backView();
	}
	protected void backView()
	{
		if (this.type != PanelType.Normal)
		{
			this.mUIMgr.HidePanel(this.uID);
		}
		else
		{
			if (this.uID == PUID.Login)
			{
				this.mUIMgr.ApplyQuit();
			}
			else
			{
				this.mUIMgr.ShowPanel(this.mBackUID);
			}
		}
	}
	public virtual bool IsShowing()
	{
		return base.gameObject.activeSelf;
	}
}
