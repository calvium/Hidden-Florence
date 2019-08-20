using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class infoCredits : MonoBehaviour {

	[SerializeField] private float speed;

	// Use this for initialization
	void Start () {
		
	}
	void Update () {
		
	}

	IEnumerator fadeIn(CanvasGroup c){
		float temp = c.alpha = 0;
		yield return new WaitForSeconds(0.5f);
		while(temp<1){
			temp += Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}
	IEnumerator fadeOut(CanvasGroup c){
		float temp = c.alpha = 1;
		yield return new WaitForSeconds(0.5f);
		while(temp>0){
			temp -= Time.deltaTime*speed;
			c.alpha=temp;
			yield return null;
		}
	}
}
