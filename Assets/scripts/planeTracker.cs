using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class planeTracker : MonoBehaviour {
    [SerializeField] private IMStartMenu menuScript;
    [SerializeField] private debugLogTextScript dbScript;
    [SerializeField] private Material HCol;
    [SerializeField] private Material VCol;
    [SerializeField] private LineRenderer line;
    
	void Start () {
        menuScript = GameObject.FindGameObjectWithTag("UIcanvas").GetComponent<IMStartMenu>();
        // dbScript = GameObject.Find("debugText").GetComponent<debugLogTextScript>();
        if(!menuScript.foundPlanes){
            menuScript.callSetText(2);
        }

        // if(this.transform.eulerAngles.x < 45 || this.transform.eulerAngles.x > 315 ){
        //     line.material = HCol;
        //     dbScript.addToString("h " + this.transform.eulerAngles);
        //     if(!menuScript.foundH && !menuScript.foundV){
        //         menuScript.callSetText(2);
        //         Debug.Log("debugging PLANE TRACKER calling start up 0");
        //     }
        // }else{
        //     line.material = VCol;
        //     dbScript.addToString("v " + this.transform.eulerAngles);
        //     if(menuScript.foundH && !menuScript.foundV){
        //         menuScript.callSetText(3);
        //         Debug.Log("debugging PLANE TRACKER calling start up 1");
        //     }
        // }


        // if(!menuScript.foundH && !menuScript.foundV){
        //     if(this.transform.eulerAngles.x < 45 || this.transform.eulerAngles.x > 315 ){
        //         line.material = HCol;
        //         dbScript.addToString("h " + this.transform.eulerAngles);
        //         menuScript.callSetText(2);
        //         Debug.Log("debugging PLANE TRACKER calling start up 0");
        //     }
        // }else
        // if(menuScript.foundH && !menuScript.foundV){
        //     menuScript.callSetText(3);
        //     Debug.Log("debugging PLANE TRACKER calling start up 1");
        // }
	}
}
