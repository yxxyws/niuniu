using NiuNiu;
using System;
using UnityEngine;
public class GameChatManager : MonoBehaviour
{
	public static GameChatManager Instance;
	public TweenPosition tween_pos;
	public UIToggle toggle_expression;
	public UIToggle toggle_dialogue;
	public UIToggle toggle_chat;
	public GameObject obj_expression;
	public GameObject obj_dialogue;
	public GameObject obj_gameChat;
	public UIButton btn_mask;
	public UIButton[] all_expression;
	public UIButton[] all_dialogue;
	public PoolManager pool_gameChat;
	public UIGrid grid_gameChat;
	public UIInput input_chat;
	public UIButton btn_send;
	private void Awake()
	{
		GameChatManager.Instance = this;
		for (int i = 0; i < this.all_expression.Length; i++)
		{
			this.SetBtnCallback(this.all_expression[i], new Action<int>(this.OnExpressionBtnOnClick), i);
		}
		for (int j = 0; j < this.all_dialogue.Length; j++)
		{
			this.SetBtnCallback(this.all_dialogue[j], new Action<int>(this.OnDialogueBtnClick), j);
		}
		EventDelegate.Set(this.toggle_expression.onChange, new EventDelegate.Callback(this.OnExpressionToggleChange));
		EventDelegate.Set(this.toggle_dialogue.onChange, new EventDelegate.Callback(this.OnDialogueToggleChange));
		EventDelegate.Set(this.toggle_chat.onChange, new EventDelegate.Callback(this.OnGameChatToggleChange));
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnMaskBtnClick));
		EventDelegate.Set(this.btn_send.onClick, new EventDelegate.Callback(this.OnSendBtnClick));
		UIEventListener expr_10A = this.input_chat.GetComponent<UIEventListener>();
		expr_10A.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(expr_10A.onSelect, new UIEventListener.BoolDelegate(this.InputOnSelect));
		this.tween_pos.ResetToBeginning();
	}
	private void Start()
	{
	}
	public void ShowGameChat()
	{
		this.tween_pos.PlayForward();
		this.btn_mask.gameObject.SetActive(true);
	}
	private void OnMaskBtnClick()
	{
		this.tween_pos.PlayReverse();
		base.Invoke("AnimCallback", this.tween_pos.duration);
	}
	private void AnimCallback()
	{
		this.btn_mask.gameObject.SetActive(false);
	}
	private void OnExpressionToggleChange()
	{
		if (this.toggle_expression.value)
		{
			this.obj_expression.SetActive(true);
			this.obj_dialogue.SetActive(false);
			this.obj_gameChat.SetActive(false);
		}
	}
	private void OnDialogueToggleChange()
	{
		if (this.toggle_dialogue.value)
		{
			this.obj_expression.SetActive(false);
			this.obj_dialogue.SetActive(true);
			this.obj_gameChat.SetActive(false);
		}
	}
	private void OnGameChatToggleChange()
	{
		if (this.toggle_chat.value)
		{
			this.obj_expression.SetActive(false);
			this.obj_dialogue.SetActive(false);
			this.obj_gameChat.SetActive(true);
		}
	}
	private void SetBtnCallback(UIButton btn, Action<int> callback, int index)
	{
		EventDelegate.Set(btn.onClick, delegate
		{
			callback(index);
		});
	}
	private void OnExpressionBtnOnClick(int index)
	{
		NiuNiuNetManager.Instance.SendExpressionToRoom(index);
	}
	private void OnDialogueBtnClick(int index)
	{
		NiuNiuNetManager.Instance.SendDialogueToRoom(index);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnSendBtnClick()
	{
		if (this.input_chat.value == string.Empty)
		{
			TipManager.Instance.ShowTips("发送的内容不能为null", 2f);
		}
		else
		{
			NiuNiuNetManager.Instance.SendChatTextToRoom(this.input_chat.value);
			this.input_chat.value = string.Empty;
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void InputOnSelect(GameObject obj, bool isSelect)
	{
		SingletonMono<DataManager, AllScene>.Instance.IsSelectInput = isSelect;
	}
}
