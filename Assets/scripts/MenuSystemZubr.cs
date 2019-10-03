using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour 
{
	public Image loadingBar;
	// [SerializeField] private CanvasGroup loadingCircle;
	[SerializeField] private CanvasGroup background;
	[SerializeField] private float speed;

	bool fadeStart;

	void Start()
	{
		StartCoroutine(fadeOut(background));
		fadeStart=false;
	}
	public void LoadScene(string sceneName)
	{
		Debug.Log("Loading scene :" + sceneName);
		// StartCoroutine(fadeIn(loadingCircle));
		StartCoroutine(Loading(sceneName));
	}

	private IEnumerator Loading(string sceneName)
	{
		yield return new WaitForSeconds(1f);
		yield return null;

		AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName);
		asyncOp.allowSceneActivation = false;
		Debug.Log("Loading scene :" + asyncOp.progress);

		while (!asyncOp.isDone)
		{
			// loadingCircle.alpha = 1;
			loadingBar.fillAmount = asyncOp.progress;

			if (asyncOp.progress >= 0.9f)
			{
				
				fadeStart=true;
				asyncOp.allowSceneActivation = true;
			}

			yield return null;
		}
	}

	IEnumerator fadeIn(CanvasGroup c){
		float temp = c.alpha = 0;
		yield return new WaitForSeconds(0.5f);
		while(temp<1){
			temp += Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}
	IEnumerator fadeOut(CanvasGroup c){
		float temp = c.alpha = 1;
		yield return new WaitForSeconds(0.5f);
		while(temp>0){
			temp -= Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}
}
