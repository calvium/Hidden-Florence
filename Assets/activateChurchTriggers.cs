using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateChurchTriggers : MonoBehaviour {

	[SerializeField] private GameObject churchImg;
	[SerializeField] private GameObject triggerCube;

	// Use this for initialization
	void Start () {
		churchImg.SetActive(false);
		triggerCube.SetActive(false);
	}
	
	// Update is called once per frame
	public void turnOn(){
		churchImg.SetActive(true);
		triggerCube.SetActive(true);
	}
}
