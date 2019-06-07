using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveChurchToPlane : MonoBehaviour {

	[SerializeField] private GameObject church;
	[SerializeField] private churchScript churchVarScript;
	[SerializeField] private bool canMove;
	[SerializeField] private Image ready;
	[SerializeField] private Material particleMat;
	// [SerializeField]private Color _particleStart;
	[SerializeField]private Color _particleSeen;
	void Start () {
		church = GameObject.FindGameObjectWithTag("church");
		churchVarScript = church.GetComponent<churchScript>();
		ready = GameObject.Find("readyToTap").GetComponent<Image>();
		ready.color = Color.green;
		if(churchVarScript.canMove){
			Debug.Log("movechurch");
			churchVarScript.child.SetActive(true);
			church.transform.parent = transform;
			church.transform.localPosition = new Vector3(0f,0,7f);
			// church.transform.localEulerAngles = new Vector3(-90,90,-90);
			churchVarScript.canMove = false;
			particleMat.SetColor("_TintColor", _particleSeen);
			Debug.Log("movechurch_");
		}
	}

	void Update () {
	}
}
