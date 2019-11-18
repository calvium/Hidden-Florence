using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundaryBox : MonoBehaviour {
	[SerializeField] private IMStartMenu menu;

	// Use this for initialization
	
	// Update is called once per frame
	void OnTriggerEnter(Collider col) {
		// menu.callStop();
    }
}
