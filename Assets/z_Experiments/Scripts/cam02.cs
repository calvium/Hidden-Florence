using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam02 : MonoBehaviour {
	public RenderTexture _tex;

	// Use this for initialization
	public void OnPostRender()
    {
		_tex.DiscardContents();
	}
}
