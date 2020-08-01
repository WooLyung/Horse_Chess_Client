using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    public BGMManager bgmM;

    public Animator animator_ui;
    public Animator animator_obj;
    public Text nickname;
    public Text info;

	private void Start()
    {
        if (PlayerData.Instance != null)
        {
            PlayerData playerData = PlayerData.Instance;

            nickname.text = playerData.Name;
            info.text = playerData.Rate + "점";

            if (playerData.WinGame == 0)
                info.text += " / -%";
            else
                info.text += playerData.WinGame * 100 / playerData.Game + "%";
        }
    }

    public void ToMain()
    {
        animator_ui.SetInteger("state", 1);
        animator_obj.SetInteger("state", 1);
        StartCoroutine("ChangeScene");
    }

    IEnumerator ChangeScene()
    {
        bgmM.SetState(BGMManager.BGMSTATE.FINISH);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
