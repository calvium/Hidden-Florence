using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour 
{
	public Image loadingBar;

	public void LoadScene(string sceneName)
	{
		Debug.Log("Loading scene :" + sceneName);
		StartCoroutine(Loading(sceneName));

	}

	private IEnumerator Loading(string sceneName)
	{
		yield return null;

		AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
		asyncOp.allowSceneActivation = false;
		Debug.Log("Loading scene :" + asyncOp.progress);

		while (!asyncOp.isDone)
		{
			loadingBar.fillAmount = asyncOp.progress;

			if (asyncOp.progress >= 0.9f)
			{
				asyncOp.allowSceneActivation = true;
			}

			yield return null;
		}
	}
}
