using System;
using UnityEngine;
public class Panel_Lobby : Panel_Basic
{
	public TweenPosition tween_lobbyTopBar;
	public TweenPosition tween_LobbyDeskBar;
	public TweenPosition tween_siteDeskBar;
	public TweenAlpha tween_siteTopBar;
	public TweenAlpha tween_gameSelect;
	public TweenAlpha tween_siteLevle;
	public UITexture pic_head;
	public UILabel label_nick;
	public UILabel label_count;
	public UILabel[] label_minDownBet;
	public UILabel[] label_joinRoomMinCoin;
	public UISprite pic_tip;
	public UIButton btn_add;
	public UIButton btn_niuniu;
	public UIButton btn_shuangJian;
	public UIButton btn_playerHead;
	public UIButton btn_safe;
	public UIButton btn_generalize;
	public UIButton btn_task;
	public UIButton btn_notice;
	public UIButton btn_openFrirnd;
	public UIButton btn_setting;
	public UIButton btn_shop;
	public UIButton btn_backGameSelect;
	public UIButton btn_chuJiChang;
	public UIButton btn_ZhongJiChang;
	public UIButton btn_GaoJiChang;
	public GameObject bar_Generalize;
	public GameObject bar_safe;
	public GameObject bar_task;
	public GameObject bar_notice;
	public GameObject bar_userInfo;
	public GameObject bar_shop;
	public GameObject[] obj_friendTip;
	public GameObject obj_DaiLiBar;
	public UIButton btn_openFriend_02;
	public UIButton btn_shop02;
	public UIButton btn_paiHang;
	public UIButton btn_liQuan;
	public UIButton btn_paiMaiHang;
	public UIButton btn_more;
	public MoreBarManager moreBar;
	private bool isShowCoin = true;
	private void Awake()
	{
		EventDelegate.Set(this.btn_niuniu.onClick, new EventDelegate.Callback(this.OnNiuNiuBtnClick));
		EventDelegate.Set(this.btn_shuangJian.onClick, new EventDelegate.Callback(this.OnShuangJianBtnClick));
		EventDelegate.Set(this.btn_playerHead.onClick, new EventDelegate.Callback(this.OnPlayerHeadBtnClick));
		EventDelegate.Set(this.btn_generalize.onClick, new EventDelegate.Callback(this.OnGeneralizeBtnClick));
		EventDelegate.Set(this.btn_safe.onClick, new EventDelegate.Callback(this.OnSafeBtnClick));
		EventDelegate.Set(this.btn_notice.onClick, new EventDelegate.Callback(this.OnNoticeBtnClick));
		EventDelegate.Set(this.btn_setting.onClick, new EventDelegate.Callback(this.OnSettingBtnClick));
		EventDelegate.Set(this.btn_task.onClick, new EventDelegate.Callback(this.OnTaskBtnClick));
		EventDelegate.Set(this.btn_add.onClick, new EventDelegate.Callback(this.OnShopBtnClick));
		EventDelegate.Set(this.btn_shop.onClick, new EventDelegate.Callback(this.OnShopBtnClick));
		EventDelegate.Set(this.btn_openFrirnd.onClick, new EventDelegate.Callback(this.OnOpenFriendBtnClick));
		EventDelegate.Set(this.btn_backGameSelect.onClick, new EventDelegate.Callback(this.OnBackGameSelectBtnClick));
		EventDelegate.Set(this.btn_chuJiChang.onClick, new EventDelegate.Callback(this.OnChuJiChangBtnClick));
		EventDelegate.Set(this.btn_ZhongJiChang.onClick, new EventDelegate.Callback(this.OnZhongJiChangBtnClick));
		EventDelegate.Set(this.btn_GaoJiChang.onClick, new EventDelegate.Callback(this.OnGaoJiChangBtnClick));
		EventDelegate.Set(this.btn_openFriend_02.onClick, new EventDelegate.Callback(this.OnOpenFriendBtnClick));
		EventDelegate.Set(this.btn_shop02.onClick, new EventDelegate.Callback(this.OnShopBtnClick));
		EventDelegate.Set(this.btn_paiHang.onClick, new EventDelegate.Callback(this.OnPaiHangBtnClick));
		EventDelegate.Set(this.btn_liQuan.onClick, new EventDelegate.Callback(this.OnLiQuanBtnClick));
		EventDelegate.Set(this.btn_paiMaiHang.onClick, new EventDelegate.Callback(this.OnPaiMaiHangBtnClick));
		EventDelegate.Set(this.btn_more.onClick, new EventDelegate.Callback(this.OnMoreBtnClick));
		FriendManager.UpdateFriendTip = (Action<bool>)Delegate.Combine(FriendManager.UpdateFriendTip, new Action<bool>(this.UpdateFriendTipShow));
	}
	private void Start()
	{
		this.tween_LobbyDeskBar.ResetToBeginning();
		this.tween_lobbyTopBar.ResetToBeginning();
		this.tween_siteDeskBar.ResetToBeginning();
		this.tween_siteTopBar.ResetToBeginning();
		this.tween_gameSelect.ResetToBeginning();
		this.tween_siteLevle.ResetToBeginning();
		UIManager expr_47 = UIManager.Instance;
		expr_47.ShowTaskBar = (Action)Delegate.Combine(expr_47.ShowTaskBar, new Action(this.ShowTaskBar));
		UIManager expr_6D = UIManager.Instance;
		expr_6D.IsShowDaiLiBar = (Action<bool>)Delegate.Combine(expr_6D.IsShowDaiLiBar, new Action<bool>(this.IsShowDaiLiBar));
		UIManager expr_93 = UIManager.Instance;
		expr_93.UpdateUserInfoShow = (Action)Delegate.Combine(expr_93.UpdateUserInfoShow, new Action(this.UpdateShowCoinOrDiamond));
		UIManager expr_B9 = UIManager.Instance;
		expr_B9.ShowSafeBar = (Action)Delegate.Combine(expr_B9.ShowSafeBar, new Action(this.OpenSafeBar));
		FriendManager.Instance.friendBar.UpdateFriendTip();
	}
	private void OnDestroy()
	{
		FriendManager.UpdateFriendTip = (Action<bool>)Delegate.Remove(FriendManager.UpdateFriendTip, new Action<bool>(this.UpdateFriendTipShow));
	}
	protected override void setUID()
	{
		this.mUID = PUID.Lobby;
	}
	protected override void setPanelType()
	{
		this.mType = PanelType.Normal;
	}
	public void InitShow()
	{
		SoundManager.Instance.PlaySound(SoundType.BG, "lobbgBg");
		for (int i = 0; i < this.label_minDownBet.Length; i++)
		{
			this.label_minDownBet[i].text = "最小下注：" + DataManager.ChangeDanWei(SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[i].minDownBet);
			this.label_joinRoomMinCoin[i].text = DataManager.ChangeDanWei(SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[i].joinRoomMinCoin) + "准入";
		}
		if (SingletonMono<DataManager, AllScene>.Instance.headTexture == null)
		{
			AsyncImageDownload.Instance.SetAsyncImage(SingletonMono<DataManager, AllScene>.Instance.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
		}
		else
		{
			this.pic_head.mainTexture = SingletonMono<DataManager, AllScene>.Instance.headTexture;
		}
		this.label_nick.text = SingletonMono<DataManager, AllScene>.Instance.nick;
		this.UpdateShowCoinOrDiamond();
	}
	private void AsyncGetHeadCallback(Texture2D tex)
	{
		this.pic_head.mainTexture = tex;
		SingletonMono<DataManager, AllScene>.Instance.headTexture = tex;
	}
	public void UpdateShowCoinOrDiamond()
	{
		if (this.isShowCoin)
		{
			this.label_count.text = DataManager.ChangeDanWei(SingletonMono<DataManager, AllScene>.Instance.coin);
			this.pic_tip.spriteName = "pic_gold";
		}
		else
		{
			this.label_count.text = DataManager.ChangeDanWei(SingletonMono<DataManager, AllScene>.Instance.diamond);
			this.pic_tip.spriteName = "pic_diamond";
		}
	}
	private void UpdateFriendTipShow(bool isShow)
	{
		for (int i = 0; i < this.obj_friendTip.Length; i++)
		{
			this.obj_friendTip[i].SetActive(isShow);
		}
	}
	public void OpenSafeBar()
	{
		this.bar_safe.GetComponent<SafaManager>().InitShow();
		this.bar_safe.SetActive(true);
		TipManager.Instance.HideWaitTip();
		if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
		{
			SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("safe"));
		}
	}
	public void IsShowDaiLiBar(bool isShow)
	{
		this.obj_DaiLiBar.SetActive(isShow);
	}
	private void ShowTaskBar()
	{
		this.bar_task.GetComponent<TaskManager>().UpdateSignShow();
		this.bar_task.SetActive(true);
	}
	private void OnNiuNiuBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "click");
		this.tween_siteLevle.PlayForward();
		this.tween_gameSelect.PlayForward();
		this.tween_LobbyDeskBar.PlayForward();
		this.tween_siteDeskBar.PlayForward();
		this.tween_lobbyTopBar.PlayForward();
		this.tween_siteTopBar.PlayForward();
	}
	private void OnShuangJianBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "click");
		TipManager.Instance.ShowTipsBar("该游戏正在开发中...", delegate
		{
			TipManager.Instance.HideTipBar();
		});
	}
	private void OnBackGameSelectBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.tween_siteLevle.PlayReverse();
		this.tween_gameSelect.PlayReverse();
		this.tween_LobbyDeskBar.PlayReverse();
		this.tween_siteDeskBar.PlayReverse();
		this.tween_lobbyTopBar.PlayReverse();
		this.tween_siteTopBar.PlayReverse();
	}
	private void OnChuJiChangBtnClick()
	{
		this.SiteSelect(0);
	}
	private void OnZhongJiChangBtnClick()
	{
		this.SiteSelect(1);
	}
	private void OnGaoJiChangBtnClick()
	{
		this.SiteSelect(2);
	}
	private void SiteSelect(int siteIndex)
	{
		if (SingletonMono<DataManager, AllScene>.Instance.coin >= SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[siteIndex].joinRoomMinCoin)
		{
			if (SingletonMono<DataManager, AllScene>.Instance.coin < SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[siteIndex].joinRoomMaxCoin)
			{
				SingletonMono<DataManager, AllScene>.Instance.CurSiteIndex = siteIndex;
				TipManager.Instance.ShowWaitTip("正在加入房间...");
				SingletonMono<NetManager, AllScene>.Instance.SendJoinRoom();
				if (SingletonMono<DataManager, AllScene>.Instance.IsRecordOperation())
				{
					SingletonMono<DataManager, AllScene>.Instance.curAllOperation.Add(new OperationRecord("room"));
				}
			}
			else
			{
				TipManager.Instance.ShowTipsBar("你的金币数量超过" + SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[siteIndex].joinRoomMaxCoin + ", 不能进入该房间", delegate
				{
					TipManager.Instance.HideTipBar();
				});
			}
		}
		else
		{
			TipManager.Instance.ShowTipsBar("你的金币数量少于" + SingletonMono<DataManager, AllScene>.Instance.allSiteInfo[siteIndex].joinRoomMinCoin + ", 不能进入该房间", delegate
			{
				TipManager.Instance.HideTipBar();
			});
		}
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnGeneralizeBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.bar_Generalize.SetActive(true);
	}
	private void OnSafeBtnClick()
	{
		TipManager.Instance.ShowWaitTip(string.Empty);
		SingletonMono<NetManager, AllScene>.Instance.SendOpenSafeClick();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnPlayerHeadBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.bar_userInfo.SetActive(true);
		this.bar_userInfo.GetComponent<PlayerBarManager>().InitShow();
		this.bar_userInfo.GetComponent<PlayerBarManager>().PlayAnim();
	}
	private void OnTaskBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("该功能正在开发中...");
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnNoticeBtnClick()
	{
		this.bar_notice.GetComponent<NoticeManager>().InitShow();
		this.bar_notice.SetActive(true);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnSettingBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SoundManager.Instance.PlaySound(SoundType.EFFECT, "slide_open");
		SettingManager.Instance.PlayAnim();
	}
	public void OnShopBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.bar_shop.GetComponent<ShopManager>().PlayAnim();
	}
	private void OnOpenFriendBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SingletonMono<NetManager, AllScene>.Instance.SendGetBuddyList();
		TipManager.Instance.ShowWaitTip(string.Empty);
	}
	private void OnPaiHangBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("该功能正在开发中...");
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnLiQuanBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("该功能正在开发中...");
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnPaiMaiHangBtnClick()
	{
		TipManager.Instance.ShowFeedbackTipsBar("该功能正在开发中...");
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnMoreBtnClick()
	{
		this.moreBar.PlayAnim();
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
}
