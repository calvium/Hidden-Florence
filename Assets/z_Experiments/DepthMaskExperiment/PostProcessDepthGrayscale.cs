using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessDepthGrayscale : MonoBehaviour {

	public Material mat;
    public Camera cam;
	// Use this for initialization
	void Start () {
		cam.depthTextureMode = DepthTextureMode.Depth;
	}
	
   void OnRenderImage (RenderTexture source, RenderTexture destination){
      Graphics.Blit(source,destination,mat);
      //mat is the material which contains the shader
      //we are passing the destination RenderTexture to
   }
}
