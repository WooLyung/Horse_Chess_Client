﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using System;

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
    private SocketIOComponent socket;

    public STATE State
    {
        get
        {
            return state_;
        }
    }

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("machingResponse", machingResponse);
    }

    public void MachingButton()
    {
        if (State == STATE.NONE)
        {
            Maching();
            socket.Emit("maching");
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
        button.text = "게임 시작";
    }

    private void MachingFail(string err)
    {
        state_ = STATE.NONE;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        canvas.SetBool("Text", true);
        message.text = err;
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

    private void machingResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 매칭 성공
        if (success)
        {
            Debug.Log(data);
            MachingSuccess();
        }
        else
        {
            String err = data.GetField("err").ToString();
            MachingFail(err.Substring(1, err.Length - 2));
        }
    }
}
