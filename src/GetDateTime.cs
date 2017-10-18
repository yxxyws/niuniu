using System;
using System.Collections.Generic;
using UnityEngine;
public class GetDateTime : MonoBehaviour
{
	public static GetDateTime _instance;
	public UIButton btn_mask;
	public UILabel label_time;
	public UIInput input_year;
	public UIInput input_month;
	public List<UIButton> dayList;
	public GameObject table_day;
	public UIButton btn_comfirm;
	public UIButton btn_cancle;
	private Action<DateTime> callback;
	private string time;
	private int year;
	private int month;
	private int day;
	private void Awake()
	{
		GetDateTime._instance = this;
		EventDelegate.Set(this.btn_comfirm.onClick, new EventDelegate.Callback(this.OnComfirmBtnClick));
		EventDelegate.Set(this.btn_cancle.onClick, new EventDelegate.Callback(this.OnCancleBtnClick));
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.input_year.onSubmit, new EventDelegate.Callback(this.OnInputYearCallback));
		EventDelegate.Set(this.input_month.onSubmit, new EventDelegate.Callback(this.OnInputMonthCallback));
		for (int i = 0; i < this.dayList.Count; i++)
		{
			this.SetButtonCallBack(this.dayList[i], new Action<int>(this.OnDayBtnClick), i);
		}
	}
	public void InitShow(Action<DateTime> _callback)
	{
		this.callback = _callback;
		this.year = int.Parse(DateTime.Now.Year.ToString());
		this.month = int.Parse(DateTime.Now.Month.ToString());
		this.day = DateTime.Now.Day;
		this.input_year.value = this.year.ToString();
		this.input_month.value = this.month.ToString();
		this.ShowCurSelectTime();
	}
	private int GetDaysInMonth(int year, int month)
	{
		return DateTime.DaysInMonth(year, month);
	}
	private void CreateCalender(int days)
	{
		for (int i = 0; i < this.dayList.Count; i++)
		{
			this.dayList[i].gameObject.SetActive(false);
		}
		for (int j = 0; j < days; j++)
		{
			this.dayList[j].gameObject.SetActive(true);
			this.dayList[j].GetComponentInChildren<UILabel>().text = (j + 1).ToString();
			this.day = j + 1;
		}
		this.table_day.GetComponent<UITable>().Reposition();
	}
	private void OnInputYearCallback()
	{
		try
		{
			if (this.input_year.value.Equals(string.Empty))
			{
				return;
			}
			if (int.Parse(this.input_year.value) <= 0)
			{
				this.input_year.value = DateTime.Now.Year.ToString();
			}
		}
		catch
		{
			this.input_year.value = DateTime.Now.Year.ToString();
		}
		this.year = int.Parse(this.input_year.value);
		this.CreateCalender(this.GetDaysInMonth(this.year, this.month));
		this.ShowCurSelectTime();
	}
	private void OnInputMonthCallback()
	{
		try
		{
			if (this.input_month.value.Equals(string.Empty))
			{
				return;
			}
			if (int.Parse(this.input_month.value) <= 0 || int.Parse(this.input_month.value) > 12)
			{
				this.input_month.value = DateTime.Now.Month.ToString();
			}
		}
		catch
		{
			this.input_month.value = DateTime.Now.Month.ToString();
		}
		this.month = int.Parse(this.input_month.value);
		this.CreateCalender(this.GetDaysInMonth(this.year, this.month));
		this.ShowCurSelectTime();
	}
	private void ShowCurSelectTime()
	{
		this.time = string.Format("{0}-{1}-{2}", this.year, this.month, this.day);
		this.label_time.text = this.time;
	}
	private void OnComfirmBtnClick()
	{
		this.callback(Convert.ToDateTime(this.time));
		base.gameObject.SetActive(false);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnMaskBtnClick()
	{
		base.gameObject.SetActive(false);
	}
	private void OnCancleBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.OnMaskBtnClick();
	}
	private void SetButtonCallBack(UIButton btn, Action<int> callback, int index)
	{
		EventDelegate.Set(btn.onClick, delegate
		{
			callback(index + 1);
		});
	}
	private void OnDayBtnClick(int _day)
	{
		this.day = _day;
		this.ShowCurSelectTime();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
