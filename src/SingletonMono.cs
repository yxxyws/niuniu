using System;
using System.Threading;
using UnityEngine;
public abstract class SingletonMono<T, V> : MonoBehaviour where T : SingletonMono<T, V> where V : SingletonType
{
	private static T m_instance;
	private static object m_lockBoo = new object();
	private static bool applicationQuit = false;
	private bool inited;
	public static T Instance
	{
		get
		{
			if (SingletonMono<T, V>.m_instance == null)
			{
				if (SingletonMono<T, V>.applicationQuit)
				{
					Debug.Log("application is quitting.Won't create singleton " + typeof(T).ToString());
					return (T)((object)null);
				}
				object lockBoo = SingletonMono<T, V>.m_lockBoo;
				Monitor.Enter(lockBoo);
				try
				{
					if (SingletonMono<T, V>.m_instance == null)
					{
						GameObject gameObject = UnityEngine.Object.FindObjectOfType(typeof(T)) as GameObject;
						if (gameObject == null)
						{
							gameObject = new GameObject();
							gameObject.name = "SG_" + typeof(T).ToString();
							gameObject.AddComponent<T>();
						}
						SingletonMono<T, V>.setDontDestroy(gameObject);
					}
				}
				finally
				{
					Monitor.Exit(lockBoo);
				}
			}
			return SingletonMono<T, V>.m_instance;
		}
	}
	private void Awake()
	{
		if (SingletonMono<T, V>.m_instance == null)
		{
			object lockBoo = SingletonMono<T, V>.m_lockBoo;
			Monitor.Enter(lockBoo);
			try
			{
				if (SingletonMono<T, V>.m_instance == null)
				{
					SingletonMono<T, V>.setDontDestroy(base.gameObject);
				}
				else
				{
					Debug.LogError("destroy one singleton component in " + base.gameObject.name + ",please remove the component " + typeof(T).ToString());
					UnityEngine.Object.Destroy(base.gameObject.GetComponent<T>());
				}
			}
			finally
			{
				Monitor.Exit(lockBoo);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
	public static bool HadInstance()
	{
		return SingletonMono<T, V>.m_instance != null;
	}
	private static void setDontDestroy(GameObject obj)
	{
		if (SingletonMono<T, V>.m_instance != null)
		{
			return;
		}
		SingletonMono<T, V>.m_instance = obj.GetComponent<T>();
		if (typeof(V) == typeof(AllScene))
		{
			UnityEngine.Object.DontDestroyOnLoad(obj);
		}
		else
		{
			Debug.Log("create OneScene singleton:" + typeof(T).ToString());
		}
		SingletonMono<T, V>.m_instance.inited = true;
		SingletonMono<T, V>.m_instance.Init();
	}
	protected abstract void Init();
	protected abstract void Quit();
	protected static bool isQuitting()
	{
		return SingletonMono<T, V>.applicationQuit;
	}
	public void OnDestroy()
	{
		SingletonMono<T, V>.applicationQuit = true;
		if (this.inited)
		{
			this.Quit();
		}
	}
}
