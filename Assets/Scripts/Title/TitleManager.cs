using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animation animation;
    public bool isChange = false;

	void Update ()
    {
        InputCheck();

        if (!animation.IsPlaying("TitleAnim") && isChange)
        {
            ChangeScene();
        }
	}

    private void InputCheck()
    {
        // 로그인 넘어가기
        if (Input.GetMouseButtonDown(0))
        {
            if (!isChange)
            {
                animation.Play();
                isChange = true;
            }
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Login");
    }
}
