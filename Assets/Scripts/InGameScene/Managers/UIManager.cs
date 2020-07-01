using System.Collections;
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
    public ServerManager serverM;
    public InGameManager ingameM;
    public InGameData data;
    public Animator textAnim;
    public Animator sceneAnim;
    public Animator resultAnim;
    public Animator objectAnim;
    public Animator takebackHighlight;
    public Animator addtimeHighlight;
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
                winRate = "0%";
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
            if (data.turnCount2 <= 2)
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
        int bet = rate - PlayerData.Instance.Rate;
        string betS = (bet > 0) ? "+" + bet : bet + "";

        PlayerData.Instance.Rate = rate;
        PlayerData.Instance.Game++;

        resultReason.text = json.GetField("data").GetField("message").ToString();
        resultReason.text = resultReason.text.Substring(1, resultReason.text.Length - 2);
        resultInfo.text = data.turnCount + "턴 / " + PlayerData.Instance.Rate + "(" + betS + ")";

        if (bet > 0)
        {
            result.sprite = win;
            PlayerData.Instance.WinGame++;
        }
        else
        {
            result.sprite = lose;
        }
    }

    public void TakeBack()
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.GAME)
        {
            if (!data.isClicked_takeback)
            {
                data.isClicked_takeback = true;

                if (data.isMyTurn && data.turnCount2 > 2)
                {
                    serverM.RequestTakeBack();
                    data.isClicked_takeback = true;
                }
                else if (data.isSended_takeback)
                {
                    serverM.AcceptTakeBack();
                    data.isClicked_takeback = true;
                }
            }
        }
    }

    public void AddTime()
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.GAME)
        {
            if (!data.isClicked_addtime)
            {
                if (data.isMyTurn)
                {
                    serverM.RequestAddTime();
                    data.isClicked_addtime = true;
                }
                else if (data.isSended_addtime)
                {
                    serverM.AcceptAddTime();
                    data.isClicked_addtime = true;
                }
            }
        }
    }

    public void Surrender()
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.GAME)
        {
            ingameM.Surrender();
        }
    }

    public void TakeBackHighlight()
    {
        StartCoroutine("TakeBackHighlightCoroutine");
    }

    public void AddTimeHighlight()
    {
        StartCoroutine("AddTimeHighlightCoroutine");
    }

    private IEnumerator TakeBackHighlightCoroutine()
    {
        takebackHighlight.SetBool("Play", true);
        yield return new WaitForSeconds(1);
        takebackHighlight.SetBool("Play", false);
    }

    private IEnumerator AddTimeHighlightCoroutine()
    {
        addtimeHighlight.SetBool("Play", true);
        yield return new WaitForSeconds(1);
        addtimeHighlight.SetBool("Play", false);
    }

    private IEnumerator GameFinishCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        resultAnim.SetInteger("State", 1);
        yield return new WaitForSeconds(3);
        resultAnim.SetInteger("State", 2);
        sceneAnim.SetBool("Finish", true);
        objectAnim.SetInteger("state", 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}