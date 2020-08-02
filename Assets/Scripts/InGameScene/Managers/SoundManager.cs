using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SettingManager settingM;
    public AudioSource audioSource;

    public AudioClip win;
    public AudioClip lose;
    public AudioClip touch;
    public AudioClip place;
    public AudioClip select;
    public AudioClip cancel;

    public void PlaySound(string audioName)
    {
        if (audioName == "win")
        {
            audioSource.PlayOneShot(win, settingM.SoundEffect * settingM.MasterVolume);
        }
        else if (audioName == "lose")
        {
            audioSource.PlayOneShot(lose, settingM.SoundEffect * settingM.MasterVolume);
        }
        else if (audioName == "touch")
        {
            audioSource.PlayOneShot(touch, settingM.SoundEffect * settingM.MasterVolume * 0.7f);
        }
        else if (audioName == "place")
        {
            audioSource.PlayOneShot(place, settingM.SoundEffect * settingM.MasterVolume);
        }
        else if (audioName == "select")
        {
            audioSource.PlayOneShot(select, settingM.SoundEffect * settingM.MasterVolume * 0.2f);
        }
    }
}