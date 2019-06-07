using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resizingUI : MonoBehaviour {

	[SerializeField] private GameObject churchImg;

	// Use this for initialization
	void Start(){
		Screen.orientation = ScreenOrientation.LandscapeLeft;

        float tempHeight = Screen.height;
        float tempWidth = Screen.width;
		float tempRatio = tempWidth/tempHeight;
		Debug.Log( "--- height: " + Screen.height + " --- width: " + Screen.width + " --- temp measurements " +tempHeight +"---" + tempWidth + "---" + tempRatio);

		if(tempRatio < 1.5){
			Debug.Log("this is an iPad.");
			// churchImg.transform.localScale = new Vector3(1075f, 1075f, 1075f);
			
		}else{

			if(tempRatio > 2){
				Debug.Log("this is an iPhone X");

			} else {
				Debug.Log("this is an iPhone");

			}
		}
    }	
}
