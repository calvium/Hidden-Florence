using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IMStartMenu : MonoBehaviour {
	[SerializeField] TextMeshProUGUI titleTextBox, instructionsTextBox;
	[SerializeField] string[] texts;
	[SerializeField] private CanvasGroup startBackground, titleBox, subtitleBox;
	[SerializeField] private CanvasGroup scanFloorIcon, paintingIcon;
	[SerializeField] private float fadeSpeed;
	public bool foundPlanes;
	[SerializeField] private triggerMain arScript;
	[SerializeField] private GameObject sCol, paintingCol;
	// [SerializeField] private CanvasGroup infoCanvas;
	// [SerializeField] private GameObject infoObject;
    public CanvasGroup helpCanvas, startButtonCanvas;
    public Button startButton;
    public ScannerEffectDemo scannerEffectDemo;
	public Animator churchMove;
	

    // Use this for initialization
    void Start () {
		titleTextBox.text = texts[0];
		instructionsTextBox.text = "";
		titleBox.alpha=subtitleBox.alpha=helpCanvas.alpha=startButtonCanvas.alpha=0;
        startButton.interactable = false;
		foundPlanes = true;
		StartCoroutine(fadeOut(startBackground, 1f));
		StartCoroutine(fadeIn(titleBox));
		StartCoroutine(beginning());
		paintingCol.SetActive(false);
	}
	
	IEnumerator beginning(){ //Setting first text when you start
		titleTextBox.text = texts[0];
		yield return new WaitForSeconds(3f);
		StartCoroutine(fadeOut(titleBox));
		StartCoroutine(fadeIn(subtitleBox));
		StartCoroutine(fadeIn(scanFloorIcon));
		StartCoroutine(setText(1));
	}


	public void callSetText(int txt){
		StartCoroutine(setText(txt));

	}
	IEnumerator setText(int txt){
		Debug.Log("debugging --- " + txt);
		switch (txt){
			case 1:
				instructionsTextBox.text = texts[txt];
				yield return new WaitForSeconds(2f);
				foundPlanes = false;
				break;
			case 2:
				foundPlanes = true;
				yield return new WaitForSeconds(1f);
				StartCoroutine(fadeOut(scanFloorIcon));
				instructionsTextBox.text = texts[txt]; //Look at the centre panel of the painting
				arScript.active = true;
                break;
            //case 3:
            //    yield return new WaitForSeconds(1f);
            //    // StartCoroutine(fadeOut(scanFloorIcon, 1f));
            //    instructionsTextBox.text = texts[txt]; //Now take a few steps back and mind the bench!
            //    sCol.SetActive(true);
            //    break;
            //case 4:
            //    yield return new WaitForSeconds(0.5f);
            //    instructionsTextBox.text = texts[txt]; //Look back at the painting at tap start
            //    paintingCol.SetActive(true);
            //    paintingCol.GetComponent<paintingRaycast>().active = true;
            //    startButton.interactable = true;
            //    StartCoroutine(fadeIn(startButtonCanvas, 1f));
            //case 5:
            //    sCol.SetActive(false);
            //    paintingCol.SetActive(false);
            //    scannerEffectDemo.startPainting();
            //    churchMove.SetTrigger("go");
            //    StartCoroutine(fadeOut(startButtonCanvas, 1f));
            //    StartCoroutine(fadeOut(subtitleBox));
            //    break;
            case 3:
                yield return new WaitForSeconds(1f);
                // StartCoroutine(fadeOut(scanFloorIcon, 1f));
                churchMove.SetTrigger("go");
                instructionsTextBox.text = texts[txt];
                yield return new WaitForSeconds(5f);
                StartCoroutine(fadeOut(subtitleBox));
                scannerEffectDemo.startPainting();
                break;

		}
	}
    public void handleStartButtonPress()
    {
        callSetText(5);
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
			temp += Time.deltaTime*fadeSpeed;
			c.alpha=temp;
			yield return null;
		}
	}
	IEnumerator fadeOut(CanvasGroup c, float maxAlpha = 0.7f)
    {
		float temp = c.alpha = maxAlpha;
		yield return new WaitForSeconds(0.5f);
		while(temp>0){
			temp -= Time.deltaTime*fadeSpeed;
			c.alpha=temp;
			yield return null;
		}
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
