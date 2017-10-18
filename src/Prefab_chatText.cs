using com.max.JiXiangNiuNiu;
using System;
using UnityEngine;
public class Prefab_chatText : MonoBehaviour
{
	public UIWidget widget_chat;
	public GameObject obj_slef;
	public UITexture pic_self_head;
	public UISprite pic_self_chatTextBg;
	public UILabel label_self_chatText;
	public GameObject obj_friend;
	public UITexture pic_friend_head;
	public UISprite pic_friend_chatTextBg;
	public UILabel label_friend_chatText;
	public void InitShow(bool isSelf, string chat, string headUrl = "")
	{
		if (isSelf)
		{
			this.pic_self_head.mainTexture = SingletonMono<DataManager, AllScene>.Instance.headTexture;
			this.label_self_chatText.text = chat;
			this.widget_chat.height = this.label_self_chatText.height + 64;
			this.obj_slef.SetActive(true);
			this.obj_friend.SetActive(false);
		}
		else
		{
			AsyncImageDownload.Instance.SetAsyncImage(headUrl, new Action<Texture2D>(this.GetHeadCallback));
			this.label_friend_chatText.text = chat;
			this.widget_chat.height = this.label_self_chatText.height + 64;
			this.obj_slef.SetActive(false);
			this.obj_friend.SetActive(true);
		}
	}
	private void GetHeadCallback(Texture2D tex)
	{
		this.pic_friend_head.mainTexture = tex;
	}
}
public class Prefab_ChatText : MonoBehaviour
{
	public GameObject obj_left;
	public GameObject obj_right;
	public UILabel label_left_chatText;
	public UITexture pic_left_head;
	public UILabel label_right_chatText;
	public UITexture pic_right_head;
	public void InitShow(Msg msg)
	{
		if (msg.gameChat.username == SingletonMono<DataManager, AllScene>.Instance.username)
		{
			this.obj_left.SetActive(false);
			this.obj_right.SetActive(true);
			this.pic_right_head.mainTexture = SingletonMono<DataManager, AllScene>.Instance.headTexture;
			this.label_right_chatText.text = msg.gameChat.chatText;
		}
		else
		{
			AsyncImageDownload.Instance.SetAsyncImage(msg.gameChat.headUrl, new Action<Texture2D>(this.SetHeadCallback));
			this.label_left_chatText.text = msg.gameChat.chatText;
			this.obj_left.SetActive(true);
			this.obj_right.SetActive(false);
		}
	}
	private void SetHeadCallback(Texture2D tex)
	{
		this.pic_left_head.mainTexture = tex;
	}
}
