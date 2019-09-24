using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class cameraTracking : MonoBehaviour
{
    public Transform[] linePoints;
	private Vector3[] vertices = new Vector3[0];
    public float lineDrawSpeed, acceleration;
    public Transform cam;
    public Vector3 temp;
    void Start()
    {
        GetLinePoints();
        StartCoroutine(Draw());
    }

    private IEnumerator Draw() {
        cam.parent = this.transform;
        cam.localPosition = new Vector3(0,0,0);
        yield return new WaitForSeconds(2f);
        int index = 1;
		int vertexPos = 1;
        // Vector3 temp = vertices[0];
        temp = vertices[0];
        Vector3 target = vertices[index];
		while (true){
            temp = Vector3.MoveTowards(temp, target, Time.deltaTime * lineDrawSpeed);
            cam.localPosition = temp;
            if (temp == target){
                if(index == vertices.Length - 1){	
					yield break;
				}
                vertexPos++;
                index++;
                target = vertices[index];
				}
            lineDrawSpeed += acceleration;
            yield return null;
        }
    }

	private void GetLinePoints() {
		int i = 0;
		Array.Resize(ref vertices, linePoints.Length);
		foreach (Transform t in linePoints)
		{
			vertices[i] = t.localPosition;
			i++;
		}
	}
}
