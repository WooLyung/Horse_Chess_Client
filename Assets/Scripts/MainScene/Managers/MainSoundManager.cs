using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSoundManager : MonoBehaviour
{
    public enum BGMSTATE
    {
        START, PLAY, FINISH
    }

    public SettingManager settingM;
    public AudioSource audioSource;

    private BGMSTATE state;
    private float time = 0;

    public void SetState(BGMSTATE state)
    {
        this.state = state;
    }

    private void Start()
    {
        state = BGMSTATE.START;
    }

    private void Update()
    {
        if (state == BGMSTATE.START)
        {
            time += Time.deltaTime;
            audioSource.volume = settingM.BGM * settingM.MasterVolume * time;

            if (time >= 1)
            {
                state = BGMSTATE.PLAY;
            }
        }
        else if (state == BGMSTATE.PLAY)
        {
            audioSource.volume = settingM.BGM * settingM.MasterVolume;
            time = 1;
        }
        else if (state == BGMSTATE.FINISH)
        {
            if (time <= 0)
            {
                time = 0;
            }

            time -= Time.deltaTime;
            audioSource.volume = settingM.BGM * settingM.MasterVolume * time;
        }
    }
}