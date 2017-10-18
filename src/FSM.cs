using System;
using System.Collections.Generic;
using UnityEngine;
public class FSM<T> where T : IComparable
{
	private Dictionary<T, FsmStateStruct<T>> mFsmDict;
	private FsmStateStruct<T> CurrentState;
	private FsmStateStruct<T> LastState;
	private FsmStateStruct<T> NextState;
	public FSM()
	{
		this.CurrentState = null;
		this.LastState = null;
		this.mFsmDict = new Dictionary<T, FsmStateStruct<T>>();
	}
	public void InsertState(T iId, FsmFunc iEnter, FsmFunc iUpdate, FsmFunc iExit)
	{
		this.InsertState(new FsmStateStruct<T>
		{
			mId = iId,
			mEnter = iEnter,
			mUpdate = iUpdate,
			mExit = iExit
		});
	}
	public void InsertState(FsmStateStruct<T> iStateFunc)
	{
		if (this.mFsmDict.ContainsKey(iStateFunc.mId))
		{
			Debug.LogError("Fsm Contain two" + iStateFunc.mId + " state!");
			return;
		}
		this.mFsmDict.Add(iStateFunc.mId, iStateFunc);
	}
	public void RemoveState(T iId)
	{
		if (this.mFsmDict.ContainsKey(iId))
		{
			this.mFsmDict.Remove(iId);
		}
		else
		{
			Debug.LogWarning("No" + iId + "state in FsmDictionary");
		}
	}
	public void SetCurrentState(T iId)
	{
		if (!this.mFsmDict.ContainsKey(iId))
		{
			Debug.LogError("FsmDictionary do not contain " + iId + " state!");
			return;
		}
		this.NextState = this.mFsmDict[iId];
		if (this.CurrentState != null)
		{
			this.CurrentState.mExit();
			this.LastState = this.CurrentState;
		}
		this.CurrentState = this.mFsmDict[iId];
		if (this.LastState == null)
		{
			this.LastState = this.CurrentState;
		}
		this.CurrentState.mEnter();
	}
	public T GetCurrentState()
	{
		return this.CurrentState.mId;
	}
	public T GetLastState()
	{
		return this.LastState.mId;
	}
	public T GetNextState()
	{
		return this.NextState.mId;
	}
	public void Update()
	{
		if (this.CurrentState != null)
		{
			this.CurrentState.mUpdate();
		}
	}
}
