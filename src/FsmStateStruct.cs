using System;
public class FsmStateStruct<T> where T : IComparable
{
	public FsmFunc mEnter;
	public FsmFunc mUpdate;
	public FsmFunc mExit;
	public T mId;
}
