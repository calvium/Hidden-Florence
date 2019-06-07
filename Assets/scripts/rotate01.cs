using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate01 : MonoBehaviour {

	// Use this for initialization
	   [SerializeField] private bool rotating;
    [SerializeField] private Vector2 startVector;
    [SerializeField] private float rotGestureWidth;
    [SerializeField] private float rotAngleMinimum;
     
    void Update () {
        if (Input.touchCount == 2) {
            if (!rotating) {
                startVector = Input.GetTouch(1).position - Input.GetTouch(0).position;
                rotating = startVector.sqrMagnitude > rotGestureWidth * rotGestureWidth;
            } else {
                Vector2 currVector = Input.GetTouch(1).position - Input.GetTouch(0).position;
                float angleOffset = Vector2.Angle(startVector, currVector);
                Vector3 LR = Vector3.Cross(startVector, currVector);
               
                if (angleOffset > rotAngleMinimum) {
                    if (LR.z > 0) {
                        // Anticlockwise turn equal to angleOffset.
                    } else if (LR.z < 0) {
                        // Clockwise turn equal to angleOffset.
                    }
                }
               
            }
           
        } else {
            rotating = false;
        }
    }

}
