using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.iOS;


public class SceneManager_TapToPlace : MonoBehaviour
{
    public enum TapToPlace_State { SCANNING, PLACING, GETTING_READY, EXPERIENCING };
    private TapToPlace_State state = TapToPlace_State.SCANNING;
    private ExperienceType selectedExperience;

    [Header("Church")]
    public GameObject churchContainer;
    public Animator churchAnimator;

    [Header("Placeholder Altar")]
    public Transform altarPiece;
    public float altarPieceOffsetHeight;
    public GameObject altarBase_Florence;
    public GameObject altarBase_Elsewhere;

    [Header("ScannerEffect")]
    public Transform scannerEffectOrigin;
    [SerializeField] private ScannerEffectDemo scannerEffectScrip;
    [SerializeField] private Material paintingMat;
    [SerializeField] private float paintingNum = 1.2f;

    [Header("UI Objects")]
    public Text alertText;
    public Text instructionsText;
    public CanvasGroup alertCanvas;
    public CanvasGroup instructionsCanvas;
    public CanvasGroup scanGifCanvas;
    public CanvasGroup helpCanvas;

    [Header("UI Strings - Florence")]
    public string scanningAlert_Florence;
    public string scanningInstruction_Florence;
    public string placingAlert_Florence;
    public string placingInstruction_Florence;
    public string gettingReadyAlert_Florence;
    public string gettingReadyInstruction_Florence;

    [Header("UI Strings - Elsewhere")]
    public string scanningAlert_Elsewhere;
    public string scanningInstruction_Elsewhere;
    public string placingAlert_Elsewhere;
    public string placingInstruction_Elsewhere;
    public string gettingReadyAlert_Elsewhere;
    public string gettingReadyInstruction_Elsewhere;

    [Header("AR Object")]
    public GameObject focusSquare;
    public GameObject focusSquareFocused;
    [SerializeField] public UnityARGeneratePlane generatePlaneScrip;


    private void Start()
    {
        alertCanvas.alpha = instructionsCanvas.alpha = scanGifCanvas.alpha = helpCanvas.alpha = 0;
        selectedExperience = AppManager.Instance.SelectedExperience;

        setExperienceState(TapToPlace_State.SCANNING);
    }

    private void Update()
    {
        // Update scanner effect material variable
        paintingMat.SetFloat("_Level", paintingNum);

        if (Input.GetMouseButtonDown(0) && !isPointerOverUIObject())
        {
            if (state == TapToPlace_State.PLACING)
            {
                setExperienceState(TapToPlace_State.GETTING_READY);
            } else if (state == TapToPlace_State.GETTING_READY)
            {
                setExperienceState(TapToPlace_State.EXPERIENCING);
            }
        } else if (state == TapToPlace_State.SCANNING && generatePlaneScrip.hasGeneratedPlanes())
        {
            setExperienceState(TapToPlace_State.PLACING);
        }

        // Update scanner effect origin position
        if (selectedExperience == ExperienceType.ELSEWHERE && scannerEffectOrigin.position != altarPiece.position)
        {
            scannerEffectOrigin.position = altarPiece.position;
        }
    }

    private void setExperienceState(TapToPlace_State newState)
    {
        Text alert = alertText.GetComponent<UnityEngine.UI.Text>();
        Text instructions = instructionsText.GetComponent<UnityEngine.UI.Text>();
        bool isFlorence = selectedExperience == ExperienceType.FLORENCE;
        switch (newState)
        {
            case TapToPlace_State.SCANNING:
				this.state = newState;
                alert.text = isFlorence ? scanningAlert_Florence : scanningAlert_Elsewhere;
                instructions.text = isFlorence ? scanningInstruction_Florence : scanningInstruction_Elsewhere;
				StartCoroutine(fadeIn(alertCanvas, 1f));
                StartCoroutine(fadeIn(scanGifCanvas, 2f));
				StartCoroutine(fadeIn(instructionsCanvas, 2f));
                StartCoroutine(fadeOut(alertCanvas, 5f));
                altarBase_Florence.SetActive(false);
                altarBase_Elsewhere.SetActive(false);
                focusSquare.SetActive(true);
                break;

            case TapToPlace_State.PLACING:
				this.state = newState;
                alert.text = isFlorence ? placingAlert_Florence : placingAlert_Elsewhere;
                instructions.text = isFlorence ? placingInstruction_Florence : placingInstruction_Elsewhere;
                //scanGifCanvas.gameObject.SetActive(false);
                StartCoroutine(fadeOut(scanGifCanvas, 0f));
                StartCoroutine(fadeIn(alertCanvas, 0f));
                StartCoroutine(fadeOut(alertCanvas, 6f));
                break;

            case TapToPlace_State.GETTING_READY:
                if (!placeAltarPiece()) break;
                this.state = newState;
                alert.text = isFlorence ? gettingReadyAlert_Florence : gettingReadyAlert_Elsewhere;
                instructions.text = isFlorence ? gettingReadyInstruction_Florence : gettingReadyInstruction_Elsewhere; ;
                StartCoroutine(fadeOut(scanGifCanvas, 0f));
                StartCoroutine(fadeIn(alertCanvas, 0f));
                StartCoroutine(fadeOut(alertCanvas, 6f));
                if (selectedExperience == ExperienceType.FLORENCE)
                {
                    altarBase_Florence.SetActive(true);
                } else
                {
                    altarBase_Elsewhere.SetActive(true);
                }
                focusSquare.SetActive(false);
                break;
            case TapToPlace_State.EXPERIENCING:
				StartCoroutine(fadeOut(instructionsCanvas, 0f));
				this.state = newState;
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
            Camera.main.transform.eulerAngles.y,
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
        StartCoroutine(fadeIn(helpCanvas, 0));
    }
    public void handleCloseHelpButtonPress()
    {
        StartCoroutine(fadeOut(helpCanvas, 0));
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
    IEnumerator fadeIn(CanvasGroup c, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        float temp = c.alpha;
        c.gameObject.SetActive(true);
        while (temp < 1)
        {
            temp += Time.deltaTime * 2;
            c.alpha = temp;
            yield return null;
        }
    }
    IEnumerator fadeOut(CanvasGroup c, float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        float temp = c.alpha;
        while (temp > 0)
        {
            temp -= Time.deltaTime * 2;
            c.alpha = temp;
            yield return null;
        }
        c.gameObject.SetActive(false);
    }
}
