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

    public enum GAME
    {
        RANK, NORANK, FRIENDSHIP
    }

    public MainSoundManager soundM;

    public Animator text1;
    public Animator text2;
    public Animator loading1;
    public Animator loading2;
    public Animator canvas;
    public Animator next;
    public Animator pre;
    public Text message;
    public Text button;
    public DataSender sender;

    public Image selectedGame;
    public Sprite spr_rank;
    public Sprite spr_norank;
    public Sprite spr_friendship;

    private GAME game = GAME.RANK;
    private STATE state_;
    private SocketIOComponent socket;
    private float cooltime = 0;
    private bool isMaching = false;

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

    public void SetSelectedGame(int dir)
    {
        if (State == STATE.NONE)
        {
            if (dir == 1)
            {
                if (game == GAME.RANK)
                    game = GAME.NORANK;
                else if(game == GAME.NORANK)
                    game = GAME.FRIENDSHIP;
                else if(game == GAME.FRIENDSHIP)
                    game = GAME.RANK;
            }
            else
            {
                if (game == GAME.NORANK)
                    game = GAME.RANK;
                else if (game == GAME.FRIENDSHIP)
                    game = GAME.NORANK;
                else if (game == GAME.RANK)
                    game = GAME.FRIENDSHIP;
            }

            SetButtonText();

            if (game == GAME.RANK)
            {
                selectedGame.sprite = spr_rank;
            }
            else if (game == GAME.NORANK)
            {
                selectedGame.sprite = spr_norank;
            }
            else if (game == GAME.FRIENDSHIP)
            {
                selectedGame.sprite = spr_friendship;
            }
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

        text1.SetBool("isLoading", true);
        text2.SetBool("isLoading", true);
        loading1.SetBool("isLoading", true);
        loading2.SetBool("isLoading", true);
        next.SetBool("isOn", false);
        pre.SetBool("isOn", false);
        button.text = "매칭 취소";
    }

    private void Dismaching()
    {
        state_ = STATE.NONE;

        text1.SetBool("isLoading", false);
        text2.SetBool("isLoading", false);
        loading1.SetBool("isLoading", false);
        loading2.SetBool("isLoading", false);
        next.SetBool("isOn", true);
        pre.SetBool("isOn", true);
        SetButtonText();
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
        SetButtonText();

        StartCoroutine("MachingFailEnd");
    }

    private void SetButtonText()
    {
        if (game == GAME.RANK)
        {
            button.text = "등급전 시작";
        }
        else if (game == GAME.NORANK)
        {
            button.text = "일반전 시작";
        }
        else if (game == GAME.FRIENDSHIP)
        {
            button.text = "친선전 시작";
        }
    }

    private void MatchingSuccess(SocketIOEvent obj)
    {
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
        soundM.SetState(MainSoundManager.BGMSTATE.FINISH);
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
