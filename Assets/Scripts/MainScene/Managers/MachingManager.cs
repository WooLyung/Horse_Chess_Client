using System.Collections;
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
    public DataSender sender;

    private STATE state_;
    private SocketIOComponent socket;
    private float cooltime = 0;
    private bool isMaching = false;
    private int senderCount = 0;

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

        socket.On("enterRoomResponse", enterRoomResponse);
        socket.On("matchingCancelResponse", matchingCancelResponse);
        socket.On("matchingSuccess", matchingSuccess);
    }

    private void Update()
    {
        if (cooltime > 0)
        {
            cooltime -= Time.deltaTime;
            if (cooltime < 0) cooltime = 0;
        }
    }

    public void MachingButton()
    {
        if (cooltime == 0)
        {
            if (State == STATE.NONE)
            {
                socket.Emit("enterRoomRequest");
            }
            else if (State == STATE.MACHING)
            {
                socket.Emit("matchingCancelRequest");
            }

            cooltime = 0.2f;
        }
    }
    
    public void MachingCancelTry()
    {
        if (isMaching)
            socket.Emit("matchingCancelRequest");
    }

    private void Maching()
    {
        state_ = STATE.MACHING;
        senderCount = 0;

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

    private void MatchingSuccess(SocketIOEvent obj)
    {
        senderCount++;

        if (senderCount >= 2)
            return;

        state_ = STATE.START;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        canvas.SetInteger("State", 1);

        StartCoroutine("ChangeScene");

        JSONObject data = obj.data;
        int blackIndex = int.Parse(data.GetField("room").GetField("blackDataIndex").ToString());
        int whiteIndex = blackIndex == 1 ? 0 : 1;

        DataSender newSender = GameObject.Instantiate(sender).GetComponent<DataSender>();
        newSender.name = "DataSender";
        newSender.isFirst = bool.Parse(data.GetField("data").GetField("first").ToString());
        JSONObject opponent = null;

        if (newSender.isFirst) // 백일 경우
        {
            opponent = data.GetField("users")[blackIndex];
        }
        else
        {
            opponent = data.GetField("users")[whiteIndex];
        }

        newSender.opponentName = opponent.GetField("nickname").ToString();
        newSender.opponentName = newSender.opponentName.Substring(1, newSender.opponentName.Length - 2);
        newSender.opponentRating = int.Parse(opponent.GetField("rate").ToString());

        float numOfPlayedGame = int.Parse(opponent.GetField("numOfPlayedGame").ToString());
        float numOfWonGame = int.Parse(opponent.GetField("numOfWonGame").ToString());

        if (numOfPlayedGame == 0)
            newSender.opponentWinRate = 0;
        else
            newSender.opponentWinRate = Mathf.Round((float)numOfWonGame * 10000 / numOfPlayedGame) / 100;
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

    private void enterRoomResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 매칭 시도 성공
        if (success)
        {
            isMaching = true;
            Maching();
        }
        else
        {
            String err = data.GetField("err").ToString();
            MachingFail(err.Substring(1, err.Length - 2));
        }
    }

    private void matchingCancelResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 매칭 취소 성공
        if (success)
        {
            isMaching = false;
            Dismaching();
        }
    }

    private void matchingSuccess(SocketIOEvent obj)
    {
        MatchingSuccess(obj);
    }
}
