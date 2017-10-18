using System;
using UnityEngine;
using UnityEngine.SceneManagement;
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	public string levelName;
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			SceneManager.LoadScene(this.levelName);
		}
	}
}
