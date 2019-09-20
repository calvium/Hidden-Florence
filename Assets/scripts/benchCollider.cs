using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class benchCollider : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera cam;
    [SerializeField] private IMStartMenu menu;
    void Start() {
		cam = Camera.main;
	}

    void OnTriggerEnter(Collider col) {
		Debug.Log("scanning0");
        if (col.gameObject == Camera.main.gameObject) {
            menu.callSetText(6);
		}
    }
}
