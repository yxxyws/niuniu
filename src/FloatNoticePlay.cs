using System;
using UnityEngine;
public class FloatNoticePlay : MonoBehaviour
{
	public UILabel label_show;
	public GameObject obj_pic_textBg;
	public UIPanel panel_Clip;
	public TweenPosition tween_pos;
	private float moveSpeed = 120f;
	private bool isPlaying;
	private float targetPosX;
	private void Awake()
	{
		this.tween_pos.AddOnFinished(new EventDelegate.Callback(this.OnTweenPosCallback));
		this.targetPosX = this.panel_Clip.GetViewSize().x / 2f;
	}
	private void Start()
	{
		FloatNoticeManager expr_05 = FloatNoticeManager.Instance;
		expr_05.PlayNotice = (Action<string>)Delegate.Combine(expr_05.PlayNotice, new Action<string>(this.PlayNotice));
		if (FloatNoticeManager.Instance.curPlayQueue.Count > 0)
		{
			this.PlayNotice(FloatNoticeManager.Instance.curPlayQueue.Dequeue());
		}
	}
	private void OnDestroy()
	{
		FloatNoticeManager expr_05 = FloatNoticeManager.Instance;
		expr_05.PlayNotice = (Action<string>)Delegate.Remove(expr_05.PlayNotice, new Action<string>(this.PlayNotice));
	}
	public void PlayNotice(string str)
	{
		if (this.isPlaying)
		{
			FloatNoticeManager.Instance.curPlayQueue.Enqueue(str);
		}
		else
		{
			this.isPlaying = true;
			this.label_show.text = str;
			this.Play();
		}
	}
	public void Play()
	{
		if (this.obj_pic_textBg != null && !this.obj_pic_textBg.activeSelf)
		{
			this.obj_pic_textBg.SetActive(true);
		}
		this.tween_pos.from = new Vector3(this.targetPosX + this.label_show.localSize.x, 1f, 0f);
		this.tween_pos.to = new Vector3(-this.targetPosX, 1f, 0f);
		float duration;
		if (this.label_show.localSize.x < this.panel_Clip.GetViewSize().x)
		{
			duration = this.panel_Clip.GetViewSize().x / this.moveSpeed;
		}
		else
		{
			duration = this.label_show.localSize.x / this.moveSpeed;
		}
		this.tween_pos.duration = duration;
		this.tween_pos.ResetToBeginning();
		this.tween_pos.PlayForward();
	}
	public void OnTweenPosCallback()
	{
		this.tween_pos.ResetToBeginning();
		if (FloatNoticeManager.Instance.curPlayQueue.Count > 0)
		{
			this.label_show.text = FloatNoticeManager.Instance.curPlayQueue.Dequeue();
			this.Play();
		}
		else
		{
			this.isPlaying = false;
			if (this.obj_pic_textBg != null)
			{
				this.obj_pic_textBg.SetActive(false);
			}
		}
	}
}
