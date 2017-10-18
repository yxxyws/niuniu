using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class FloatNoticeManager : MonoBehaviour
{
	public static FloatNoticeManager Instance = null;
	private static object m_lockBoo = new object();
	public Queue<string> curPlayQueue = new Queue<string>();
	private int curTakeTurnsIndex;
	private float timer = 180f;
	private const float defaule_timer = 180f;
	public Action<string> PlayNotice;
	private void Awake()
	{
		object lockBoo = FloatNoticeManager.m_lockBoo;
		Monitor.Enter(lockBoo);
		try
		{
			if (FloatNoticeManager.Instance == null)
			{
				FloatNoticeManager.Instance = this;
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
	}
	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = 180f;
			if (SingletonMono<DataManager, AllScene>.Instance.allNotice.Count > 0)
			{
				this.ShowNotice(SingletonMono<DataManager, AllScene>.Instance.allNotice[this.curTakeTurnsIndex].content);
				this.curTakeTurnsIndex = (this.curTakeTurnsIndex + 1) % SingletonMono<DataManager, AllScene>.Instance.allNotice.Count;
			}
		}
	}
	public void ShowNotice(string str)
	{
		if (this.PlayNotice != null)
		{
			this.PlayNotice(str);
		}
	}
}
