using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

enum ExperienceState { SCANNING, PLACING, GETTING_READY, EXPERIENCING};

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

    [Header("UI Settings")]
    public float alertSpeed;
    public float instructionsSpeed;

    [Header("UI Objects")]
    public Text alertText;
    public Text instructionsText;
    public CanvasGroup alertCanvas;
    public CanvasGroup instructionsCanvas;

    [Header("UI strings")]
    public string SCANNING_AlertText;
    public string SCANNING_InstructionsText;
    public string PLACING_AlertText;
    public string PLACING_InstructionsText;
    public string GETTING_READY_AlertText;
    public string GETTING_READY_InstructionsText;

    [Header("AR Object")]
    public GameObject focusSquare;
    public GameObject focusSquareFocused;
    [SerializeField] public UnityARGeneratePlane generatePlaneScrip;

    private ExperienceState experienceState = ExperienceState.SCANNING;

    private void Start()
    {
        alertCanvas.alpha = instructionsCanvas.alpha = 0;
        setExperienceState(ExperienceState.SCANNING);
    }

    private void Update()
    {
        // Update scanner effect material variable
        paintingMat.SetFloat("_Level", paintingNum);
        // Update scanner effect origin position
        scannerEffectOrigin.position = altarPiece.position;

        if (Input.GetMouseButtonDown(0) && !isPointerOverUIObject())
        {
            if (experienceState == ExperienceState.PLACING)
            {
                setExperienceState(ExperienceState.GETTING_READY);
            } else if (experienceState == ExperienceState.GETTING_READY)
            {
                setExperienceState(ExperienceState.EXPERIENCING);
            }
        } else if (experienceState == ExperienceState.SCANNING && generatePlaneScrip.hasGeneratedPlanes())
        {
            setExperienceState(ExperienceState.PLACING);
        }
    }

    private void setExperienceState(ExperienceState state)
    {
        Text alert = alertText.GetComponent<UnityEngine.UI.Text>();
        Text instructions = instructionsText.GetComponent<UnityEngine.UI.Text>();
        switch (state)
        {
            case ExperienceState.SCANNING:
				experienceState = state;
				alert.text = SCANNING_AlertText;
                instructions.text = SCANNING_InstructionsText;
				StartCoroutine(fadeIn(alertCanvas, alertSpeed, 3f));
				StartCoroutine(fadeIn(instructionsCanvas, instructionsSpeed, 4f));
				StartCoroutine(fadeOut(alertCanvas, alertSpeed, 6f));
				altarBase.SetActive(false);
                focusSquare.SetActive(true);
                break;

            case ExperienceState.PLACING:
				experienceState = state;
				alert.text = PLACING_AlertText;
                instructions.text = PLACING_InstructionsText;
                StartCoroutine(fadeIn(alertCanvas, alertSpeed, 0f));
                StartCoroutine(fadeOut(alertCanvas, alertSpeed, 6f));
                break;

            case ExperienceState.GETTING_READY:
                if (!placeAltarPiece()) break;
                experienceState = state;
                alert.text = GETTING_READY_AlertText;
                instructions.text = GETTING_READY_InstructionsText;
                StartCoroutine(fadeIn(alertCanvas, alertSpeed, 0f));
                StartCoroutine(fadeOut(alertCanvas, alertSpeed, 6f));
                altarBase.SetActive(true);
                focusSquare.SetActive(false);
                break;
            case ExperienceState.EXPERIENCING:
				StartCoroutine(fadeOut(instructionsCanvas, alertSpeed, 0f));
				experienceState = state;
				startExperience();
                break;
        }
    }

    private bool placeAltarPiece()
    {
        // TODO: Check if focus square is focus
        if (!focusSquareFocused.active == true)
		{
			return false;
		}

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

		return true;
    }

    private void startExperience()
    {
        StartCoroutine(startScannerEffect());
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

    IEnumerator startScannerEffect()
    {
        Debug.Log("debugging --- go");
        scannerEffectScrip.startPainting();
        yield return new WaitForSeconds(0.1f);
        churchAnimator.SetTrigger("go");
        scannerEffectScrip.startPainting();
        Debug.Log("debugging --- go end");
    }

    // UI
    private bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
    IEnumerator fadeIn(CanvasGroup c, float speed, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        float temp = c.alpha = 0;
        while (temp < 1)
        {
            temp += Time.deltaTime * speed;
            c.alpha = temp;
            yield return null;
        }
    }
    IEnumerator fadeOut(CanvasGroup c, float speed, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        float temp = c.alpha = 1;
        while (temp > 0)
        {
            temp -= Time.deltaTime * speed;
            c.alpha = temp;
            yield return null;
        }
    }
}
