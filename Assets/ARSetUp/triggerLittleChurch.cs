using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class triggerLittleChurch : MonoBehaviour {
	[SerializeField]
	private ARReferenceImage referenceImage;
	[SerializeField]
	private GameObject prefabToGenerate;
	private GameObject imageAnchorGO;
	public bool paintingStarted;
	public Transform ScannerOrigin;
	public Camera cam;
	[SerializeField] private GameObject churchImg;
	public bool active = false;
	public bool hovering = false;
	[SerializeField] private Image dialChurch;
	[SerializeField] private GameObject dialChurchO;
	public float num;
	// [SerializeField] private textBoxes tbs;
	[SerializeField] private debugLogTextScript dbScript;
	private bool prefabAppeared = false;
	[SerializeField] private IMStartMenu menu;

	// public ScannerEffectDemo scanScript;
	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.LogFormat("image anchor added[{0}] : tracked => {1}", arImageAnchor.identifier, arImageAnchor.isTracked);
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			dbScript.addToString("church seen the painting 00");
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			// if(active){
				dbScript.addToString("church seen the painting 01");
				imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
				ScannerOrigin.position = position;
				
			// }
		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		dbScript.addToString("church update 00");
		Debug.LogFormat("image anchor updated[{0}] : tracked => {1}", arImageAnchor.identifier, arImageAnchor.isTracked);
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			dbScript.addToString("church update 01");
			
				dbScript.addToString("church update 02");
            if (arImageAnchor.isTracked)
            {	
				dbScript.addToString("church update 03");
                if (!imageAnchorGO.activeSelf)
                {
                    imageAnchorGO.SetActive(true);
                }

				if(active && !prefabAppeared){ 
					activateChurchTriggers temp = GameObject.FindWithTag("scanTrigger").GetComponent<activateChurchTriggers>();
					temp.turnOn();
					prefabAppeared = true;
				}
				// hovering = true;
				ScannerOrigin.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
                imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

				if(churchImg.activeInHierarchy && dialChurch.fillAmount == 0){
					dbScript.addToString("church is active and fill is 0");
					StartCoroutine(fillDial());
				}
				if(!paintingStarted){
					dbScript.addToString("church seen the painting 02");
					paintingStarted = true;
					dbScript.addToString("church seen the painting 03");
					}
				}
        }

	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.LogFormat("image anchor removed[{0}] : tracked => {1}", arImageAnchor.identifier, arImageAnchor.isTracked);
		if (imageAnchorGO) {
			GameObject.Destroy (imageAnchorGO);
		}

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

	// Update is called once per frame
	IEnumerator fillDial () {
		dbScript.addToString("church start fill dial");
		yield return new WaitForSeconds(0.5f);
		while(dialChurch.fillAmount < 1){
			dialChurch.fillAmount += (Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(0.5f);
		churchImg.SetActive(false);
		dialChurchO.SetActive(false);
		// dialChurch.fillAmount=0;
		menu.callSetText(5);
	}
}
