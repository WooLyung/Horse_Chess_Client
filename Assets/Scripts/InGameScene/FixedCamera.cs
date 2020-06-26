using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
	void Awake ()
    {
        Camera.main.orthographicSize = 9 * 0.5625f / Camera.main.aspect;
    }
}