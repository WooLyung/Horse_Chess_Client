﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class InfoManager : MonoBehaviour
{
    public Animator animator;
    public MachingManager machingManager;
    public MainSoundManager soundM;

    public Text nickname;
    public Text ranking;
    public Text winRate;
    public Text record;
    public Text score;

    public Text nickname2;
    public Text ranking2;
    public Text score2;

    private SocketIOComponent socket;

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }

    private void Start()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData playerData = PlayerData.Instance;

            nickname.text = nickname2.text = playerData.Name;
            score.text = score2.text = playerData.Rate + "점";
            ranking.text = ranking2.text = playerData.Ranking + "등";
            record.text = playerData.Game + "전 " + playerData.WinGame + "승 " + (playerData.Game - playerData.WinGame) + "패";

            if (playerData.WinGame == 0)
                winRate.text = "0%";
            else
                winRate.text = Mathf.Round((float)playerData.WinGame * 10000 / playerData.Game) / 100 + "%";

            // -----
            
            if (playerData.IsFirst)
            {
                playerData.IsFirst = false;
                StartCoroutine("TutorialFirst");
            }
        }
    }

    public void Tutorial()
    {
        animator.SetInteger("State", 1);
        StartCoroutine("ChangeTutorial");
        machingManager.MachingCancelTry();
    }

    public void Logout()
    {
        machingManager.MachingCancelTry();
        animator.SetInteger("State", 1);
        socket.Emit("logoutRequest");
        PlayerPrefs.SetInt("KeepLogin", 0);
        StartCoroutine("ChangeTitle");
    }

    IEnumerator ChangeTutorial()
    {
        soundM.SetState(MainSoundManager.BGMSTATE.FINISH);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }

    IEnumerator ChangeTitle()
    {
        soundM.SetState(MainSoundManager.BGMSTATE.FINISH);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    IEnumerator TutorialFirst()
    {
        yield return new WaitForSeconds(1);
        Tutorial();
    }
}
