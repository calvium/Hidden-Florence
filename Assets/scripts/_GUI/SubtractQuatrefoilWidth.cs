using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SubtractQuatrefoilWidth : MonoBehaviour
{
	public RectTransform quatrefoil;

    void Start()
    {
        UpdateWidth();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateWidth();
        }
    }

    private void UpdateWidth()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float newWidth = quatrefoil.gameObject.active ? -quatrefoil.rect.width : 0;
        rt.offsetMax = new Vector2(newWidth, rt.offsetMax.y);
    }
}
