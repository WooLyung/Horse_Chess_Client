using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;

public class LoginManager : MonoBehaviour
{
    float term = 0;

    private SocketIOComponent socket;

    public LoginSceneManager loginSceneManager;
    public InputField login_id;
    public InputField login_pwd;
    public InputField register_id;
    public InputField register_pwd;
    public InputField register_pwd2;
    public InputField register_nick;

    private void Start()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("registerResponse", registerResponse);
        socket.On("loginResponse", loginResponse);
    }

    private void Update()
    {
        if (term > 0) term -= Time.deltaTime;
        if (term < 0) term = 0;
    }

    public void Register()
    {
        if (term == 0)
        {
            term = 2;

            // 아이디 5 미만 or 24 초과
            if (register_id.text.Length < 5 || register_id.text.Length > 24)
            {
                loginSceneManager.Alert("아이디는 5 ~ 24글자만 가능합니다");
                return;
            }

            // 비밀번호 5 미만 or 24 초과
            if (register_pwd.text.Length < 5 || register_pwd.text.Length > 24)
            {
                loginSceneManager.Alert("비밀번호는 5 ~ 24글자만 가능합니다");
                return;
            }

            // 닉네임 없음
            if (register_nick.text == "")
            {
                loginSceneManager.Alert("닉네임을 입력하세요");
                return;
            }

            // 닉네임 24 초과
            if (register_nick.text.Length > 24)
            {
                loginSceneManager.Alert("닉네임은 24글자 이하여야 합니다.");
                return;
            }

            // 비밀번호 확인 틀림
            if (register_pwd.text != register_pwd2.text)
            {
                loginSceneManager.Alert("비밀번호 확인이 틀렸습니다.");
                return;
            }

            String json = String.Format("\"username\":\"{0}\", \"password\":\"{1}\", \"nickname\":\"{2}\"", register_id.text, register_pwd.text, register_nick.text);
            JSONObject data = new JSONObject("{" + json + "}");

            socket.Emit("registerRequest", data);
        }
    }

    public void Login()
    {
        if (term == 0)
        {
            term = 2;

            // 아이디 없음
            if (login_id.text == "")
            {
                loginSceneManager.Alert("아이디를 입력하세요");
                return;
            }

            // 비밀번호 없음
            if (login_pwd.text == "")
            {
                loginSceneManager.Alert("비밀번호를 입력하세요");
                return;
            }

            String json = String.Format("\"username\":\"{0}\", \"password\":\"{1}\"", login_id.text, login_pwd.text);
            JSONObject data = new JSONObject("{" + json + "}");

            socket.Emit("loginRequest", data);
        }
    }

    private void loginResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 로그인 성공
        if (success)
        {
            loginSceneManager.Alert("로그인 성공!");
            return;
        }
        else
        {
            String err = data.GetField("err").ToString();
            loginSceneManager.Alert(err.Substring(1, err.Length - 2));
            return;
        }
    }

    private void registerResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 회원가입 성공
        if (success)
        {
            loginSceneManager.Alert("회원가입 성공!");
            return;
        }
        else
        {
            String err = data.GetField("err").ToString();
            loginSceneManager.Alert(err.Substring(1, err.Length - 2));
            return;
        }
    }
}