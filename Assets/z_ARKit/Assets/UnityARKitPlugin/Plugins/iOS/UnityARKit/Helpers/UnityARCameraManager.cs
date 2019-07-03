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
            if (detectionImages != null)
                config.referenceImagesGroupName = detectionImages.resourceGroupName;

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

        Application.targetFrameRate = 60;
        
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

    public void callStartUp(int num){
        Debug.Log("debugging startup" + num);
        Debug.Log("00-debug-00 --- UnityARCamManger - 009");
        // yield return new WaitForSeconds(0);
    
        if(num == 0){
            // Debug.Log("debugging tracking is true");
            // planeDetection = UnityARPlaneDetection.Horizontal;
            Debug.Log("debugging planes are horizontal");

            // GameObject tempconfig = GameObject.Find("ARKitWorldTrackingRemoteConnection");
            // Destroy(GameObject.Find("ARKitWorldTrackingRemoteConnection"), 0);

            // Debug.Log("THING SHOULD HAVE DELETED");

            // var config = sessionConfiguration;
            // m_session.RunWithConfig (config);
        } else if (num == 1){
            menu.callSetText(2);
            foundHorizontal = true;
            // planeDetection = UnityARPlaneDetection.Vertical;
            Debug.Log("debugging planes are vertical");

            // var config = sessionConfiguration;
            // m_session.RunWithConfig (config);
        } else if (num == 2){
            menu.callSetText(3);
            foundVertical = setUpFinished = true;
            // planeDetection = UnityARPlaneDetection.HorizontalAndVertical;
            Debug.Log("debugging see first painting");
        // } else if (num == 3){
        //     seenFirstImage = true;
        //     menu.callSetText(4);
        //     Debug.Log("debugging see second painting");
        }
        // var config = sessionConfiguration;
        // m_session.RunWithConfig (config);
        // StartCoroutine(startUp(num));
    }

}

