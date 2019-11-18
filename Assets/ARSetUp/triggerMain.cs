using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class triggerMain : MonoBehaviour {


	[SerializeField] private ARReferenceImage referenceImage;
	private GameObject imageAnchorGO;
	[SerializeField] private UnityARCameraManager camScript;
	private bool seen = false;
	public Transform ScannerOrigin;
	public bool active = false;
	public GameObject church;
	[SerializeField] private IMStartMenu menu;
	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			church.transform.position = position;
			church.transform.rotation = rotation;
			ScannerOrigin.position = position;
		}
	}
	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			church.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
			church.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
			if(!seen){
				menu.callSetText(3);
				seen = true;
			}
		}
	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}
}
