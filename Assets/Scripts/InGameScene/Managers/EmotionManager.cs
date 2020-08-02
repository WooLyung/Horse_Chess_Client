using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    private bool selectEmotion = false;
    private float cool = 0;
    private float myTime = 0;
    private float yourTime = 0;

    public ServerManager serverM;

    public Animator selectEmotionAnim;
    public Animator[] myEmotion = new Animator[4];
    public Animator[] yourEmotion = new Animator[4];

    private void Update()
    {
        cool -= Time.deltaTime;
        if (cool < 0) cool = 0;

        if (myTime != 0)
        {
            myTime -= Time.deltaTime;
            if (myTime < 0)
            {
                myTime = 0;
                for (int i = 0; i < 4; i++) myEmotion[i].SetBool("isOn", false);
            }
        }

        if (yourTime != 0)
        {
            yourTime -= Time.deltaTime;
            if (yourTime < 0)
            {
                yourTime = 0;
                for (int i = 0; i < 4; i++) yourEmotion[i].SetBool("isOn", false);
            }
        }
    }

    public void CloseSelectEmotion()
    {
        if (selectEmotion)
        {
            selectEmotion = false;
            selectEmotionAnim.SetBool("isOn", false);
        }
    }

    public void OpenSelectEmotion()
    {
        if (!selectEmotion && cool == 0)
        {
            selectEmotion = true;
            selectEmotionAnim.SetBool("isOn", true);
        }
        else if (selectEmotion)
        {
            selectEmotion = false;
            selectEmotionAnim.SetBool("isOn", false);
        }
    }

    public void SelectMyEmotion(int code)
    {
        if (cool == 0)
        {
            myEmotion[code].SetBool("isOn", true);
            selectEmotion = false;
            selectEmotionAnim.SetBool("isOn", false);

            cool = 1;
            myTime = 1;
            serverM.SendEmotion(code);
        }
    }

    public void YourEmotion(int code)
    {
        yourEmotion[code].SetBool("isOn", true);
        yourTime = 1;
    }
}
