using System;
using UnityEngine;
public class SafaManager : MonoBehaviour
{
	public UIButton btn_mask;
	public UIButton btn_close;
	public UIButton btn_sub;
	public UIButton btn_add;
	public UIButton btn_reset;
	public UIButton btn_confirm;
	public UIButton btn_idGive;
	public UISlider slider_coin;
	public UIInput input_curCoin;
	public UILabel label_curCoin;
	public UILabel label_deposit;
	public GameObject bar_idGive;
	private double curCoin;
	private double safeCoin;
	private double totalCoin;
	private bool isPress;
	private void Awake()
	{
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		EventDelegate.Set(this.btn_sub.onClick, new EventDelegate.Callback(this.OnSubBtnClick));
		EventDelegate.Set(this.btn_add.onClick, new EventDelegate.Callback(this.OnAddBtnClick));
		EventDelegate.Set(this.btn_reset.onClick, new EventDelegate.Callback(this.OnResetBtnClick));
		EventDelegate.Set(this.btn_confirm.onClick, new EventDelegate.Callback(this.OnConfirmBtnClick));
		EventDelegate.Set(this.btn_idGive.onClick, new EventDelegate.Callback(this.OnIdGiveBtnClick));
		UIEventListener expr_D6 = this.slider_coin.GetComponent<UIEventListener>();
		expr_D6.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_D6.onPress, new UIEventListener.BoolDelegate(this.SliderOnPress));
		EventDelegate.Set(this.slider_coin.onChange, new EventDelegate.Callback(this.OnSliderCoinChange));
		UIEventListener expr_11F = this.input_curCoin.GetComponent<UIEventListener>();
		expr_11F.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(expr_11F.onSelect, new UIEventListener.BoolDelegate(this.InputOnSelect));
		EventDelegate.Set(this.input_curCoin.onChange, new EventDelegate.Callback(this.OnInputCurCoinChange));
	}
	private void Start()
	{
		UIManager expr_05 = UIManager.Instance;
		expr_05.RefreshSafeShow = (Action)Delegate.Combine(expr_05.RefreshSafeShow, new Action(this.InitShow));
	}
	public void InitShow()
	{
		this.slider_coin.value = (float)(SingletonMono<DataManager, AllScene>.Instance.safeCoin / (SingletonMono<DataManager, AllScene>.Instance.coin + SingletonMono<DataManager, AllScene>.Instance.safeCoin));
		this.curCoin = SingletonMono<DataManager, AllScene>.Instance.coin;
		this.safeCoin = SingletonMono<DataManager, AllScene>.Instance.safeCoin;
		this.totalCoin = this.curCoin + this.safeCoin;
		this.UpdateCoinShow();
	}
	private void UpdateCoinShow()
	{
		this.label_curCoin.text = this.curCoin.ToString();
		this.label_deposit.text = this.safeCoin.ToString();
	}
	private void OnCloseBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		base.gameObject.SetActive(false);
		if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
		{
			SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("lobby"));
		}
	}
	private void OnSubBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.safeCoin > 10000.0)
		{
			this.safeCoin -= 10000.0;
			this.curCoin += 10000.0;
		}
		else
		{
			this.curCoin += this.safeCoin;
			this.safeCoin = 0.0;
		}
		this.UpdateCoinShow();
		this.slider_coin.value = (float)(this.safeCoin / this.totalCoin);
	}
	private void OnAddBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.curCoin > 10000.0)
		{
			this.curCoin -= 10000.0;
			this.safeCoin += 10000.0;
		}
		else
		{
			this.safeCoin += this.curCoin;
			this.curCoin = 0.0;
		}
		this.UpdateCoinShow();
		this.slider_coin.value = (float)(this.safeCoin / this.totalCoin);
	}
	private void OnResetBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.InitShow();
	}
	private void OnConfirmBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.safeCoin != SingletonMono<DataManager, AllScene>.Instance.safeCoin)
		{
			double num = this.curCoin - SingletonMono<DataManager, AllScene>.Instance.coin;
			double num2 = this.safeCoin - SingletonMono<DataManager, AllScene>.Instance.safeCoin;
			if (Math.Abs(num) != Math.Abs(num2) || (num2 > 0.0 && num > 0.0) || (num < 0.0 && num2 < 0.0))
			{
				TipManager.Instance.ShowTipsBar("数据异常", null);
			}
			else
			{
				if (num < 0.0 && Math.Abs(num) > SingletonMono<DataManager, AllScene>.Instance.coin)
				{
					TipManager.Instance.ShowTipsBar("数据异常", null);
				}
				else
				{
					if (num2 < 0.0 && Math.Abs(num2) > SingletonMono<DataManager, AllScene>.Instance.safeCoin)
					{
						Debug.Log(num2 + "   " + SingletonMono<DataManager, AllScene>.Instance.safeCoin);
						Debug.Log(1111);
						TipManager.Instance.ShowTipsBar("数据异常", null);
					}
					else
					{
						SingletonMono<NetManager, AllScene>.Instance.SendSaveSafeCoin(num, num2);
						TipManager.Instance.ShowWaitTip("正在保存数据....");
					}
				}
			}
		}
	}
	private void OnInputCurCoinChange()
	{
		this.curCoin = double.Parse(this.input_curCoin.value);
		if (this.curCoin > this.totalCoin)
		{
			this.curCoin = this.totalCoin;
			this.input_curCoin.value = this.curCoin.ToString();
			this.label_curCoin.text = this.curCoin.ToString();
		}
		this.safeCoin = this.totalCoin - this.curCoin;
		this.isPress = true;
		this.slider_coin.value = (float)(this.safeCoin / this.totalCoin);
		this.isPress = false;
	}
	private void OnSliderCoinChange()
	{
		if (this.isPress)
		{
			if (this.slider_coin.value == 0f)
			{
				this.safeCoin = 0.0;
			}
			else
			{
				if (this.slider_coin.value == 1f)
				{
					this.safeCoin = this.totalCoin;
				}
				else
				{
					this.safeCoin = (double)Mathf.FloorToInt((float)((double)this.slider_coin.value * this.totalCoin));
				}
			}
			this.curCoin = this.totalCoin - this.safeCoin;
			this.UpdateCoinShow();
		}
	}
	private void InputOnSelect(GameObject obj, bool isSelect)
	{
		this.input_curCoin.value = this.curCoin.ToString();
	}
	private void SliderOnPress(GameObject obj, bool isPressed)
	{
		this.isPress = isPressed;
	}
	private void OnIdGiveBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		base.gameObject.SetActive(false);
		this.bar_idGive.transform.parent.GetComponent<IdGiveManager>().InitShow();
		this.bar_idGive.SetActive(true);
		if (!SingletonMono<DataManager, AllScene>.Instance.isGotGiveRecord)
		{
			TipManager.Instance.ShowWaitTip(string.Empty);
			SingletonMono<NetManager, AllScene>.Instance.SendGetGiveRecord();
		}
		if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
		{
			SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("give"));
		}
	}
}
