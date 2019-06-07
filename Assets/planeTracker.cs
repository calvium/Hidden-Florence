using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class planeTracker : MonoBehaviour {
	private UnityARCameraManager unityARCameraManager;

	// Use this for initialization
	void Start () {
		unityARCameraManager = GameObject.FindGameObjectWithTag("ARController").GetComponent<UnityARCameraManager>();
		if(unityARCameraManager.planeDetection == UnityARPlaneDetection.Horizontal){
            unityARCameraManager.callStartUp(1);
            Debug.Log("debugging calling start up 0");
        }
        if(unityARCameraManager.planeDetection == UnityARPlaneDetection.Vertical){
            unityARCameraManager.callStartUp(2);
            Debug.Log("debugging calling start up 1");
        }
	}
}
