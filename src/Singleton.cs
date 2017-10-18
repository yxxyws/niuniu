using System;
using System.Reflection;
public abstract class Singleton<T> where T : class
{
	protected static T instance;
	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
				ConstructorInfo constructorInfo = Array.Find<ConstructorInfo>(constructors, (ConstructorInfo c) => c.GetParameters().Length == 0);
				if (constructorInfo == null)
				{
					throw new Exception(typeof(T).ToString() + " Non-public Constructor not found!");
				}
				Singleton<T>.instance = (constructorInfo.Invoke(null) as T);
			}
			return Singleton<T>.instance;
		}
	}
	public static bool HadInstance()
	{
		return Singleton<T>.instance != null;
	}
}
