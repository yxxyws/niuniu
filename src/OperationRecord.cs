using System;
public class OperationRecord
{
	private string _operation = "lobby";
	private long _operationTime;
	public string Operator
	{
		get
		{
			return this._operation;
		}
	}
	public long OperationTime
	{
		get
		{
			return this._operationTime;
		}
	}
	public OperationRecord(string operation)
	{
		this._operation = operation;
		this._operationTime = SingletonMono<DataManager, AllScene>.Instance.serverTime + (DataManager.ConvertDateTimeToInt(DateTime.Now) - SingletonMono<DataManager, AllScene>.Instance.clientTime);
	}
}
