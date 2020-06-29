using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private SocketIOComponent socket;
    public InGameManager ingameM;

	private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("turnStart", turnStart);
        socket.On("turnEnd", turnEnd);
	}

    public void SettingDone(InGameData data)
    {
        int count = 0;
        Vector2Int[] pos = new Vector2Int[4];

        for (int x = 1; x <= 8; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                if (data.map[x, y] == InGameData.TILE.PLAYER)
                {
                    pos[count] = new Vector2Int(x - 1, y - 1);
                    count++;
                }
            }
        }

        JSONObject emitData = new JSONObject("{\"fisrtX\":" + pos[0].x + ", \"fisrtY\":" + pos[0].y +
            ", \"secondX\":" + pos[1].x + ", \"secondY\":" + pos[1].y +
            ", \"thirdX\":" + pos[2].x + ", \"thirdY\":" + pos[2].y +
            ", \"fourthX\":" + pos[3].x + ", \"fourthY\":2" + pos[3].y + "}");
        socket.Emit("placeRequest", emitData);
    }

    private void turnStart(SocketIOEvent obj) // 턴 시작
    {
        Debug.Log("턴 시작");
        ingameM.TurnStart();
    }

    private void turnEnd(SocketIOEvent obj) // 턴 종료
    {
        Debug.Log("턴 종료");
    }
}
