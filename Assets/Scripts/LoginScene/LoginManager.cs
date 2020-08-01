using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;

public class LoginManager : MonoBehaviour
{
    float term = 0;

    private bool isKeepLogin = false;
    private SocketIOComponent socket;

    public LoginSceneManager loginSceneManager;
    public InputField login_id;
    public InputField login_pwd;
    public InputField register_id;
    public InputField register_pwd;
    public InputField register_pwd2;
    public InputField register_nick;

    private bool KeepLogin
    {
        get
        {
            if (!PlayerPrefs.HasKey("KeepLogin"))
                PlayerPrefs.SetInt("KeepLogin", 0);
            return PlayerPrefs.GetInt("KeepLogin") == 1;
        }

        set
        {
            PlayerPrefs.SetInt("KeepLogin", value ? 1 : 0);
        }
    }

    private void Start()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("registerResponse", registerResponse);
        socket.On("loginResponse", loginResponse);

        // 로그인 유지
        if (KeepLogin)
        {
            isKeepLogin = true;
            StartCoroutine("AutoLogin");
        }
        else
        {
            loginSceneManager.LoginAppear();
        }
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
            PlayerPrefs.SetString("KeepLogin_ID", login_id.text);
            PlayerPrefs.SetString("KeepLogin_pwd", login_pwd.text);
            loginSceneManager.ChangeScene();

            JSONObject playerData = data.GetField("data").GetField("userData");
            string nickname = playerData.GetField("nickname").ToString();
            nickname = nickname.Substring(1, nickname.Length - 2);
            int rate = int.Parse(playerData.GetField("rate").ToString());
            int game = int.Parse(playerData.GetField("numOfPlayedGame").ToString());
            int winGame = int.Parse(playerData.GetField("numOfWonGame").ToString());
            PlayerDataUpdate(nickname, rate, game, winGame);
        }
        else
        {
            if (isKeepLogin)
            {
                loginSceneManager.LoginAppear();
            }
            else
            {
                String err = data.GetField("err").ToString();
                loginSceneManager.Alert(err.Substring(1, err.Length - 2));
            }

            isKeepLogin = false;
        }
    }

    private void registerResponse(SocketIOEvent obj)
    {
        JSONObject data = obj.data;
        bool success = Boolean.Parse(data.GetField("success").ToString());

        // 회원가입 성공
        if (success)
        {
            PlayerPrefs.SetString("KeepLogin_ID", register_id.text);
            PlayerPrefs.SetString("KeepLogin_pwd", register_pwd.text);
            loginSceneManager.Alert("회원가입 성공!");
            loginSceneManager.ChangeScene();

            JSONObject playerData = data.GetField("data").GetField("userData");
            string nickname = playerData.GetField("nickname").ToString();
            nickname = nickname.Substring(1, nickname.Length - 2);
            int rate = int.Parse(playerData.GetField("rate").ToString());
            int game = int.Parse(playerData.GetField("numOfPlayedGame").ToString());
            int winGame = int.Parse(playerData.GetField("numOfWonGame").ToString());
            PlayerDataUpdate(nickname, rate, game, winGame);

            PlayerData.Instance.IsFirst = true;
        }
        else
        {
            String err = data.GetField("err").ToString();
            loginSceneManager.Alert(err.Substring(1, err.Length - 2));
        }
    }

    private void PlayerDataUpdate(string nickname, int rate, int game, int winGame)
    {
        PlayerData playerData = PlayerData.Instance;
        playerData.Name = nickname;
        playerData.Rate = rate;
        playerData.Game = game;
        playerData.WinGame = winGame;
    }

    public void ShowKeyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    IEnumerator AutoLogin()
    {
        yield return new WaitForSeconds(0.1f);

        String id = PlayerPrefs.GetString("KeepLogin_ID");
        String pwd = PlayerPrefs.GetString("KeepLogin_pwd");
        login_id.text = id;
        login_pwd.text = pwd;
        String json = String.Format("\"username\":\"{0}\", \"password\":\"{1}\"", id, pwd);

        JSONObject data = new JSONObject("{" + json + "}");

        socket.Emit("loginRequest", data);
    }
}