﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
	public bool mainCam;

	public string textureName;
	public Transform ScannerOrigin;
	public Material EffectMaterial;
	public float ScanDistance;
	public float speed = 25;

	public Camera _cameraMain;

	public Image isitpainting;

	// Demo Code
	public bool _scanning;
	public bool _unscanning;
	[SerializeField] private bool tapToPlace;
	// Scannable[] _scannables;

	// public GenerateImageAnchor imageScript;

	[SerializeField] private GameObject cam02;

	[SerializeField] private mainPaintingScript fadeInPainting;

	[SerializeField] private Transform camTran;
	[SerializeField] private bool started = false;
	[SerializeField] private Material particleMat;
	[SerializeField] private Color _particlesStart;
	[SerializeField] private Color _particlesOff;
	[SerializeField] private debugLogTextScript dbScript;
	[SerializeField] private IMStartMenu menuScript;

	void Start()
	{
		// _scannables = FindObjectsOfType<Scannable>();
		particleMat.SetColor("_TintColor", _particlesStart);
    }

	void Update()
	{
		if (_scanning) {
			ScanDistance += Time.deltaTime * speed;
			speed += Time.deltaTime/5;
			EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		}

		// if (_unscanning && tapToPlace) {//
		if (_unscanning) {
			ScanDistance -= Time.deltaTime * speed;
			speed += Time.deltaTime/3;
			// ScanDistance -= Time.deltaTime * speed;
			// speed += Time.deltaTime/1f;
			EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		}

		if (Input.GetKeyDown(KeyCode.C) && mainCam){
			_scanning = true;
			ScanDistance = 0;
		}
	}

	public void startPainting(){
		ScanDistance = 0;
		EffectMaterial.SetInt("_rev", 0);
		Debug.Log("debugging --- startPainting00");
		_scanning = true;
		speed=0.25f;
		Debug.Log("debugging --- startPainting01");
		StartCoroutine(turnOffCam02());
	}

	public void reversePainting(){
		// EffectMaterial.SetInt("_rev", 1);
		Debug.Log("debugging --- reversePainting00");
		_unscanning = true;
		Debug.Log("debugging --- reversePainting01");
		speed = 0.25f;
		// ScanDistance = 0;
	}
	// End Demo Code

	IEnumerator turnOffCam02(){
		yield return new WaitForSeconds (30f);
		// EffectMaterial.SetInt("_rev", 1);
		_scanning = false;
		// ScanDistance = 100;
		// speed = 0;
		dbScript.addToString("camera off");
		// // cam02.SetActive(false);
		Debug.Log("debugging --- cam02 off");
		// menuScript.boundariesOn();
	}

	void OnEnable()
	{
		_cameraMain = GetComponent<Camera>();
		_cameraMain.depthTextureMode = DepthTextureMode.Depth;
	}

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
		EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		RaycastCornerBlit(src, dst, EffectMaterial);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _cameraMain.farClipPlane;
		float camFov = _cameraMain.fieldOfView;
		float camAspect = _cameraMain.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _cameraMain.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _cameraMain.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_cameraMain.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_cameraMain.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_cameraMain.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_cameraMain.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture(textureName, source);
		// extra.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
	
}
