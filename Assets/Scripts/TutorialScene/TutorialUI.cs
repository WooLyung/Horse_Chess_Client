using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Text nickname;
    public Text info;

	private void Start ()
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
}
