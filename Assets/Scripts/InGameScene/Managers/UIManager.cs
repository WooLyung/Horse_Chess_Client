﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private float time = 0;
    private float maxTime = 60;
    private float textTime = 0;
    private bool isAppearTime = false;
    private bool isTimerActivate = false;

    public EffectManager effectM;
    public InGameManager ingameM;
    public InGameData data;
    public Animator textAnim;
    public Animator sceneAnim;
    public Animator resultAnim;
    public Animator objectAnim;
    public Text text;

    public Image timer;
    public Text timerText;
    public Text opponentName;
    public Text opponentInfo;
    public Text playerName;
    public Text playerInfo;
    public Text takeBack;
    public Text addTime;

    public Sprite win;
    public Sprite lose;
    public Image result;
    public Text resultInfo;
    public Text resultReason;

    private void Start()
    {
        opponentName.text = data.opponentName;
        opponentInfo.text = data.opponentRating + "점 / " + data.opponentWinRate + "%";

        if (PlayerData.Instance != null)
        {
            PlayerData playerData = PlayerData.Instance;
            string winRate = "";

            if (playerData.WinGame == 0)
                winRate = "-%";
            else
                winRate = playerData.WinGame * 100 / playerData.Game + "%";

            playerName.text = PlayerData.Instance.Name;
            playerInfo.text = PlayerData.Instance.Rate + "점 / " + winRate;
        }
    }

    private void Update()
    {
        if (isTimerActivate)
        {
            time += Time.deltaTime;
            timer.fillAmount = time / maxTime;
            timerText.text = (int)(Mathf.Ceil(maxTime - time)) + "";

            if (time >= maxTime)
                isTimerActivate = false;
        }

        if (isAppearTime)
        {
            textTime += Time.deltaTime;
            if (textTime >= 2.7f)
            {
                textTime = 0;
                isAppearTime = false;
                textAnim.SetBool("IsAppear", false);
            }
        }
    }

    public void StartTimer(float maxTime)
    {
        this.maxTime = maxTime;
        time = 0;
        isTimerActivate = true;
    }

    public void AddTimer(float time)
    {
        maxTime += time;
        isTimerActivate = true;
        effectM.AddTimeEffect();
    }

    public void SetButtonText()
    {
        addTime.text = "시간 연장";
        takeBack.text = "무르기";

        if (data.isMyTurn)
        {
            if (data.turnCount <= 2)
            {
                takeBack.text = "불가능";
            }
        }
    }

    public void ShowText(string text)
    {
        textAnim.SetBool("IsAppear", true);
        textTime = 0;
        isAppearTime = true;
        this.text.text = text;
    }

    public void GameFinish(JSONObject json)
    {
        StartCoroutine("GameFinishCoroutine");

        int rate = int.Parse(json.GetField("data").GetField("userData").GetField("rate").ToString());
        string rateS = (rate > 0) ? "+" + rate : rate + "";

        PlayerData.Instance.Rate += rate;
        PlayerData.Instance.Game++;

        resultReason.text = json.GetField("data").GetField("message").ToString();
        resultReason.text = resultReason.text.Substring(1, resultReason.text.Length - 2);
        resultInfo.text = data.turnCount + "턴 / " + PlayerData.Instance.Rate + "(" + rateS + ")";

        if (rate > 0)
        {
            result.sprite = win;
            PlayerData.Instance.WinGame++;
        }
        if (rate < 0)
        {
            result.sprite = lose;
        }
    }

    public void TakeBack()
    {
        if (!data.isClicked_takeback)
        {
            data.isClicked_takeback = true;

            if (data.isMyTurn)
            {
                // 무르기 요청 메세지 보냄
            }
            else if (data.isSended_takeback)
            {
                // 무르기 수락 메세지 보냄
            }
        }
    }

    public void AddTime()
    {
        if (!data.isClicked_addtime)
        {
            data.isClicked_addtime = true;

            if (data.isMyTurn)
            {
                // 시간연장 요청 메세지 보냄
            }
            else if (data.isSended_addtime)
            {
                // 시간연장 수락 메세지 보냄
            }
        }
    }

    public void Surrender()
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.GAME)
            ingameM.Surrender();
    }

    private IEnumerator GameFinishCoroutine()
    {
        yield return new WaitForSeconds(1);
        resultAnim.SetInteger("State", 1);
        yield return new WaitForSeconds(5);
        resultAnim.SetInteger("State", 2);
        sceneAnim.SetBool("Finish", true);
        objectAnim.SetInteger("state", 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}