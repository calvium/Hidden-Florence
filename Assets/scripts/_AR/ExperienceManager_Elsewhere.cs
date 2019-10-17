using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;

enum ElsewhereExperience_State { SCANNING, PLACING, GETTING_READY, EXPERIENCING};

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
    public CanvasGroup scanGifCanvas;
    public CanvasGroup helpCanvas;

    [Header("UI Strings")]
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

    private ElsewhereExperience_State experienceState = ElsewhereExperience_State.SCANNING;

    private void Start()
    {
        alertCanvas.alpha = instructionsCanvas.alpha = scanGifCanvas.alpha = helpCanvas.alpha = 0;
        setExperienceState(ElsewhereExperience_State.SCANNING);
    }

    private void Update()
    {
        // Update scanner effect material variable
        paintingMat.SetFloat("_Level", paintingNum);
        // Update scanner effect origin position
        scannerEffectOrigin.position = altarPiece.position;

        if (Input.GetMouseButtonDown(0) && !isPointerOverUIObject())
        {
            if (experienceState == ElsewhereExperience_State.PLACING)
            {
                setExperienceState(ElsewhereExperience_State.GETTING_READY);
            } else if (experienceState == ElsewhereExperience_State.GETTING_READY)
            {
                setExperienceState(ElsewhereExperience_State.EXPERIENCING);
            }
        } else if (experienceState == ElsewhereExperience_State.SCANNING && generatePlaneScrip.hasGeneratedPlanes())
        {
            setExperienceState(ElsewhereExperience_State.PLACING);
        }
    }

    private void setExperienceState(ElsewhereExperience_State state)
    {
        Text alert = alertText.GetComponent<UnityEngine.UI.Text>();
        Text instructions = instructionsText.GetComponent<UnityEngine.UI.Text>();
        switch (state)
        {
            case ElsewhereExperience_State.SCANNING:
				experienceState = state;
				alert.text = SCANNING_AlertText;
                instructions.text = SCANNING_InstructionsText;
				StartCoroutine(fadeIn(alertCanvas, alertSpeed, 1f));
				StartCoroutine(fadeIn(instructionsCanvas, instructionsSpeed, 4f));
                StartCoroutine(fadeIn(scanGifCanvas, instructionsSpeed, 4f));
                StartCoroutine(fadeOut(alertCanvas, alertSpeed, 5f));
				altarBase.SetActive(false);
                focusSquare.SetActive(true);
                break;

            case ElsewhereExperience_State.PLACING:
				experienceState = state;
				alert.text = PLACING_AlertText;
                instructions.text = PLACING_InstructionsText;
                StartCoroutine(fadeOut(scanGifCanvas, instructionsSpeed, 0f));
                StartCoroutine(fadeIn(alertCanvas, alertSpeed, 0f));
                StartCoroutine(fadeOut(alertCanvas, alertSpeed, 6f));
                break;

            case ElsewhereExperience_State.GETTING_READY:
                if (!placeAltarPiece()) break;
                experienceState = state;
                alert.text = GETTING_READY_AlertText;
                instructions.text = GETTING_READY_InstructionsText;
                StartCoroutine(fadeIn(alertCanvas, alertSpeed, 0f));
                StartCoroutine(fadeOut(alertCanvas, alertSpeed, 6f));
                altarBase.SetActive(true);
                focusSquare.SetActive(false);
                break;
            case ElsewhereExperience_State.EXPERIENCING:
				StartCoroutine(fadeOut(instructionsCanvas, alertSpeed, 0f));
				experienceState = state;
				startExperience();
                break;
        }
    }

    private bool placeAltarPiece()
    {
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

    public void handleHelpButtonPress()
    {
        // TODO: Rework this bit
        if (helpCanvas.alpha == 0)
        {
            StartCoroutine(fadeIn(helpCanvas, alertSpeed, 0));
        } else
        {
            StartCoroutine(fadeOut(helpCanvas, alertSpeed, 0));
        }
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
        float temp = c.alpha;
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
        float temp = c.alpha;
        while (temp > 0)
        {
            temp -= Time.deltaTime * speed;
            c.alpha = temp;
            yield return null;
        }
    }
}
