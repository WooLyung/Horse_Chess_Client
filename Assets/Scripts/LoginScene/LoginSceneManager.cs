using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneManager : MonoBehaviour
{
    public Animator animator;
    public LoginManager loginManager;

    private int State
    {
        get
        {
            return animator.GetInteger("State");
        }

        set
        {
            animator.SetInteger("State", value);
        }
    }

    private bool Text
    {
        get
        {
            return animator.GetBool("Text");
        }

        set
        {
            animator.SetBool("Text", value);
        }
    }

    public void ToLogin()
    {
        if (Text == false && State == 1)
        {
            State = 0;
        }
    }

    public void ToRegister()
    {
        if (Text == false && State == 0)
        {
            State = 1;
        }
    }
}
