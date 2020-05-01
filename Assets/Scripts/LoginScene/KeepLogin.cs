using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepLogin : MonoBehaviour
{
    public Animator animator;

    private bool Var
    {
        get
        {
            if (!PlayerPrefs.HasKey("KeepLogin"))
                PlayerPrefs.SetInt("KeepLogin", 0);
            return PlayerPrefs.GetInt("KeepLogin") == 1;
        }

        set
        {
            PlayerPrefs.SetInt("KeepLogin", value ? 1 : 0);
        }
    }

    private void Start()
    {
        animator.SetBool("on", Var);
    }

    public void Toggle()
    {
        Var = !Var;
        animator.SetBool("on", Var);
    }
}
