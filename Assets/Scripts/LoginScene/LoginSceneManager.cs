using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    public Animator animator;
    public LoginManager loginManager;
    public Text alertText;

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

    public void ChangeScene()
    {
        State = 3;
    }

    public void LoginAppear()
    {
        State = 0;
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

    public void Alert(string message)
    {
        Text = true;
        alertText.text = message;
        StartCoroutine("CloseAlert");
    }

    IEnumerator CloseAlert()
    {
        yield return new WaitForSeconds(1.2f);
        Text = false;
    }
}
