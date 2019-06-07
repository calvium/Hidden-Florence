using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class rotate : MonoBehaviour {

// Use this for initialization
[SerializeField] private Vector3 firstpoint;//change type on Vector3
[SerializeField] private Vector3 secondpoint;
[SerializeField] private float xAngle = 0.0f;
[SerializeField] private float yAngle = 0.0f;
[SerializeField] private float xAngTemp = 0.0f;
[SerializeField] private float yAngTemp = 0.0f;
// [SerializeField] private float xAngFling = 0.0f;
// [SerializeField] private float yAngFling = 0.0f;
[SerializeField] private GameObject camParent;
// [SerializeField] private GameObject camParent2;
[SerializeField] private float speedX = -1;
[SerializeField] private float speedY = -1;
[SerializeField] private float mouseDownTime;
public bool inPanning = false;
public bool mouseDown = false;
[SerializeField] private float clickSpeed = 0.1f;
[SerializeField] private bool flinging;
// [SerializeField] private float flingTime;
// [SerializeField] private float distanceX;
// [SerializeField] private float distanceY;
// [SerializeField] private float endDistanceX;
// [SerializeField] private float endDistanceY;
// [SerializeField] private float velocityX;
// [SerializeField] private float velocityY;
// [SerializeField] private float deceleration;
// public Rigidbody rBody;
// public Rigidbody rBody2;
// public float lerpTime = 1f;
// public float currentLerpTime;
public bool canMove;

// public Vector2 differenceX;
public Vector2 differenceY;
public List<Vector3> currentPoint;

public float amount = 100f;

public float clampRotation;

void Start() {
   	xAngle = 0.0f;
   	yAngle = 0.0f;
	// camParent.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
	// camParent2.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
	canMove = true;
}
void Update() {

	// Debug.Log(Input.GetAxis("Mouse X"));

	//**********************/
	//********Panning*******/
	//**********************/
	if(Input.GetMouseButtonDown (0) && canMove && (Input.touchCount == 2)) {
    	firstpoint = Input.mousePosition;
     	xAngTemp = camParent.transform.eulerAngles.y/speedX;
     	yAngTemp = yAngle;
		// xAngTemp = camParent2.transform.eulerAngles.x;
		// yAngTemp = camParent.transform.eulerAngles.y;
		// Debug.Log(yAngTemp + "---" + xAngTemp);

		inPanning = true;
		
		mouseDown = true;
		flinging = false;
		StartCoroutine(recordPos());
    }

	if(mouseDown){
		mouseDownTime += Time.deltaTime;
	}

    if(inPanning) {
     	secondpoint = Input.mousePosition;
		//  if( Vector2.Distance(firstpoint,secondpoint) > 100){
     		//Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
     		xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
     		yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
			
			// xAngle = (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
     		// yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;
     		//Rotate camera
			camParent.transform.localRotation = Quaternion.Euler(90f, (xAngle*speedX), 0.0f);

			clampRotation = yAngle*speedY;
			clampRotation = Mathf.Clamp(clampRotation, -90, 90);
			// camParent2.transform.localRotation = Quaternion.Euler(clampRotation, 0.0f, 0.0f);
			// camParent2.transform.eulerAngles.y = Mathf.Clamp(camParent2.transform.eulerAngles.y, 0.0f, 0.0f);

			// rBody.MoveRotation(rBody.rotation * (Quaternion.Euler(0.0f, (xAngle*speedX), 0.0f)));
		//  }
    }

	if(Input.GetMouseButtonUp (0)){
		if(inPanning){
		// if(mouseDownTime<clickSpeed){
		// 	Debug.Log("clicked");
		// }
		inPanning = false;
		mouseDown = false;
		flinging = true;
		// xAngFling = xAngle;
		// yAngFling = yAngle;
		// if(xAngle > 0){
		// 	StartCoroutine(flingXPos());
		// }
		// if(xAngle < 0){
		// 	StartCoroutine(flingXNeg());
		// }
		// distanceX = secondpoint.x - firstpoint.x;
		// distanceY = secondpoint.y - firstpoint.y;
		// velocityX = distanceX/mouseDownTime;
		// velocityY = distanceY/mouseDownTime;
		// currentLerpTime = 0f;
		// StartCoroutine(flingX());
		// differenceX = new Vector2(firstpoint.y - secondpoint.y, 0);

		differenceY = new Vector2(0, currentPoint[0].x - currentPoint[currentPoint.Count-1].x); 
		}
	}


	//**********************/
	//********Tapping*******/
	//**********************/
		if (Input.GetMouseButtonDown (0)) {    
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {
                 // whatever tag you are looking for on your game object  
					// Debug.Log(hit.transform.gameObject.name);
					if(IsPointerOverUIObject()){
						Debug.Log("over ui");
						return;
					}				
                if((hit.transform.gameObject.name == "Item")){  
                //   Debug.Log("hit " + hit.transform.gameObject.name);
                }

            }
  
        }

		if(flinging){
			// float tempX = (velocityX/deceleration)/2;
			// endDistanceX = distanceX + tempX; 
			// camParent.transform.rotation = Quaternion.Euler((yAngFling*speed), (xAngFling*speed), 0.0f);
			// camParent.transform.rotation = Quaternion.Euler((xAngFling), (xAngFling), 0.0f);

			// xAngFling += Time.deltaTime*10;
        	// if (xAngFling > lerpTime) {
        	//     xAngFling = lerpTime;
        	// }
 
        	// //lerp!
        	// float perc = currentLerpTime / lerpTime;
        	// xAngFling = Mathf.Lerp(xAngle, 0, perc);
			// StartCoroutine(flingX());
			// if(xAngFling < 0.2 && xAngFling > 0.2){
			// 	flinging = false;
			// }
			// speed = xAngFling;
			// float h = 1 * -amount * Time.deltaTime
        	// float v = Input.GetAxis("Mouse Y") * -amount * Time.deltaTime;
			if(differenceY.y > 250 || differenceY.y < -250){
        		// rBody.AddTorque(differenceY * amount);
			}
			// rBody2.AddTorque(differenceX);
			flinging = false;
		}

  	}
	// IEnumerator flingX(){
	// 	float tempX = (velocityX/deceleration)/2;
	// 	endDistanceX = xAngFling + tempX; 
	// 	while(true){
	// 		while(xAngFling > endDistanceX){
	// 			xAngFling -= Time.deltaTime/10;
	// 			camParent.transform.rotation = Quaternion.Euler((xAngFling), (xAngFling), 0.0f);
	// 		}


	// 	}
	// }
	IEnumerator recordPos(){
		currentPoint.Clear();
		while(inPanning){
			currentPoint.Add(Input.mousePosition);
			if(currentPoint.Count > 2){
				currentPoint.RemoveAt(0);
			}
			yield return new WaitForSeconds(0.05f);
		}
	}



  	/*~~~~~~~~~~ See if mouse is over UI ~~~~~~~~~~*/
	private bool IsPointerOverUIObject(){
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
    }

	public void allowMoving(){
		canMove = true;
	}
	public void disallowMoving(){
		canMove = false;
	}
}
