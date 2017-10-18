using System;
using UnityEngine;
public class PlayerBarManager : MonoBehaviour
{
	public TweenPosition tween_pos;
	public UIButton btn_back;
	public UIButton btn_add_coin;
	public UIButton btn_add_associator;
	public UITexture pic_head;
	public UILabel lable_id;
	public UILabel label_nick;
	public UILabel label_sex;
	public UILabel label_coin;
	public UILabel label_agentname;
	public UILabel label_phone;
	public UILabel label_associator;
	public ShopManager bar_shop;
	public GameObject obj_mask;
	private void Awake()
	{
		EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		EventDelegate.Set(this.btn_add_coin.onClick, new EventDelegate.Callback(this.OnAddCoinBtnClick));
		EventDelegate.Set(this.btn_add_associator.onClick, new EventDelegate.Callback(this.OnAddAssociatorBtnClik));
		EventDelegate.Set(this.tween_pos.onFinished, new EventDelegate.Callback(this.OnTweenPosCallbacck));
	}
	private void Start()
	{
		this.tween_pos.ResetToBeginning();
		UIManager expr_10 = UIManager.Instance;
		expr_10.UpdateUserInfoShow = (Action)Delegate.Combine(expr_10.UpdateUserInfoShow, new Action(this.InitShow));
	}
	public void InitShow()
	{
		this.lable_id.text = "ID : " + SingletonMono<DataManager, AllScene>.Instance.username;
		this.pic_head.mainTexture = SingletonMono<DataManager, AllScene>.Instance.headTexture;
		this.label_nick.text = SingletonMono<DataManager, AllScene>.Instance.nick;
		this.label_agentname.text = SingletonMono<DataManager, AllScene>.Instance.agentname;
		if (SingletonMono<DataManager, AllScene>.Instance.sex.Equals("0"))
		{
			this.label_sex.text = "男";
		}
		else
		{
			this.label_sex.text = "女";
		}
		this.label_coin.text = SingletonMono<DataManager, AllScene>.Instance.coin.ToString();
		if (SingletonMono<DataManager, AllScene>.Instance.phone == string.Empty)
		{
			this.label_phone.text = "无";
		}
		else
		{
			this.label_phone.text = SingletonMono<DataManager, AllScene>.Instance.phone;
		}
		if (SingletonMono<DataManager, AllScene>.Instance.vip == "0")
		{
			this.label_associator.text = "充值任意金额开通";
		}
		else
		{
			this.label_associator.text = SingletonMono<DataManager, AllScene>.Instance.vip;
		}
	}
	public void PlayAnim()
	{
		this.obj_mask.SetActive(true);
		this.tween_pos.PlayForward();
	}
	private void OnBackBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_mask.SetActive(true);
		this.tween_pos.PlayReverse();
	}
	private void OnAddCoinBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.bar_shop.PlayAnim();
	}
	private void OnAddAssociatorBtnClik()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.bar_shop.PlayAnim();
	}
	private void OnTweenPosCallbacck()
	{
		this.obj_mask.SetActive(!this.obj_mask.activeSelf);
	}
}
