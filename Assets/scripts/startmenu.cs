using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startmenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onSite(){
		// Debug.Log("tapped nonAR");
		SceneManager.LoadScene("imageAnchor");
	}

	public void atHome(){
		// Debug.Log("tapped AR");
		SceneManager.LoadScene("tapToPlace");
	}

	// 	public void moveBackToStart(){
	// 	// Debug.Log("tapped bva");
	// 	SceneManager.LoadScene("start");
	// }
}
