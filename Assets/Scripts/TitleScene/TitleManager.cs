using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SocketIO;

public class TitleManager : MonoBehaviour
{
    public Animation animation;
    public bool isChange = false;

    private void Awake()
    {
        GameObject socketObj = GameObject.Find("SocketIO");

        if (socketObj != null)
        {
            SocketIOComponent socket = socketObj.GetComponent<SocketIOComponent>();
            socket.Close();

            GameObject.Destroy(socketObj);
            GameObject.Destroy(GameObject.Find("PlayerData"));
        }
    }

    private void Update()
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
