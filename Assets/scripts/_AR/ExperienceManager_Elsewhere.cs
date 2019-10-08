using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

enum ExperienceState { SEARCHING_PLANES, ANCHORING, READY_TO_START, EXPERIENCING};

public class ExperienceManager_Elsewhere : MonoBehaviour
{
    [Header("Church")]
    public GameObject churchContainer;
    public Animator churchAnimator;

    [Header("Placeholder Altar")]
    public Transform altarPiece;
    public float altarPieceOffsetHeight;
    public GameObject altarBase;

    [Header("ScannerEffect")]
    public Transform scannerEffectOrigin;
    [SerializeField] private ScannerEffectDemo scannerEffectScrip;
    [SerializeField] private Material paintingMat;
    [SerializeField] private float paintingNum = 1.2f;

    [Header("UI Objects")]
    public GameObject placeButton;
    public GameObject startButton;

    [Header("AR Helpers")]
    public GameObject focusSquare;
    public GameObject focusSquareFocused;
    [SerializeField] public UnityARGeneratePlane generatePlaneScrip;

    private ExperienceState experienceState = ExperienceState.SEARCHING_PLANES;

    private void Start()
    {
        setExperienceState(ExperienceState.SEARCHING_PLANES);
    }

    private void Update()
    {
        // Update scanner effect material variable
        paintingMat.SetFloat("_Level", paintingNum);
        // Update scanner effect origin position
        scannerEffectOrigin.position = altarPiece.position;

        if (experienceState == ExperienceState.SEARCHING_PLANES && generatePlaneScrip.hasGeneratedPlanes())
        {
            setExperienceState(ExperienceState.ANCHORING);
        }
    }

    private void setExperienceState(ExperienceState state)
    {
        experienceState = state;
        switch (experienceState)
        {
            case ExperienceState.SEARCHING_PLANES:
                altarBase.SetActive(false);
                focusSquare.SetActive(false);
                placeButton.SetActive(false);
                startButton.SetActive(false);
                break;

            case ExperienceState.ANCHORING:
                focusSquare.SetActive(true);
                placeButton.SetActive(true);
                break;

            case ExperienceState.READY_TO_START:
                altarBase.SetActive(true);
                startButton.SetActive(true);
                break;
            case ExperienceState.EXPERIENCING:
                focusSquare.SetActive(false);
                break;
        }
    }

    public void placeAltarPiece()
    {
        setExperienceState(ExperienceState.READY_TO_START);

        // Move church to focusSquare position
        churchContainer.transform.position = new Vector3(
            focusSquareFocused.transform.position.x,
            focusSquareFocused.transform.position.y + altarPieceOffsetHeight,
            focusSquareFocused.transform.position.z);
        // Rotate church to face Camera
        churchContainer.transform.eulerAngles = new Vector3(
            churchContainer.transform.eulerAngles.x,
            Camera.main.transform.eulerAngles.y, //camera.transform.eulerAngles.y,
            churchContainer.transform.eulerAngles.z);


        // IDK ??
        if (paintingNum <= -0.2)
        {
            Debug.Log("debugging -- error starting showPainting because paintingNum <= -0.2");
        }

        StartCoroutine(showPainting());
    }

    public void startExperience()
    {
        setExperienceState(ExperienceState.EXPERIENCING);

        Debug.Log("debugging --- StartCoroutine(paint());");
        StartCoroutine(paint());
    }

    IEnumerator showPainting()
    {
        Debug.Log("showingPainting");
        while (paintingNum > -0.2f)
        {
            paintingNum -= Time.deltaTime * 1.5f;
            yield return null;
        }
    }

    IEnumerator paint()
    {
        Debug.Log("debugging --- go");
        scannerEffectScrip.startPainting();
        yield return new WaitForSeconds(0.1f);
        churchAnimator.SetTrigger("go");
        // yield return new WaitForSeconds(0.1f);
        // ScannerOrigin.position = altarpiece.position;
        scannerEffectScrip.startPainting();
        Debug.Log("debugging --- go end");
    }
}
