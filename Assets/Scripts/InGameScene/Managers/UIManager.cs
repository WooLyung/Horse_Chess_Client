using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private float time = 0;
    private float maxTime = 60;
    private bool isTimerActivate = false;

    public EffectManager effectM;
    public InGameData data;

    public Image timer;
    public Text timerText;
    public Text opponentName;
    public Text opponentInfo;
    public Text playerName;
    public Text playerInfo;
    public Text takeBack;
    public Text addTime;

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
}
