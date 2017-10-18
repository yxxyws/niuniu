using com.max.JiXiangLobby;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class IdGiveManager : MonoBehaviour
{
	public UIButton btn_mask;
	public UIButton btn_close;
	public UIToggle toggle_give;
	public UIToggle toggle_giveRecord;
	public UIToggle toggle_recharge;
	public UIToggle toggle_receiveRecord;
	public GameObject obj_IdGivePanel;
	public GameObject obj_give;
	public GameObject obj_giveRecord;
	public GameObject obj_receiveRecord;
	public GameObject obj_rechargeRecord;
	public GameObject prefab_recoad;
	public UIButton btn_confirm;
	public UILabel label_curCoin;
	public UIInput input_id;
	public UIInput input_giveCoin;
	private double curCoin;
	public PoolManager pool_giveRecord;
	public UIGrid grid_giveRecord;
	private Dictionary<int, GameObject> allGiveObj = new Dictionary<int, GameObject>();
	public PoolManager pool_receiveRecord;
	public UIGrid grid_receiveRecord;
	private Dictionary<int, GameObject> allReceiveObj = new Dictionary<int, GameObject>();
	private DateTime startTime;
	private DateTime endTime;
	public UIButton btn_startTime;
	public UILabel label_startTime;
	public UIButton btn_endTime;
	public UILabel label_endTime;
	public UIButton btn_confirmQuery;
	public PoolManager pool_rechargeRecord;
	public UIGrid grid_rechargeRecord;
	public GameObject obj_dateTimeSelelct;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBtnClick));
		EventDelegate.Set(this.input_giveCoin.onChange, new EventDelegate.Callback(this.OnInputGiveCoinChange));
		EventDelegate.Set(this.btn_confirmQuery.onClick, new EventDelegate.Callback(this.OnConfirmQueryBtnClick));
		EventDelegate.Set(this.btn_startTime.onClick, new EventDelegate.Callback(this.OnStartTimeBtnClick));
		EventDelegate.Set(this.btn_endTime.onClick, new EventDelegate.Callback(this.OnEndTimeBtnClick));
		EventDelegate.Set(this.toggle_give.onChange, new EventDelegate.Callback(this.OnGiveToggleChange));
		EventDelegate.Set(this.toggle_giveRecord.onChange, new EventDelegate.Callback(this.OnGiveRecordToggleChange));
		EventDelegate.Set(this.toggle_receiveRecord.onChange, new EventDelegate.Callback(this.OnReceiveRecordToggleChange));
		EventDelegate.Set(this.toggle_recharge.onChange, new EventDelegate.Callback(this.OnRechargeRecordToggleChange));
	}
	private void Start()
	{
		UIManager expr_05 = UIManager.Instance;
		expr_05.InitAllRecord = (Action)Delegate.Combine(expr_05.InitAllRecord, new Action(this.InitAllRecord));
		UIManager expr_2B = UIManager.Instance;
		expr_2B.AddGiveRecord = (Action<Record>)Delegate.Combine(expr_2B.AddGiveRecord, new Action<Record>(this.AddGiveRecord));
		UIManager expr_51 = UIManager.Instance;
		expr_51.AddReceiveRecord = (Action<Record>)Delegate.Combine(expr_51.AddReceiveRecord, new Action<Record>(this.AddReceiveRecord));
		UIManager expr_77 = UIManager.Instance;
		expr_77.UpdateUserInfoShow = (Action)Delegate.Combine(expr_77.UpdateUserInfoShow, new Action(this.UpdateCurCoinShow));
		UIManager expr_9D = UIManager.Instance;
		expr_9D.LoadingRechargeRecord = (Action<List<Record>>)Delegate.Combine(expr_9D.LoadingRechargeRecord, new Action<List<Record>>(this.LoadingRechargeRecord));
	}
	public void InitShow()
	{
		this.UpdateCurCoinShow();
		this.input_id.value = string.Empty;
		this.input_giveCoin.value = string.Empty;
		this.toggle_give.Set(true, true);
		this.toggle_giveRecord.Set(false, true);
		this.toggle_receiveRecord.Set(false, true);
		if (SingletonMono<DataManager, AllScene>.Instance.isGotGiveRecord)
		{
			this.InitAllRecord();
		}
	}
	public void InitAllRecord()
	{
		this.pool_giveRecord.ResetAllIdleItem();
		this.pool_receiveRecord.ResetAllIdleItem();
		for (int i = 0; i < SingletonMono<DataManager, AllScene>.Instance.allGiveRecord.Count; i++)
		{
			this.AddGiveRecord(SingletonMono<DataManager, AllScene>.Instance.allGiveRecord[i]);
		}
		for (int j = 0; j < SingletonMono<DataManager, AllScene>.Instance.allReceiveRecord.Count; j++)
		{
			this.AddReceiveRecord(SingletonMono<DataManager, AllScene>.Instance.allReceiveRecord[j]);
		}
		TipManager.Instance.HideWaitTip();
	}
	public void UpdateCurCoinShow()
	{
		this.curCoin = SingletonMono<DataManager, AllScene>.Instance.coin;
		this.label_curCoin.text = this.curCoin.ToString();
	}
	public void AddGiveRecord(Record record)
	{
		GiveRecord component = this.pool_giveRecord.GetNGUIItem().GetComponent<GiveRecord>();
		component.InitShow(record);
		component.transform.parent = this.grid_giveRecord.transform;
		this.grid_giveRecord.repositionNow = true;
	}
	public void AddReceiveRecord(Record record)
	{
		GiveRecord component = this.pool_receiveRecord.GetNGUIItem().GetComponent<GiveRecord>();
		component.InitShow(record);
		component.transform.parent = this.grid_receiveRecord.transform;
		this.grid_receiveRecord.repositionNow = true;
	}
	private void InitShowRechargeRecord()
	{
		this.UpdateStartTime(DateTime.Now.AddDays(-1.0));
		this.UpdateEndTime(DateTime.Now);
		this.pool_rechargeRecord.ResetAllIdleItem();
	}
	public void UpdateStartTime(DateTime time)
	{
		this.startTime = time;
		this.label_startTime.text = string.Format("{0:yyyy-MM-dd}", time);
	}
	public void UpdateEndTime(DateTime time)
	{
		this.endTime = time;
		this.label_endTime.text = string.Format("{0:yyyy-MM-dd}", time);
	}
	private void LoadingRechargeRecord(List<Record> allRecord)
	{
		base.StartCoroutine(this.AsynLodingRecord(allRecord));
	}
	[DebuggerHidden]
	private IEnumerator AsynLodingRecord(List<Record> allRecord)
	{
		IdGiveManager.<AsynLodingRecord>c__Iterator0 <AsynLodingRecord>c__Iterator = new IdGiveManager.<AsynLodingRecord>c__Iterator0();
		<AsynLodingRecord>c__Iterator.allRecord = allRecord;
		<AsynLodingRecord>c__Iterator.$this = this;
		return <AsynLodingRecord>c__Iterator;
	}
	public void AddRechargeRecord(Record record)
	{
		GiveRecord component = this.pool_rechargeRecord.GetNGUIItem().GetComponent<GiveRecord>();
		component.InitShow(record);
		component.transform.parent = this.grid_rechargeRecord.transform;
		this.grid_rechargeRecord.repositionNow = true;
	}
	private void OnCloseBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_IdGivePanel.SetActive(false);
		if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
		{
			SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("lobby"));
		}
	}
	private void OnConfirmBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (SingletonMono<DataManager, AllScene>.Instance.isGlobalCanGive.Equals("0") || SingletonMono<DataManager, AllScene>.Instance.isCanGive.Equals("0"))
		{
			TipManager.Instance.ShowFeedbackTipsBar("当前已经关闭赠送的相关操作,请联系客服询问原因");
			return;
		}
		if (this.input_id.value == string.Empty)
		{
			TipManager.Instance.ShowTips("请输入赠送玩家的ID信息", 2f);
		}
		else
		{
			if (this.input_id.value.Equals(SingletonMono<DataManager, AllScene>.Instance.username))
			{
				TipManager.Instance.ShowTips("请输入其他玩家的ID信息,自己不能赠送给自己", 2f);
			}
			else
			{
				if (this.input_giveCoin.value == string.Empty)
				{
					TipManager.Instance.ShowFeedbackTipsBar("请输入赠送玩家的金币数");
				}
				else
				{
					double num = double.Parse(this.input_giveCoin.value);
					if (num >= 100000.0)
					{
						TipManager.Instance.ShowWaitTip(string.Empty);
						SingletonMono<NetManager, AllScene>.Instance.SendGiveCoinIdGetNick(this.input_id.value, num);
						this.input_giveCoin.value = string.Empty;
					}
					else
					{
						TipManager.Instance.ShowFeedbackTipsBar("金币数少于10万不能赠送...");
					}
				}
			}
		}
	}
	private void OnInputGiveCoinChange()
	{
		if (this.input_giveCoin.value != string.Empty)
		{
			if (double.Parse(this.input_giveCoin.value) > SingletonMono<DataManager, AllScene>.Instance.coin)
			{
				this.curCoin = 0.0;
				this.input_giveCoin.value = SingletonMono<DataManager, AllScene>.Instance.coin.ToString();
			}
			else
			{
				this.curCoin = SingletonMono<DataManager, AllScene>.Instance.coin - double.Parse(this.input_giveCoin.value);
			}
			this.label_curCoin.text = this.curCoin.ToString();
		}
	}
	private void OnGiveToggleChange()
	{
		if (this.toggle_give.value)
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			this.obj_give.SetActive(true);
			this.obj_giveRecord.SetActive(false);
			this.obj_receiveRecord.SetActive(false);
			this.obj_rechargeRecord.SetActive(false);
		}
	}
	private void OnGiveRecordToggleChange()
	{
		if (this.toggle_giveRecord.value)
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			this.obj_give.SetActive(false);
			this.obj_giveRecord.SetActive(true);
			this.obj_receiveRecord.SetActive(false);
			this.obj_rechargeRecord.SetActive(false);
		}
	}
	private void OnReceiveRecordToggleChange()
	{
		if (this.toggle_receiveRecord.value)
		{
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
			this.obj_give.SetActive(false);
			this.obj_giveRecord.SetActive(false);
			this.obj_receiveRecord.SetActive(true);
			this.obj_rechargeRecord.SetActive(false);
		}
	}
	private void OnRechargeRecordToggleChange()
	{
		if (this.toggle_recharge.value)
		{
			this.InitShowRechargeRecord();
			this.obj_give.SetActive(false);
			this.obj_giveRecord.SetActive(false);
			this.obj_receiveRecord.SetActive(false);
			this.obj_rechargeRecord.SetActive(true);
			SoundManager.Instance.PlaySound(SoundType.UI, "button");
		}
	}
	private void OnConfirmQueryBtnClick()
	{
		if (DateTime.Compare(this.startTime, this.endTime) < 0)
		{
			TipManager.Instance.ShowWaitTip(string.Empty);
			SingletonMono<NetManager, AllScene>.Instance.SendGetRechargeRecord(string.Format("{0:yyyy-MM-dd}", this.startTime), string.Format("{0:yyyy-MM-dd}", this.endTime));
		}
		else
		{
			TipManager.Instance.ShowTips("查询结束时间不可以比开始时间早,请检查时间", 2f);
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnStartTimeBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_dateTimeSelelct.GetComponent<GetDateTime>().InitShow(new Action<DateTime>(this.UpdateStartTime));
		this.obj_dateTimeSelelct.SetActive(true);
	}
	private void OnEndTimeBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_dateTimeSelelct.GetComponent<GetDateTime>().InitShow(new Action<DateTime>(this.UpdateEndTime));
		this.obj_dateTimeSelelct.SetActive(true);
	}
}
