using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceCalculator : MonoBehaviour {

	[SerializeField] private Camera cam;
	[SerializeField] private Transform[] points;
	[SerializeField] private List<float> distances;
	public bool far;
	public float dist;
	public Transform closestPoint;

	public ScannerEffectDemo scanScript;
	[SerializeField] private bool scanning = false;
	// [SerializeField] private churchScript church;

	private bool isEven(float i)  
    {  
        return(i>dist); 
    }  

	void Start(){
		cam = Camera.main;
		// scanScript = cam.GetComponent<ScannerEffectDemo>();
		// church = GameObject.FindGameObjectWithTag("church").GetComponent<churchScript>();

		points = GetComponentsInChildren<Transform>();
		foreach(Transform p in points){
			distances.Add(0);
		}
	}

	void Update(){
		float smallestDist = 100;
		for(int q=0; q<points.Length; q++){
			distances[q] = Vector3.Distance(cam.transform.position, points[q].position);
			if(distances[q]< smallestDist){
				smallestDist = distances[q];
				closestPoint = points[q];
			}
		}
		far = (distances.TrueForAll(isEven));
		if(far){
			scanScript._unscanning = true;
			scanScript.ScanDistance = 10-(smallestDist*3);
		}else{
			scanScript._unscanning = false;
		}
	}
}