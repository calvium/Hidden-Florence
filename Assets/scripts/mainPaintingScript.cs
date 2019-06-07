using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainPaintingScript : MonoBehaviour {
	[SerializeField] private Material mat;
	[SerializeField] private float speed;

	public float temp;


	void Start () {
		mat.SetFloat("_Level", 1.2f);
	}
	public void startFade(){
		mat.SetFloat("_Level", 1.2f);
		Debug.Log("start fade");
		StartCoroutine(fade());
	}

	IEnumerator fade(){
		// mat.SetFloat("_Level", 1.2f);
		temp = 1.2f;
		while(temp > -0.3f){
			temp -= Time.deltaTime*speed;
			mat.SetFloat("_Level", temp);
			yield return null;
		}
	}
}
