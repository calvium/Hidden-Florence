using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveChurchToCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("debugging church moved start");
		GameObject church = GameObject.FindGameObjectWithTag("church");
		church.transform.parent = this.transform;
		church.transform.localPosition = new Vector3(0,0,0);
		church.transform.localEulerAngles = new Vector3(0,0,0);
		Debug.Log("debugging church moved end");
	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
