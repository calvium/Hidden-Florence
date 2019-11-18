using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activateChurchTriggers : MonoBehaviour {

	[SerializeField] private GameObject churchImg;
	[SerializeField] private GameObject triggerCube;
	public GameObject smlRaycast, bigRaycast;
	[SerializeField] private Material glowmat;
	[SerializeField] private debugLogTextScript dbScript;
	[SerializeField] private IMStartMenu menu;
	public bool active = true;
	[SerializeField] private bool started;
	[SerializeField] private float downTime;
	[SerializeField] private float activateTime;
	public scanTrigger _scanTrigger;

	// Use this for initialization
	void Start () {
		// dbScript = GameObject.Find("debugText").GetComponent<debugLogTextScript>();
		// menu = GameObject.Find("canvasUI").GetComponent<IMStartMenu>();
		// churchImg.SetActive(false);
		// triggerCube.SetActive(false);
		// bigRaycast.SetActive(false);
	}

	void Update(){
		// float posX = Screen.width / 2f;
		// float posY = Screen.height / 2f;
		// Ray ray = Camera.main.ScreenPointToRay(new Vector3(posX, posY, Mathf.Infinity));
		// RaycastHit hit;

        // if (Physics.Raycast(ray, out hit, 200)) {
		// 	if(smlRaycast == hit.collider.gameObject){
		// 		Debug.Log("You have selected the " + hit.collider.name);
		// 		started = true;
		// 	}else{
		// 		started = false;
		// 	}
		// 	if(bigRaycast == hit.collider.gameObject){
		// 		_scanTrigger.startScan();
		// 	}
		// }
		// if(started && downTime<activateTime){
		// 	downTime += Time.deltaTime;
		// 	menu.dialChurch.fillAmount = downTime/activateTime;
		// 	if(downTime >= activateTime){
		// 		turnOn();
		// 		smlRaycast.SetActive(false);
		// 	}
		// }
	}
	
	// Update is called once per frame
	// public void turnOn(){
	// 	// menu.dialChurch.fillAmount=0;
	// 	churchImg.SetActive(true);
	// 	triggerCube.SetActive(true);
	// 	Debug.Log("debugging Turnt on");
	// 	dbScript.addToString("turned on");
	// 	menu.callSetText(5);
	// }

	// IEnumerator fadeIn(){
	// 	float temp = 0f;
	// 	while(temp<1f){
	// 		temp += Time.deltaTime*0.3f;
	// 		glowmat.SetFloat("_Alpha", temp);
	// 		yield return null;
	// 	}
	// }
}
