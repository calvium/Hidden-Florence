using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshMoveSphere : MonoBehaviour {
	[SerializeField] private GameObject sphere;
	// [SerializeField] private cubeVariables cubeVariablesScript;
	// void Start () {
	// 	sphere = GameObject.FindGameObjectWithTag("sphere");
	// 	cubeVariablesScript = sphere.GetComponent<cubeVariables>();
	// 	if(cubeVariablesScript.canMove){
	// 		sphere.transform.parent = transform;
	// 		//sphere.transform.localPosition = new Vector3(0,0,0);
	// 		cubeVariablesScript.canMove = false;
	// 		cubeVariablesScript.uiBox.SetActive(true);
	// 		cubeVariablesScript.button.SetActive(true);
	// 		cubeVariablesScript.aScript.yAxis = this.transform.position.y;
	// 		cubeVariablesScript.aScript.setPos();
	// 	}
	// }
	// void Update () {
	// }
}
