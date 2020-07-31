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

    private void Start()
    {
        audioSource.volume = settingM.SoundEffect * settingM.MasterVolume;
    }

    public void PlaySound(string audioName)
    {
        if (audioName == "win")
        {
            audioSource.PlayOneShot(win);
        }
        else if (audioName == "lose")
        {
            audioSource.PlayOneShot(lose);
        }
        else if (audioName == "touch")
        {
            audioSource.PlayOneShot(touch);
        }
        else if (audioName == "place")
        {
            audioSource.PlayOneShot(place);
        }
    }
}