using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Animator canvas;
    public Text message;
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
            MachingSuccess();
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
        button.text = "게임 시작";
    }

    private void MachingFail()
    {
        state_ = STATE.NONE;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        canvas.SetBool("Text", true);
        message.text = "매칭에 실패했습니다";
        button.text = "게임 시작";

        StartCoroutine("MachingFailEnd");
    }

    private void MachingSuccess()
    {
        state_ = STATE.START;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        canvas.SetInteger("State", 1);

        StartCoroutine("ChangeScene");
    }

    IEnumerator MachingFailEnd()
    {
        yield return new WaitForSeconds(1.5f);
        canvas.SetBool("Text", false);
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(3);
    }
}
