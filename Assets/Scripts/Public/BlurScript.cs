using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScript : MonoBehaviour
{
    private int lastRadius = 0;

    public Material blurMaterial;
    public int radius = 0;

    public void Update()
    {
        if (radius != lastRadius)
        {
            blurMaterial.SetInt("_Radius", radius);
            lastRadius = radius;
        }
    }
}