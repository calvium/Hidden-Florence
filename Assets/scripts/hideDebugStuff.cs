using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideDebugStuff : MonoBehaviour {
	[SerializeField] private bool active = true;
	[SerializeField] private Material hLines;
	[SerializeField] private Color hLineCol;
	[SerializeField] private Material vLines;
	[SerializeField] private Color vLineCol;
	[SerializeField] private Color hiddenLines;
	[SerializeField] private CanvasGroup debugText;
	[SerializeField] private debugLogTextScript dbScript;
	[SerializeField] private GameObject fpsBox;
	[SerializeField] private Renderer triggerBox;

	// Use this for initialization
	void Start () {
		// dbScript = GameObject.Find("debugText").GetComponent<debugLogTextScript>();
		switchDebug();
	}
	
	public void switchDebug(){
		if(active){
			hLines.SetColor("_Color", hiddenLines);
			vLines.SetColor("_Color", hiddenLines);
			debugText.alpha = 0;
			active = false;
			fpsBox.SetActive(false);
			triggerBox.enabled = false;
			dbScript.addToString("debug off");
		}else{
			hLines.SetColor("_Color", hLineCol);
			vLines.SetColor("_Color", vLineCol);
			debugText.alpha = 1;
			active = true;
			fpsBox.SetActive(true);
			triggerBox.enabled = true;
			dbScript.addToString("debug on");
		}
	}
}
