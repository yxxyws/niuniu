using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
public class AsyncImageDownload : MonoBehaviour
{
	private static AsyncImageDownload _instance;
	public static AsyncImageDownload Instance
	{
		get
		{
			if (AsyncImageDownload._instance == null)
			{
				GameObject gameObject = new GameObject("AsyncImageDownload");
				AsyncImageDownload._instance = gameObject.AddComponent<AsyncImageDownload>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				AsyncImageDownload._instance.Init();
			}
			return AsyncImageDownload._instance;
		}
	}
	public string path
	{
		get
		{
			return Application.persistentDataPath + "/ImageCache/";
		}
	}
	public static AsyncImageDownload GetInstance()
	{
		return AsyncImageDownload.Instance;
	}
	public bool Init()
	{
		if (!Directory.Exists(this.path))
		{
			Directory.CreateDirectory(this.path);
		}
		return true;
	}
	public void SetAsyncImage(string url, Action<Texture2D> callback)
	{
		if (!File.Exists(this.path + url.GetHashCode()))
		{
			base.StartCoroutine(this.DownloadImage(url, callback));
		}
		else
		{
			base.StartCoroutine(this.LoadLocalImage(url, callback));
		}
	}
	[DebuggerHidden]
	private IEnumerator DownloadImage(string url, Action<Texture2D> callback)
	{
		AsyncImageDownload.<DownloadImage>c__Iterator0 <DownloadImage>c__Iterator = new AsyncImageDownload.<DownloadImage>c__Iterator0();
		<DownloadImage>c__Iterator.url = url;
		<DownloadImage>c__Iterator.callback = callback;
		<DownloadImage>c__Iterator.$this = this;
		return <DownloadImage>c__Iterator;
	}
	[DebuggerHidden]
	private IEnumerator LoadLocalImage(string url, Action<Texture2D> callback)
	{
		AsyncImageDownload.<LoadLocalImage>c__Iterator1 <LoadLocalImage>c__Iterator = new AsyncImageDownload.<LoadLocalImage>c__Iterator1();
		<LoadLocalImage>c__Iterator.url = url;
		<LoadLocalImage>c__Iterator.callback = callback;
		<LoadLocalImage>c__Iterator.$this = this;
		return <LoadLocalImage>c__Iterator;
	}
}
