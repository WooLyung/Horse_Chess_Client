using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public SettingManager settingManager;
    public Lever lever;
    public Slider slider_masterVolume;
    public Slider slider_bgm;
    public Slider slider_soundEffect;

    private void Start()
    {
        slider_masterVolume.value = settingManager.MasterVolume;
        slider_bgm.value = settingManager.BGM;
        slider_soundEffect.value = settingManager.SoundEffect;
        lever.flag = settingManager.Effect;
    }

    public void ChangeMasterVolume(float value)
    {
        settingManager.MasterVolume = value;
    }

    public void ChangeSoundEffect(float value)
    {
        settingManager.SoundEffect = value;
    }

    public void ChangeBGM(float value)
    {
        settingManager.BGM = value;
    }

    public void ChangeEffect()
    {
        lever.flag = !lever.flag;
        settingManager.Effect = lever.flag;
    }

    public void ChangeMasterVolume()
    {
        settingManager.MasterVolume = slider_masterVolume.value;
    }

    public void ChangeSoundEffect()
    {
        settingManager.SoundEffect = slider_soundEffect.value;
    }

    public void ChangeBGM()
    {
        settingManager.BGM = slider_bgm.value;
    }
}
