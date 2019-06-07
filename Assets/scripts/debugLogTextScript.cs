using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugLogTextScript : MonoBehaviour {

	[SerializeField] private string log = "";
	[SerializeField] private Text textbox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		textbox.text = log;
	}

	public void addToString(string txt){
		string temp = log;
		log = txt + "\n" + temp;
	}
}
