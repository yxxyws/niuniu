using com.max.JiXiangLobby;
using System;
using UnityEngine;
public class GiveRecord : MonoBehaviour
{
	public UILabel label_id;
	public UILabel label_nick;
	public UILabel label_coin;
	public UILabel label_time;
	public void InitShow(Record info)
	{
		this.label_id.text = info.username.ToString();
		this.label_nick.text = info.nick;
		this.label_coin.text = DataManager.ChangeDanWei(info.coin);
		this.label_time.text = info.time.Replace(" ", "\n");
	}
}
