using System;
using UnityEngine;
public class ExpressionShow : MonoBehaviour
{
	public UISpriteAnimation anim;
	[HideInInspector]
	public float time = 3f;
	public Action callback;
	public void InitShow(int index)
	{
		if (index < 10)
		{
			this.anim.namePrefix = "pic_0" + index.ToString() + "_0";
		}
		else
		{
			this.anim.namePrefix = "pic_" + index.ToString() + "_0";
		}
	}
	private void Update()
	{
		if (this.time > 0f)
		{
			this.time -= Time.deltaTime;
			if (this.time < 0f && this.callback != null)
			{
				this.callback();
			}
		}
	}
}
