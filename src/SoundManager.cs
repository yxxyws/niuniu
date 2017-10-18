using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;
	public AudioClip[] allClip;
	public Dictionary<string, int> dict_allClipName = new Dictionary<string, int>();
	public AudioSource bgPlay;
	public AudioSource effectPlay;
	public AudioSource uiPlay;
	private static object m_lockBoo = new object();
	private void Awake()
	{
		object lockBoo = SoundManager.m_lockBoo;
		Monitor.Enter(lockBoo);
		try
		{
			if (SoundManager.Instance == null)
			{
				SoundManager.Instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
				for (int i = 0; i < this.allClip.Length; i++)
				{
					this.dict_allClipName.Add(this.allClip[i].name, i);
				}
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
	public void ChangeSoundValue(float bg, float effect)
	{
		this.bgPlay.GetComponent<AudioSource>().volume = bg;
		this.effectPlay.GetComponent<AudioSource>().volume = effect;
		this.uiPlay.GetComponent<AudioSource>().volume = effect;
	}
	public void PlaySound(SoundType type, string name)
	{
		int num;
		this.dict_allClipName.TryGetValue(name, out num);
		switch (type)
		{
		case SoundType.BG:
			this.bgPlay.clip = this.allClip[num];
			this.bgPlay.Play();
			break;
		case SoundType.EFFECT:
			this.effectPlay.PlayOneShot(this.allClip[num]);
			break;
		case SoundType.UI:
			this.uiPlay.PlayOneShot(this.allClip[num]);
			break;
		}
	}
}
