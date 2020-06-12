using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    private float _masterVolume = 0;
    private float _soundEffect = 0;
    private float _bgm = 0;
    private bool _effect = false;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Setting.BGM"))
        {
            _masterVolume = PlayerPrefs.GetFloat("Setting.MasterVolume");
            _soundEffect = PlayerPrefs.GetFloat("Setting.SoundEffect");
            _bgm = PlayerPrefs.GetFloat("Setting.BGM");
            _effect = PlayerPrefs.GetInt("Setting.Effect") == 1;
        }
        else
        {
            MasterVolume = 1;
            SoundEffect = 0.5f;
            BGM = 0.5f;
            Effect = true;
        }
    }

    public float MasterVolume
    {
        get
        {
            return _masterVolume;
        }

        set
        {
            _masterVolume = value;
            PlayerPrefs.SetFloat("Setting.MasterVolume", _masterVolume);
        }
    }

    public float BGM
    {
        get
        {
            return _bgm;
        }

        set
        {
            _bgm = value;
            PlayerPrefs.SetFloat("Setting.BGM", _bgm);
        }
    }

    public float SoundEffect
    {
        get
        {
            return _soundEffect;
        }

        set
        {
            _soundEffect = value;
            PlayerPrefs.SetFloat("Setting.SoundEffect", _soundEffect);
        }
    }


    public bool Effect
    {
        get
        {
            return _effect;
        }

        set
        {
            _effect = value;
            PlayerPrefs.SetInt("Setting.Effect", _effect ? 1 : 0);
        }
    }
}