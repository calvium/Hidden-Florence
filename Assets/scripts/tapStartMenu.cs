using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class tapStartMenu : MonoBehaviour {

	[SerializeField] private CanvasGroup startBackground;
	[SerializeField] private float speed;
	[SerializeField] private CanvasGroup titleBox;
	[SerializeField] private CanvasGroup subtitleBox;
	[SerializeField] private CanvasGroup slider;
	[SerializeField] private Material planeMat;
	[SerializeField] private Texture glowPlane;
	[SerializeField] private Texture nonPlane;
	[SerializeField] private CanvasGroup infoCanvas;
	// [SerializeField] private GameObject infoObject;

	// Use this for initialization
	void Start () {
		StartCoroutine(fadeOut(startBackground));
		planeMat.SetTexture("_MainTex", glowPlane);
		titleBox.alpha=subtitleBox.alpha=slider.alpha=0;
		StartCoroutine(fadeIn(titleBox));
		StartCoroutine(beginning());
	}
	
	IEnumerator beginning(){ //Setting first text when you start
		yield return new WaitForSeconds(5f);
		StartCoroutine(fadeOut(titleBox));
		StartCoroutine(fadeIn(subtitleBox));
		StartCoroutine(fadeIn(slider));
	}
	

	public void fadeAll(){
		StartCoroutine(fadeOut(slider));
		StartCoroutine(fadeOut(subtitleBox));
		planeMat.SetTexture("_MainTex", nonPlane);
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

	public void fadeInInfo(){
		// infoObject.SetActive(true);
		infoCanvas.blocksRaycasts=true;
		infoCanvas.interactable=true;
		StartCoroutine(fadeIn(infoCanvas));
	}
	public void fadeOutInfo(){
		// infoObject.SetActive(false);
		StartCoroutine(fadeOut(infoCanvas));
		// infoObject.SetActive(false);
		infoCanvas.interactable=false;
		infoCanvas.blocksRaycasts=false;
	}

	public void callBackButton(){
		StartCoroutine(back());
	}
	IEnumerator back(){
		StartCoroutine(fadeIn(startBackground));
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene("MainMenu");
	}

}
