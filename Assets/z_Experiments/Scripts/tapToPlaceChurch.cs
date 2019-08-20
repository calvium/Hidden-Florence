using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tapToPlaceChurch : MonoBehaviour {

	public GameObject church;
	public float createHeight;
	public float maxRayDistance = 30.0f;
	public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
	private MaterialPropertyBlock props;
	public Slider mainSlider;
	public float yRot;
	public float rotSpd;
	[SerializeField] private ScannerEffectDemo scanScript;
	[SerializeField] private Material paintingMat;
	[SerializeField] private float paintingNum=1.2f;
	public Transform altarpiece;
	public Transform ScannerOrigin;
	public Animator anim;

	public GameObject altarBase;


	// void Start(){
	// 	Debug.Log(this.gameObject.name);
	// }

	void Start(){
		altarBase.SetActive(false);
	}


	void Update () {
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) 
			{
				if(IsPointerOverUIObject()){
					Debug.Log("over ui");
					return;
				}
				church.transform.position = new Vector3(hit.point.x, hit.point.y + createHeight, hit.point.z);
				Debug.Log ("debugging " + string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", hit.point.x, hit.point.y, hit.point.z));
				altarBase.SetActive(true);
				if(paintingNum>-0.2){
						StartCoroutine(showPainting());
				}
			}
		}
		rotateChurch();
		paintingMat.SetFloat("_Level", paintingNum);
		ScannerOrigin.position = altarpiece.position;
	}

	IEnumerator showPainting(){
		Debug.Log("showingPainting");
		while(paintingNum>-0.2f){
			paintingNum -= Time.deltaTime*1.5f;
			yield return null;
		}
	}
    void rotateChurch()
    {
			yRot = (mainSlider.value*360)-180;
		church.transform.eulerAngles = new Vector3(church.transform.eulerAngles.x, yRot, church.transform.eulerAngles.z);
    }

		private bool IsPointerOverUIObject(){
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
			
        }
	public void startPainting(){
		Debug.Log("debugging --- StartCoroutine(paint());");
		StartCoroutine(paint());
	}
	IEnumerator paint(){
		Debug.Log("debugging --- go");
		scanScript.startPainting();
		yield return new WaitForSeconds(0.1f);
		anim.SetTrigger("go");
		// yield return new WaitForSeconds(0.1f);
		// ScannerOrigin.position = altarpiece.position;
		scanScript.startPainting();
		Debug.Log("debugging --- go end");
	}
}
