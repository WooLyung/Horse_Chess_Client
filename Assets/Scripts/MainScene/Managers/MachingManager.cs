using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachingManager : MonoBehaviour
{
    public enum STATE
    {
        NONE, MACHING, START
    }

    public Animator text1;
    public Animator text2;
    public Animator loading1;
    public Animator loading2;
    public Text button;

    private STATE state_;

    public STATE State
    {
        get
        {
            return state_;
        }
    }

    public void MachingButton()
    {
        if (State == STATE.NONE)
        {
            Maching();
        }
        else if (State == STATE.MACHING)
        {
            Dismaching();
        }
    }

    private void Maching()
    {
        state_ = STATE.MACHING;

        text1.SetBool("isLoading", true);
        text2.SetBool("isLoading", true);
        loading1.SetBool("isLoading", true);
        loading2.SetBool("isLoading", true);
        button.text = "매칭 취소";
    }

    private void Dismaching()
    {
        state_ = STATE.NONE;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        button.text = "매칭 취소";
    }
}
