using System;
using System.Threading;
using UnityEngine;
public class SettingManager : MonoBehaviour
{
	public static SettingManager Instance;
	public TweenPosition tween_pos;
	public UIButton btn_mask;
	public UIButton btn_back;
	public UIButton btn_quit;
	public UIButton btn_changeUser;
	public UISlider slider_music;
	public UISlider slider_sound;
	private bool isPressBg;
	private bool isPressEffect;
	private static object m_lockBoo = new object();
	private void Awake()
	{
		object lockBoo = SettingManager.m_lockBoo;
		Monitor.Enter(lockBoo);
		try
		{
			if (SettingManager.Instance == null)
			{
				SettingManager.Instance = this;
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		finally
		{
			Monitor.Exit(lockBoo);
		}
		EventDelegate.Set(this.btn_mask.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		EventDelegate.Set(this.btn_back.onClick, new EventDelegate.Callback(this.OnBackBtnClick));
		UIEventListener expr_89 = this.slider_music.GetComponent<UIEventListener>();
		expr_89.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_89.onPress, new UIEventListener.BoolDelegate(this.BgSliderOnPress));
		EventDelegate.Set(this.slider_music.onChange, new EventDelegate.Callback(this.OnBgSoundSliderChange));
		UIEventListener expr_D2 = this.slider_sound.GetComponent<UIEventListener>();
		expr_D2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_D2.onPress, new UIEventListener.BoolDelegate(this.EffectSliderOnPress));
		EventDelegate.Set(this.slider_sound.onChange, new EventDelegate.Callback(this.OnEffectSoundSliderChange));
		EventDelegate.Set(this.btn_quit.onClick, new EventDelegate.Callback(this.OnQuitBtnClick));
		EventDelegate.Set(this.btn_changeUser.onClick, new EventDelegate.Callback(this.OnChangeUserBtnClick));
	}
	private void Start()
	{
		this.tween_pos.ResetToBeginning();
	}
	public void PlayAnim()
	{
		this.slider_music.value = SingletonMono<DataManager, AllScene>.Instance.BGSound / 1f;
		this.slider_sound.value = SingletonMono<DataManager, AllScene>.Instance.EffectSound / 1f;
		this.btn_mask.gameObject.SetActive(true);
		if (SingletonMono<Main, AllScene>.Instance.cutState == LobbyState.PlayGame)
		{
			this.btn_changeUser.gameObject.SetActive(false);
			this.btn_quit.gameObject.SetActive(false);
		}
		else
		{
			this.btn_changeUser.gameObject.SetActive(true);
			this.btn_quit.gameObject.SetActive(true);
		}
		this.tween_pos.PlayForward();
	}
	public void ChangeUserToReLoginPanel()
	{
		TipManager.Instance.HideWaitTip();
		this.OnBackBtnClick();
		PlayerPrefs.DeleteKey("username");
		SingletonMono<Main, AllScene>.Instance.SetGameStates(LobbyState.Login);
	}
	private void OnBackBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
		this.btn_mask.gameObject.SetActive(false);
		this.tween_pos.PlayReverse();
	}
	private void OnBgSoundSliderChange()
	{
		if (this.isPressBg)
		{
			SingletonMono<DataManager, AllScene>.Instance.SaveSoundValue(this.slider_music.value, SingletonMono<DataManager, AllScene>.Instance.EffectSound);
		}
	}
	private void OnEffectSoundSliderChange()
	{
		if (this.isPressEffect)
		{
			SingletonMono<DataManager, AllScene>.Instance.SaveSoundValue(SingletonMono<DataManager, AllScene>.Instance.BGSound, this.slider_sound.value);
		}
	}
	private void OnQuitBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		Application.Quit();
	}
	private void OnChangeUserBtnClick()
	{
		SingletonMono<DataManager, AllScene>.Instance.loginType = 1;
		TipManager.Instance.ShowWaitTip(string.Empty);
		SingletonMono<NetManager, AllScene>.Instance.Logout();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void BgSliderOnPress(GameObject obj, bool isPressed)
	{
		this.isPressBg = isPressed;
	}
	private void EffectSliderOnPress(GameObject obj, bool isPressed)
	{
		this.isPressEffect = isPressed;
	}
}
