using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private SocketIOComponent socket;

	private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
	}

    public void SettingDone(InGameData data)
    {
        // 세팅 완료 메세지
    }

    private void aaa(SocketIOEvent obj) // 턴 시작
    {

    }

    private void bbb(SocketIOEvent obj) // 턴 종료
    {

    }
}
