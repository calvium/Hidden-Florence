using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fieldOfView : MonoBehaviour {
	public Camera camera;
	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "" + camera.fieldOfView;
	}

	public void up(){
		camera.fieldOfView += 0.1f;
	}

	public void down(){
		camera.fieldOfView -= 0.1f;
	}
}
