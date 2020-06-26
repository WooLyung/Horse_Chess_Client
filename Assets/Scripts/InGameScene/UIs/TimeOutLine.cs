using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOutLine : MonoBehaviour
{
    public Image image;
    public RectTransform rect;

	void Update ()
    {
        rect.rotation = Quaternion.Euler(0, 0, 360 - image.fillAmount * 360);
	}
}