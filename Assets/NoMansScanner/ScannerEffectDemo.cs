using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
	public bool mainCam;

	public string textureName;
	public Transform ScannerOrigin;
	public Material EffectMaterial;
	public float ScanDistance;
	public float speed = 25;
	// public Material extra;

	public Camera _cameraMain;

	public Image isitpainting;

	// Demo Code
	bool _scanning;
	// Scannable[] _scannables;

	public GenerateImageAnchor imageScript;

	void Start()
	{
		// _scannables = FindObjectsOfType<Scannable>();
    }

	void Update()
	{
		if (_scanning)
		{
			ScanDistance += Time.deltaTime * speed;
			// foreach (Scannable s in _scannables)
			// {
			// 	if (Vector3.Distance(ScannerOrigin.position, s.transform.position) <= ScanDistance)
			// 		s.Ping();
			// }
		}

		// if (Input.GetKeyDown(KeyCode.C) && mainCam)
		// {
		// 	_scanning = true;
		// 	ScanDistance = 0;
		// }

		if (Input.GetMouseButtonDown(0) && mainCam)
		{
			Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				_scanning = true;
				ScanDistance = 0;
				ScannerOrigin.position = hit.point;
			}
		}

		if(imageScript.paintingStarted){
			// ScannerOrigin.position = imageScript.point;
			isitpainting.color = Color.blue;
			}else{
			isitpainting.color = Color.red;
		}
	}

	public void startPainting(){
		Debug.Log("startPainting00");
			_scanning = true;
		Debug.Log("startPainting01");
			ScanDistance = 0;
				// ScannerOrigin.position = imageScript.point;
	}
	// End Demo Code

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
