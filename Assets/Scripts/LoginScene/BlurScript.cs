using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurScript : MonoBehaviour
{
    [SerializeField]
    public int _radius;
    public Material blurMaterial;

    public int radius
    {
        get
        {
            _radius = blurMaterial.GetInt("Radius");
            return _radius;
        }

        set
        {
            Debug.Log("TEST");
            blurMaterial.SetInt("Radius", value);
        }
    }
}