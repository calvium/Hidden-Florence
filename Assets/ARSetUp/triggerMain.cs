using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class triggerMain : MonoBehaviour {


	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	// private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

	// [SerializeField] private GameObject churchImg;

	// [SerializeField] private textBoxes tbs;
	[SerializeField] private debugLogTextScript dbScript;
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
		Debug.Log ("debugging image anchor added");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			dbScript.addToString("debugging main Instantiate prefab");
			// imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
			church.transform.position = position;
			church.transform.rotation = rotation;
			Debug.Log("debugging added new img");
			ScannerOrigin.position = position;
			// if(!seen){
			// 	camScript.callStartUp(3);
			// 	seen = true;
			// }
		}
	}

	// void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	// {
	// 	Debug.LogFormat("image anchor updated[{0}] : tracked => {1}", arImageAnchor.identifier, arImageAnchor.isTracked);
	// 	if (arImageAnchor.referenceImageName == referenceImage.imageName) {
    //         if (arImageAnchor.isTracked)
    //         {
    //             if (!imageAnchorGO.activeSelf)
    //             {
    //                 imageAnchorGO.SetActive(true);
	// 				dbScript.addToString("main image anchor");
    //             }
	// 			// ScannerOrigin.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
	// 			church.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
	// 			church.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
	// 			dbScript.addToString("objectMoved");
	// 			menu.callSetText(4);
    //             // imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
    //             // imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
	// 			// if(!seen){
	// 			// 	camScript.callStartUp(3);
	// 			// 	seen = true;
	// 			// }
    //         }
    //     }
	// }
		void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		// Debug.Log ("debugging image anchor updated");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
				// Debug.Log ("debugging moved church again");
				church.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
				church.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);
				if(!seen){
				menu.callSetText(4);
				seen = true;
				}
		}

	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		// Debug.LogFormat("image anchor removed[{0}] : tracked => {1}", arImageAnchor.identifier, arImageAnchor.isTracked);
		// if (imageAnchorGO) {
		// 	GameObject.Destroy (imageAnchorGO);
		// }
	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

	// Update is called once per frame
	void Update () {
		
	}

	// IEnumerator setUpPainting(){
	// 		dbScript.addToString("main set up painting");
	// 		// churchImg.SetActive(true);
	// 		yield return new WaitForSeconds(0.1f);
	// 		dbScript.addToString("main set up painting end");
	// }
}
