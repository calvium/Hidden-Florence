using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IMStartMenu : MonoBehaviour {
	[SerializeField] TextMeshProUGUI titleTextBox;
	[SerializeField] TextMeshProUGUI instructionsTextBox;
	[SerializeField] string[] texts;
	[SerializeField] private CanvasGroup startBackground;
	[SerializeField] private CanvasGroup titleBox;
	[SerializeField] private CanvasGroup subtitleText;
	[SerializeField] private CanvasGroup subtitleBox;
	[SerializeField] private float speed;
	[SerializeField] private CanvasGroup scanFloorIcon;
	[SerializeField] private CanvasGroup paintingIcon;
	[SerializeField] private CanvasGroup churchIcon;

	// Use this for initialization
	void Start () {
		titleTextBox.text = texts[0];
		instructionsTextBox.text = "";
		titleBox.alpha=subtitleBox.alpha=0;
		startBackground.alpha=1;

		StartCoroutine(fadeOut(startBackground));
		StartCoroutine(fadeIn(titleBox));
		StartCoroutine(beginning());
	}
	
	IEnumerator beginning(){ //Setting first text when you start
		titleTextBox.text = texts[0];
		yield return new WaitForSeconds(5f);
		StartCoroutine(fadeOut(titleBox));
		StartCoroutine(fadeIn(subtitleBox));
		StartCoroutine(fadeIn(scanFloorIcon));
		instructionsTextBox.text = texts[1];
	}
	// Update is called once per frame

	public void callSetText(int txt){
		switch (txt){
			case 2:
				Debug.Log("debugging case 02");
				break;
			case 3:
				Debug.Log("debugging case 03");
				StartCoroutine(fadeOut(scanFloorIcon));
				StartCoroutine(fadeIn(paintingIcon));
				break;
			case 4:
				Debug.Log("debugging case 03");
				StartCoroutine(fadeOut(paintingIcon));
				StartCoroutine(fadeIn(churchIcon));
				break;
		}
		StartCoroutine(setText(txt));
	}
	IEnumerator setText(int txt){
		instructionsTextBox.text = texts[txt];
		yield return new WaitForSeconds(1f);

	}
	public void two(){
		StartCoroutine(fadeOut(subtitleBox));
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
