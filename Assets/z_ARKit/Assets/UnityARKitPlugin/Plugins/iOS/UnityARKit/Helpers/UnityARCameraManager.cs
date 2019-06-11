using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class UnityARCameraManager : MonoBehaviour {

    public Camera m_camera;
    private UnityARSessionNativeInterface m_session;
    private Material savedClearMaterial;

    [Header("AR Config Options")]
    public UnityARAlignment startAlignment = UnityARAlignment.UnityARAlignmentGravity;
    public UnityARPlaneDetection planeDetection = UnityARPlaneDetection.Horizontal;
    public bool getPointCloud = true;
    public bool enableLightEstimation = true;
    public bool enableAutoFocus = true;
	public UnityAREnvironmentTexturing environmentTexturing = UnityAREnvironmentTexturing.UnityAREnvironmentTexturingNone;

    [Header("Image Tracking")]
    public ARReferenceImagesSet detectionImages = null;
    public int maximumNumberOfTrackedImages = 0;

    [Header("Object Tracking")]
    public ARReferenceObjectsSetAsset detectionObjects = null;
    private bool sessionStarted = false;

    [Header("AR Tracking")]
    public bool setUpFinished;
    public bool seenFirstImage;
    public bool trackingReady;
    public bool foundHorizontal;
    public bool foundVertical;
    public bool camStarted;
    public string arTrackingStatus;
    [SerializeField] private IMStartMenu menu;

    public ARKitWorldTrackingSessionConfiguration sessionConfiguration
    {
        get
        {
            ARKitWorldTrackingSessionConfiguration config = new ARKitWorldTrackingSessionConfiguration ();
            config.planeDetection = planeDetection;
            config.alignment = startAlignment;
            config.getPointCloudData = getPointCloud;
            config.enableLightEstimation = enableLightEstimation;
            config.enableAutoFocus = enableAutoFocus;
            config.maximumNumberOfTrackedImages = maximumNumberOfTrackedImages;
            config.environmentTexturing = environmentTexturing;
            if (detectionImages != null && setUpFinished){
                config.referenceImagesGroupName = detectionImages.resourceGroupName;
            }

			if (detectionObjects != null) 
			{
				config.referenceObjectsGroupName = "";  //lets not read from XCode asset catalog right now
				config.dynamicReferenceObjectsPtr = m_session.CreateNativeReferenceObjectsSet(detectionObjects.LoadReferenceObjectsInSet());
			}

            return config;
        }
    }

    // Use this for initialization
    void Start () {

        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

        Application.targetFrameRate = 30;
        
        var config = sessionConfiguration;
        if (config.IsSupported) {
            m_session.RunWithConfig (config);
            UnityARSessionNativeInterface.ARFrameUpdatedEvent += FirstFrameUpdate;
        }

        if (m_camera == null) {
            m_camera = Camera.main;
        }
    }

    void OnDestroy()
    {
        m_session.Pause();
    }

    void FirstFrameUpdate(UnityARCamera cam)
    {
        sessionStarted = true;
        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= FirstFrameUpdate;
    }

    public void SetCamera(Camera newCamera)
    {
        if (m_camera != null) {
            UnityARVideo oldARVideo = m_camera.gameObject.GetComponent<UnityARVideo> ();
            if (oldARVideo != null) {
                savedClearMaterial = oldARVideo.m_ClearMaterial;
                Destroy (oldARVideo);
            }
        }
        SetupNewCamera (newCamera);
    }

    private void SetupNewCamera(Camera newCamera)
    {
        m_camera = newCamera;

        if (m_camera != null) {
            UnityARVideo unityARVideo = m_camera.gameObject.GetComponent<UnityARVideo> ();
            if (unityARVideo != null) {
                savedClearMaterial = unityARVideo.m_ClearMaterial;
                Destroy (unityARVideo);
            }
            unityARVideo = m_camera.gameObject.AddComponent<UnityARVideo> ();
            unityARVideo.m_ClearMaterial = savedClearMaterial;
        }
    }

    // Update is called once per frame

    void Update () {
        
        if (m_camera != null && sessionStarted)
        {
            // JUST WORKS!
            Matrix4x4 matrix = m_session.GetCameraPose();
            m_camera.transform.localPosition = UnityARMatrixOps.GetPosition(matrix);
            m_camera.transform.localRotation = UnityARMatrixOps.GetRotation (matrix);

            m_camera.projectionMatrix = m_session.GetCameraProjection ();
        }

    }

    /***********Tracking State **************/

    void UnityARSessionNativeInterface_ARFrameUpdatedEvent (UnityARCamera camera)
	{   
        Debug.Log("00-debug-00 --- UnityARCamManger - 007");
		//1. Track The ARSession
        // if(!accessibilityMode){
        if (camera.trackingState == ARTrackingState.ARTrackingStateLimited && setUpFinished) {
            // Debug.Log("debugging AR limited");
            logTrackingReason (camera.trackingReason);
        } else {
            logTrackingState (camera.trackingState);
            // Debug.Log("debugging AR not limited");
            // if(trackPopup.activeInHierarchy){
            //     StartCoroutine(closePopUp());
				
			// 	if (arWarningMessage.activeSelf)
			// 		arWarningMessage.SetActive (false);

			// 	if (movementSpeedWarning)
			// 		movementSpeedWarning = false;

			// 	if (flatSurfaceWarning)
			// 		flatSurfaceWarning = false;
            // }
        }
        logLighting (camera.lightData.arLightEstimate.ambientIntensity);
        // }
        Debug.Log("00-debug-00 --- UnityARCamManger - 008");
    }
	public void callStartUp(int num){
        StartCoroutine(startUp(num));
    }

    IEnumerator startUp(int num) {
        Debug.Log("debugging startup" + num);
        Debug.Log("00-debug-00 --- UnityARCamManger - 009");
        yield return new WaitForSeconds(1);
        if(num == 0){
            Debug.Log("debugging tracking is true");
            planeDetection = UnityARPlaneDetection.Horizontal;
            Debug.Log("debugging planes are horizontal");
        } else if (num == 1){
            menu.callSetText(2);
            foundHorizontal = true;
            planeDetection = UnityARPlaneDetection.Vertical;
            Debug.Log("debugging planes are vertical");
        } else if (num == 2){
            menu.callSetText(3);
            foundVertical = setUpFinished = true;
            planeDetection = UnityARPlaneDetection.HorizontalAndVertical;
            Debug.Log("debugging see first painting");
        } else if (num == 3){
            seenFirstImage = true;
            menu.callSetText(4);
            Debug.Log("debugging see second painting");
        }
        var config = sessionConfiguration;
        m_session.RunWithConfig (config);
    }

    public void SimulateLowLighting()
    {
        logLighting(0);
    }

	void logLighting (float lightEstimate)
	{
		// if (lightEstimate < 100)
		// {
		// 	if (arWarningMessage == null)
		// 	{
		// 		arWarningMessage = GameObject.Find ("ParentMain(Clone)/Canvas/ARWarningMessage");
		// 	}

		// 	if (arWarningMessage != null)
		// 	{
		// 		if (!arWarningMessage.activeSelf) 
		// 		{
		// 			arWarningMessage.SetActive (true);
		// 			arWarningMessage.transform.GetChild (1).GetComponent<Text> ().text = "Lighting Is Too Dark. You may have to return to the poster to continue";
		// 		}

		// 	}
		// }
		// else if (movementSpeedWarning)
		// {
		// 	if (arWarningMessage == null)
		// 	{
		// 		arWarningMessage = GameObject.Find ("ParentMain(Clone)/Canvas/ARWarningMessage");
		// 	}

		// 	if (arWarningMessage != null)
		// 	{
		// 		if (!arWarningMessage.activeSelf) 
		// 		{
		// 			arWarningMessage.SetActive (true);
		// 			arWarningMessage.transform.GetChild (1).GetComponent<Text> ().text = "Please Slow Your Movement";
		// 		}

		// 	}
		// }
		// else if (flatSurfaceWarning)
		// {
		// 	if (arWarningMessage == null)
		// 	{
		// 		arWarningMessage = GameObject.Find ("ParentMain(Clone)/Canvas/ARWarningMessage");
		// 	}

		// 	if (arWarningMessage != null)
		// 	{
		// 		if (!arWarningMessage.activeSelf) 
		// 		{
		// 			arWarningMessage.SetActive (true);
		// 			arWarningMessage.transform.GetChild (1).GetComponent<Text> ().text = "Try To Point At A Flat Surface";
		// 		}

		// 	}
		// }
		// else
		// {
			// if (arWarningMessage.activeSelf)
			// 	arWarningMessage.SetActive (false);

			// if (movementSpeedWarning)
			// 	movementSpeedWarning = false;

			// if (flatSurfaceWarning)
			// 	flatSurfaceWarning = false;
		// }

		/*
		if(lightEstText!=null)
		{
			if (lightEstimate < 100)
			{
				if (lightMessageReady)
				{   
                    Debug.Log("debugging lightmessageready");
					if (messageSystem != null)
					{
                        Debug.Log("debugging light message");
						messageSystem.AddMessage("Lighting Is Too Dark. You may have to return to the poster to continue", true);
                    }
                    
					lightMessageReady = false;
				}

				arTrackingStatus = "Lighting Is Too Dark";
                // trackPopupText.text = "Lighting Is Too Dark.\n\nYou may have to return to the poster to continue";
                trackIcon.sprite = icons[0];
                openPopUp();
			}
			else
			{
				arWarnings = null;
			}
		}
		*/
	}
	public void logTrackingState (ARTrackingState trackingState)
    {
        // Debug.Log("debugging logTrackingState");
        switch (trackingState) {
        case ARTrackingState.ARTrackingStateNormal:
            if(!trackingReady && camStarted){
                trackingReady = true;
                Debug.Log("debugging tracking is ready01");
                StartCoroutine(startUp(0));
            }
            arTrackingStatus = "Tracking Ready";
            break;
        case ARTrackingState.ARTrackingStateNotAvailable:
            arTrackingStatus = "Tracking Unavailable";
            break;
        }
    }

    public void startCam(){
        camStarted = true;
    }

	public void logTrackingReason (ARTrackingStateReason reason)
    {

        switch (reason) {

		case ARTrackingStateReason.ARTrackingStateReasonExcessiveMotion:
			// movementSpeedWarning = true;

			/*
			if (movementMessageReady)
			{
				if (messageSystem != null)
					messageSystem.AddMessage("Please Slow your Movement", true);

				movementMessageReady = false;

			}
			*/

            arTrackingStatus = "Please Slow Your Movement";
            // // trackPopupText.text = "Please Slow Your Movement";
            // trackIcon.sprite = icons[1];
            // openPopUp();
            break;

		case ARTrackingStateReason.ARTrackingStateReasonInsufficientFeatures:
			// flatSurfaceWarning = true;

			/*
			if (flatSurfaceMessageReady)
			{
				if (messageSystem != null)
					messageSystem.AddMessage("Try To Point At A Flat Surface", true);

				flatSurfaceMessageReady = false;
			}
			*/

            arTrackingStatus = "Try To Point At A Flat Surface";
            // trackPopupText.text = "Try To Point At A Flat Surface";
            // trackIcon.sprite = icons[2];
            // openPopUp();
            break;

        case ARTrackingStateReason.ARTrackingStateReasonInitializing:
            arTrackingStatus = "Initializing";
            break;

		default:
			// movementSpeedWarning = false;
			// flatSurfaceWarning = false;
			break;
        }
    }

}
