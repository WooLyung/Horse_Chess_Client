using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        State = 2;
        StartCoroutine("ToMain");
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

    IEnumerator ToMain()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("ChangeScene_KeepLogin")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("ChangeScene_Login")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("ChangeScene_Register"))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        SceneManager.LoadScene(2);
    }
}
