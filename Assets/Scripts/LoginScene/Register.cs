using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class Register : MonoBehaviour
{
    public SocketIOComponent socket;
    public InputField id;
    public InputField pwd;
    public InputField pwd2;
    public InputField nick;

    public void Start()
    {
        
    }

    public void OnClick()
    {
        if (id.text != null && pwd.text != null && pwd2.text != null && nick.text != null)
        {
            if (pwd.text != pwd2.text)
            {
                Debug.Log("비밀번호 확인이 다름");
            }
        }
    }
}
