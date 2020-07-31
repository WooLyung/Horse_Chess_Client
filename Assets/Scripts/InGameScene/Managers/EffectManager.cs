using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public SettingManager setting;
    public SoundManager soundM;

    public Animator addTimeEffect;
    public Animator buttonHighlight_takeback;
    public Animator buttonHighlight_addtime;

    public Transform effects;
    public GameObject dust;
    public GameObject smallDust;
    public GameObject firework;

    public void WinEffect()
    {
        if (setting.Effect)
        {
            for (int i = 0; i < 25; i++)
            {
                var particle = GameObject.Instantiate(firework, effects);
                particle.transform.position = new Vector3(-6, 0);
                particle.GetComponent<FireworkParticle>().isLeft = true;
            }

            for (int i = 0; i < 25; i++)
            {
                var particle = GameObject.Instantiate(firework, effects);
                particle.transform.position = new Vector3(6, 0);
                particle.GetComponent<FireworkParticle>().isLeft = false;
            }
        }
    }

    public void AddTimeEffect()
    {
        if (setting.Effect)
        {
            StartCoroutine("AddTimeEffectPlay");
        }
    }

    public void ButtonHighLight_takeback()
    {
        if (setting.Effect)
        {
            StartCoroutine("ButtonHighLightPlay_takeback");
        }
    }

    public void ButtonHighlight_addtime()
    {
        if (setting.Effect)
        {
            StartCoroutine("ButtonHightLightPlay_addtime");
        }
    }

    public void PieceMoveEffect(Vector2Int pos)
    {
        soundM.PlaySound("place");

        if (setting.Effect)
        {
            for (int i = 0; i < 10; i++)
            {
                var newEffect = GameObject.Instantiate(dust, effects);
                newEffect.transform.position = new Vector3(pos.x - 4.5f, pos.y - 4.5f);
                newEffect.name = "dust";
            }
        }
    }

    public void BoardClickEffect(Vector3 pos)
    {
        soundM.PlaySound("touch");

        if (setting.Effect)
        {
            for (int i = 0; i < 5; i++)
            {
                var newEffect = GameObject.Instantiate(smallDust, effects);
                newEffect.transform.position = new Vector3(pos.x, pos.y);
                newEffect.name = "smallDust";
            }
        }
    }

    IEnumerator AddTimeEffectPlay()
    {
        addTimeEffect.SetBool("Play", true);
        yield return new WaitForSeconds(0.5f);
        addTimeEffect.SetBool("Play", false);
    }

    IEnumerator ButtonHighLightPlay_takeback()
    {
        buttonHighlight_takeback.SetBool("Play", true);
        yield return new WaitForSeconds(0.5f);
        buttonHighlight_takeback.SetBool("Play", false);
    }

    IEnumerator ButtonHightLightPlay_addtime()
    {
        buttonHighlight_addtime.SetBool("Play", true);
        yield return new WaitForSeconds(0.5f);
        buttonHighlight_addtime.SetBool("Play", false);
    }
}
