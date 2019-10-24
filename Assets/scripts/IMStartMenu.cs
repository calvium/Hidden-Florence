using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IMStartMenu : MonoBehaviour {
	[SerializeField] TextMeshProUGUI titleTextBox, instructionsTextBox;
	[SerializeField] string[] texts;
	[SerializeField] private CanvasGroup startBackground, titleBox, subtitleText, subtitleBox;
	[SerializeField] private float speed;
	[SerializeField] private CanvasGroup scanFloorIcon, paintingIcon, churchIcon;
	public Image dialChurch;
	[SerializeField] private activateChurchTriggers _act;
	[SerializeField] private debugLogTextScript dbScript;
	public bool foundH = true;
	public bool foundV = true;

	[SerializeField] private triggerMain arScript;
	[SerializeField] private GameObject sCol, rayCastTarget, unscanners;

	[SerializeField] private CanvasGroup infoCanvas;
	[SerializeField] private GameObject infoObject;
	// [SerializeField] private GameObject boundary;//
	public GameObject paintings;

    public CanvasGroup helpCanvas;
    public CanvasGroup startButtonCanvas;
    public Button startButton;
    public ScannerEffectDemo scannerEffectDemo;


    // Use this for initialization
    void Start () {
		titleTextBox.text = texts[0];
		instructionsTextBox.text = "";
		titleBox.alpha=subtitleBox.alpha=helpCanvas.alpha=startButtonCanvas.alpha=0;
        startButton.interactable = false;
        startBackground.alpha=1;
		rayCastTarget.SetActive(false);
		unscanners.SetActive(false);
		// boundary.SetActive(false);
		StartCoroutine(fadeOut(startBackground));
		StartCoroutine(fadeIn(titleBox));
		StartCoroutine(beginning());
		paintings.SetActive(false);
	}
	
	IEnumerator beginning(){ //Setting first text when you start
		titleTextBox.text = texts[0];
		yield return new WaitForSeconds(5f);
		StartCoroutine(fadeOut(titleBox));
		StartCoroutine(fadeIn(subtitleBox));
		StartCoroutine(fadeIn(scanFloorIcon));
		instructionsTextBox.text = texts[1];
		foundH = foundV = false;
	}


	public void callSetText(int txt){
		StartCoroutine(setText(txt));

	}
	IEnumerator setText(int txt){
		switch (txt){
			case 2:
				dbScript.addToString("found floor");
				yield return new WaitForSeconds(1f);
				Debug.Log("debugging case 02");
				instructionsTextBox.text = texts[txt]; //Now scan the walls with your device
				foundH = true;
				break;
			case 3:
				dbScript.addToString("found Walls");
				yield return new WaitForSeconds(1f);
				Debug.Log("debugging case 03");
				StartCoroutine(fadeOut(scanFloorIcon));
				instructionsTextBox.text = texts[txt]; //Look at the centre panel of the painting
				foundV = true;
				arScript.active = true;
				break;
			case 4:
				Debug.Log("debugging case 04");
				yield return new WaitForSeconds(1f);
				StartCoroutine(fadeOut(scanFloorIcon));
				paintings.SetActive(true);
				instructionsTextBox.text = texts[txt]; //See if you can find the church
				yield return new WaitForSeconds(1f);
				rayCastTarget.SetActive(true);
				dbScript.addToString("found Trigger");
				break;
			case 5:
				instructionsTextBox.text = texts[txt]; //Now take a few steps back and mind the bench!
				Debug.Log("debugging case 05");
				yield return new WaitForSeconds(0.5f);
				sCol.SetActive(true);
				dbScript.addToString("found church");
                yield return new WaitForSeconds(5f);
                callSetText(6);
                break;
			case 6:
				instructionsTextBox.text = texts[txt]; //Look back at the painting
				Debug.Log("debugging case 06");
                startButton.interactable = true;
                StartCoroutine(fadeIn(startButtonCanvas));
				//_act.bigRaycast.SetActive(true);
				//dbScript.addToString("hit box");
				break;
			case 7:
				dbScript.addToString("hit raycast");
                StartCoroutine(fadeOut(startButtonCanvas));
                StartCoroutine(fadeOut(subtitleBox));
				// StartCoroutine(turnOnBoundaries());
				break;
		}
	}
	public void two(){
		//StartCoroutine(fadeOut(subtitleBox));
	}
    public void handleStartButtonPress()
    {
        scannerEffectDemo.startPainting();
        callSetText(7);
    }
    public void handleHelpButtonPress()
    {
        StartCoroutine(fadeIn(helpCanvas, 1f));
    }

    public void handleCloseHelpPanelPress()
    {
        StartCoroutine(fadeOut(helpCanvas, 1f));
    }

    IEnumerator fadeIn(CanvasGroup c, float maxAlpha = 0.7f){
		float temp = c.alpha = 0;
		yield return new WaitForSeconds(0.5f);
		while(temp<maxAlpha){
			temp += Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}
	IEnumerator fadeOut(CanvasGroup c, float maxAlpha = 0.7f)
    {
		float temp = c.alpha = maxAlpha;
		yield return new WaitForSeconds(0.5f);
		while(temp>0){
			temp -= Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}

	public void boundariesOn(){
		StartCoroutine(turnOnBoundaries());
	}
	IEnumerator turnOnBoundaries(){
		yield return new WaitForSeconds(2f);
		// boundary.SetActive(true);
		unscanners.SetActive(true);
	}


	public void fadeInInfo(){
		infoObject.SetActive(true);
		StartCoroutine(fadeIn(infoCanvas));
	}
	public void fadeOutInfo(){
		infoObject.SetActive(false);
		StartCoroutine(fadeOut(infoCanvas));
	}
	public void callBackButton(){
		StartCoroutine(back());
	}
	IEnumerator back(){
		StartCoroutine(fadeIn(startBackground));
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene("MainMenu");
	}

	public void callStop(){
		StartCoroutine(stopUser());
	}
		

	IEnumerator stopUser(){
		instructionsTextBox.text = "Watch out for the paintings!";
		StartCoroutine(fadeIn(subtitleBox));
		yield return new WaitForSeconds(3f);
		StartCoroutine(fadeOut(subtitleBox));
	}

}
