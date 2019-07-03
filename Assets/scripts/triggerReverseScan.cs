using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerReverseScan : MonoBehaviour {
	public ScannerEffectDemo scanScript;
	[SerializeField] private bool scanning = false;
	private Camera cam;
	[SerializeField] private churchScript church;
	// [SerializeField] private mainPaintingScript painting;

	[SerializeField] private float dist;

	void Start() {
		cam = Camera.main;
		scanScript = cam.GetComponent<ScannerEffectDemo>();
		church = GameObject.FindGameObjectWithTag("church").GetComponent<churchScript>();
		// painting = GameObject.FindGameObjectWithTag("altarpiece").GetComponent<mainPaintingScript>();
	}

    // void OnTriggerExit(Collider col) {
	// 	Debug.Log("left the box");
    //     if (col.gameObject == Camera.main.gameObject && !scanning) {
	// 		StartCoroutine(scanningEvents());
	// 	}
    // }	

	IEnumerator scanningEvents(){
		Debug.Log("scanning1");
		// painting.reversePainting();
		yield return new WaitForSeconds (0.1f);
		scanScript.reversePainting();
		scanning = true;		
	}
	void Update(){
		dist = Vector3.Distance(this.transform.position, cam.transform.position);
		if(dist > 3){
			scanScript.ScanDistance = 30-(dist*5);
		}
	}
}
