using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public Animator animator;
    public Text text;
    private bool _flag = false;

    public bool flag
    {
        get
        {
            return _flag;
        }

        set
        {
            _flag = value;
            PlayAnim();
        }
    }

    private void PlayAnim()
    {
        animator.SetBool("isOn", flag);
        text.text = flag ? "켜짐" : "꺼짐";
    }
}
