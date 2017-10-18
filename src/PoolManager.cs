using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
	public List<GameObject> allItems = new List<GameObject>();
	public List<GameObject> allIdleItem = new List<GameObject>();
	public GameObject item_prefab;
	public GameObject parent;
	public GameObject CreateNGUIItem()
	{
		GameObject gameObject = this.parent.AddChild(this.item_prefab);
		this.allItems.Add(gameObject);
		return gameObject;
	}
	public GameObject CreateObjItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.item_prefab, Vector3.zero, Quaternion.identity);
		gameObject.transform.parent = this.parent.transform;
		this.allItems.Add(gameObject);
		return gameObject;
	}
	public GameObject GetNGUIItem()
	{
		GameObject gameObject;
		if (this.allIdleItem.Count > 0)
		{
			gameObject = this.allIdleItem[0];
			this.allIdleItem.RemoveAt(0);
		}
		else
		{
			gameObject = this.CreateNGUIItem();
		}
		gameObject.SetActive(true);
		return gameObject;
	}
	public GameObject GetGameObjectItem()
	{
		GameObject gameObject;
		if (this.allIdleItem.Count > 0)
		{
			gameObject = this.allIdleItem[0];
			this.allIdleItem.RemoveAt(0);
		}
		else
		{
			gameObject = this.CreateObjItem();
		}
		gameObject.SetActive(true);
		return gameObject;
	}
	public void HideAllItem()
	{
		for (int i = 0; i < this.allItems.Count; i++)
		{
			this.allItems[i].SetActive(false);
		}
	}
	public void ResetAllIdleItem()
	{
		this.allIdleItem.Clear();
		for (int i = 0; i < this.allItems.Count; i++)
		{
			this.allItems[i].SetActive(false);
			this.allIdleItem.Add(this.allItems[i]);
		}
	}
	[DebuggerHidden]
	public IEnumerator AsynRestAllIdleItem()
	{
		PoolManager.<AsynRestAllIdleItem>c__Iterator0 <AsynRestAllIdleItem>c__Iterator = new PoolManager.<AsynRestAllIdleItem>c__Iterator0();
		<AsynRestAllIdleItem>c__Iterator.$this = this;
		return <AsynRestAllIdleItem>c__Iterator;
	}
	public void ResetIdleItem(GameObject obj)
	{
		obj.SetActive(false);
		this.allIdleItem.Add(obj);
	}
}
