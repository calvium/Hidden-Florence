using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRenderTextureSize : MonoBehaviour {

	public Camera cam;
	public Material mat;
	public string texture;

	// Use this for initialization
	void Start () {
		if ( cam.targetTexture != null ) {
         	cam.targetTexture.Release( );
     	}
     	cam.targetTexture = new RenderTexture( Screen.width, Screen.height, 24 );
		mat.SetTexture(texture, cam.targetTexture);
	}
	
	void Update () {
		
	}
}
