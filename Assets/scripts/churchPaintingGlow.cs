using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class churchPaintingGlow : MonoBehaviour {
	[SerializeField] private Material mat;

	// Use this for initialization
	void Start () {
		// StartCoroutine(fadeIn());
	}
	
	IEnumerator fadeIn(){
		float temp = 0f;
		while(temp<1f){
			temp += Time.deltaTime*0.3f;
			mat.SetFloat("_Alpha", temp);
			yield return null;
		}
	}
}
