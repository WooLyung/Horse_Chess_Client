﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    public Animator animator;

    public Text nickname;
    public Text ranking;
    public Text winRate;
    public Text record;
    public Text score;

    public Text nickname2;
    public Text ranking2;
    public Text score2;

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
                winRate.text = "-%";
            else
                winRate.text = playerData.WinGame * 100 / playerData.Game + "%";
        }
    }

    public void Tutorial()
    {
        animator.SetInteger("State", 1);
        StartCoroutine("ChangeTutorial");
    }

    IEnumerator ChangeTutorial()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }
}
