using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class Prefab_Friend : MonoBehaviour
{
	public UITexture pic_head;
	public UISprite pic_userState;
	public UILabel label_nick;
	public UILabel label_coin;
	public UILabel label_diamond;
	public UIButton btn_friend;
	public UIButton btn_operation;
	public GameObject obj_operation;
	public GameObject obj_addFriendRequest;
	public UIButton btn_addFriendRequest_mask;
	public UIButton btn_agree;
	public UIButton btn_refuse;
	public UIButton btn_operation_mask;
	public UIButton btn_lookAt;
	public UIButton btn_delect;
	private UserInfo friendInfo;
	public UserInfo PlayerInfo
	{
		get
		{
			return this.friendInfo;
		}
	}
	private void Awake()
	{
		EventDelegate.Set(this.btn_friend.onClick, new EventDelegate.Callback(this.OnFriendBtnClick));
		EventDelegate.Set(this.btn_operation.onClick, new EventDelegate.Callback(this.OnOperationBtnClick));
		EventDelegate.Set(this.btn_operation_mask.onClick, new EventDelegate.Callback(this.OnOperationMaskBtnClick));
		EventDelegate.Set(this.btn_lookAt.onClick, new EventDelegate.Callback(this.OnLookAtBtnClick));
		EventDelegate.Set(this.btn_delect.onClick, new EventDelegate.Callback(this.OnDelectBtnClick));
		EventDelegate.Set(this.btn_addFriendRequest_mask.onClick, new EventDelegate.Callback(this.OnAddFriendRequestMaskBtnClick));
		EventDelegate.Set(this.btn_agree.onClick, new EventDelegate.Callback(this.OnAgreeBtnClick));
		EventDelegate.Set(this.btn_refuse.onClick, new EventDelegate.Callback(this.OnRefuseBtnClick));
	}
	public void UpdateShow(UserInfo info)
	{
		AsyncImageDownload.Instance.SetAsyncImage(info.headUrl, new Action<Texture2D>(this.AsyncGetHeadCallback));
		this.label_nick.text = info.nick;
		this.label_coin.text = DataManager.ChangeDanWei(info.coin);
		this.label_diamond.text = DataManager.ChangeDanWei(info.diamond);
		if (info.userState == 0)
		{
			this.pic_userState.spriteName = "pic_friend_NotOnline";
		}
		else
		{
			this.pic_userState.spriteName = "pic_friend_online";
		}
		if (info.isAdd == 1)
		{
			this.btn_operation.GetComponent<UISprite>().spriteName = "btn_friend_operation_02";
			this.btn_operation.normalSprite = "btn_friend_operation_02";
		}
		else
		{
			if (this.friendInfo != null && this.friendInfo.isAdd == 1)
			{
				TipManager.Instance.HideWaitTip();
			}
			this.btn_operation.GetComponent<UISprite>().spriteName = "btn_friend_operation_00";
			this.btn_operation.normalSprite = "btn_friend_operation_00";
		}
		this.friendInfo = info;
	}
	private void AsyncGetHeadCallback(Texture2D tex)
	{
		this.pic_head.mainTexture = tex;
	}
	public bool TipPointIsShow()
	{
		return this.friendInfo.isAdd == 1;
	}
	private void OnFriendBtnClick()
	{
		FriendManager.Instance.ShowFriendInfoBar(this.friendInfo);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
	}
	private void OnOperationBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		if (this.btn_operation.normalSprite == "btn_friend_operation_00")
		{
			this.obj_operation.SetActive(true);
		}
		else
		{
			this.obj_addFriendRequest.SetActive(true);
		}
	}
	private void OnOperationMaskBtnClick()
	{
		this.obj_operation.SetActive(false);
	}
	private void OnLookAtBtnClick()
	{
	}
	private void OnDelectBtnClick()
	{
		this.obj_operation.SetActive(false);
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SingletonMono<NetManager, AllScene>.Instance.SendDelectFriend(this.friendInfo.username);
	}
	private void OnAddFriendRequestMaskBtnClick()
	{
		this.obj_addFriendRequest.SetActive(false);
	}
	private void OnAgreeBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		SingletonMono<NetManager, AllScene>.Instance.SendAgreeAddFriend(this.friendInfo.username);
		TipManager.Instance.ShowWaitTip(string.Empty);
		this.obj_addFriendRequest.SetActive(false);
	}
	private void OnRefuseBtnClick()
	{
		SoundManager.Instance.PlaySound(SoundType.UI, "button");
		this.obj_addFriendRequest.SetActive(false);
		SingletonMono<NetManager, AllScene>.Instance.SendRefuseAddFriend(this.friendInfo.username);
	}
}
