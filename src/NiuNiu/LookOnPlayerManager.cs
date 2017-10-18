using com.max.JiXiangNiuNiu;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace NiuNiu
{
	public class LookOnPlayerManager : MonoBehaviour
	{
		public UIButton btn_close;
		public Dictionary<string, PlayerBase> allPlayer = new Dictionary<string, PlayerBase>();
		public PoolManager pool_playerIetmMana;
		public UIGrid grid_itemParent;
		private void Awake()
		{
			EventDelegate.Set(this.btn_close.onClick, new EventDelegate.Callback(this.OnCloseBtnClick));
		}
		public void AddPlayer(PlayerInfo info)
		{
			GameObject nGUIItem = this.pool_playerIetmMana.GetNGUIItem();
			nGUIItem.GetComponent<PlayerBase>().SetPlayerInfo(info);
			this.allPlayer.Add(info.username, nGUIItem.GetComponent<PlayerBase>());
			this.grid_itemParent.repositionNow = true;
		}
		public void RemovePlayer(PlayerInfo info)
		{
			if (this.allPlayer.ContainsKey(info.username))
			{
				PlayerBase playerBase = null;
				this.allPlayer.TryGetValue(info.username, out playerBase);
				playerBase.HidePlayer();
				this.pool_playerIetmMana.ResetIdleItem(playerBase.gameObject);
				this.grid_itemParent.repositionNow = true;
				this.allPlayer.Remove(info.username);
			}
		}
		public void UpdatePlayer(PlayerInfo info)
		{
			if (this.allPlayer.ContainsKey(info.username))
			{
				PlayerBase playerBase = null;
				this.allPlayer.TryGetValue(info.username, out playerBase);
				playerBase.SetPlayerInfo(info);
			}
			else
			{
				this.AddPlayer(info);
			}
		}
		public void OnCloseBtnClick()
		{
			base.GetComponent<UIPanel>().alpha = 0f;
		}
	}
}
