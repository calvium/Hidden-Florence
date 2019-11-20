using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paintingRaycast : MonoBehaviour
{
    [SerializeField] private IMStartMenu menu;
    // Start is called before the first frame update
    public bool active = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float posX = Screen.width / 2f;
		float posY = Screen.height / 2f;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(posX, posY, Mathf.Infinity));
		RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200)) {
			if(this.gameObject == hit.collider.gameObject == active){
				Debug.Log("You have selected the " + hit.collider.name);
                menu.callSetText(5);
			}
        }
    }
}
