using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textBoxes : MonoBehaviour {

	[SerializeField] private CanvasGroup startBackground;
	[SerializeField] private CanvasGroup box0;
	[SerializeField] private CanvasGroup box1;
	[SerializeField] private float speed;
	

	// Use this for initialization
	void Start () {
		box0.alpha=box1.alpha=0;
		startBackground.alpha=1;
		StartCoroutine(fadeOut(startBackground));
		StartCoroutine(fadeIn(box0));
	}
	public void one(){
		StartCoroutine(fadeOut(box0));
		StartCoroutine(fadeIn(box1));
	}
	public void two(){
		StartCoroutine(fadeOut(box1));
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
