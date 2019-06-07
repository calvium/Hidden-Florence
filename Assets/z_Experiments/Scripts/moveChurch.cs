using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveChurch : MonoBehaviour {
	[SerializeField] private GameObject church;

	// Use this for initialization
	void Start () {
		church = GameObject.FindGameObjectWithTag("church");
		// church.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(church != null){
			church.transform.position = this.transform.position;
		}
	}
}
