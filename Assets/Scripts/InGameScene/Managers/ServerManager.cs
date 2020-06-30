using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private SocketIOComponent socket;
    public InGameManager ingameM;
    public InGameData data;

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("placeResponse", placeResponse);
        socket.On("turnStart", turnStart);
        socket.On("gameOver", gameOver);
	}

    public void PieceMove(int x1, int y1, int x2, int y2)
    {
        JSONObject json = new JSONObject("{\"beforeX\":" + x1 + ",\"beforeY\":" + y1 + ",\"afterX\":" + x2 + ",\"afterY\":" + y2 + "}");
        socket.Emit("turnEndRequest", json);
    }

    public void Surrender()
    {
        socket.Emit("surrenderRequest");
    }

    public void Stalemate()
    {
        socket.Emit("stalemateRequest");
    }

    public void SettingDone()
    {
        int count = 0;
        Vector2Int[] pos = new Vector2Int[4];

        for (int x = 1; x <= 8; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                if (data.map[x, y] == InGameData.TILE.PLAYER)
                {
                    if (!data.isFirst) // 흑
                        pos[count] = new Vector2Int(x - 1, 8 - y);
                    else // 백
                        pos[count] = new Vector2Int(x - 1, y - 1);
                    count++;
                }
            }
        }

        JSONObject emitData = new JSONObject("{\"firstX\":" + pos[0].x + ", \"firstY\":" + pos[0].y +
            ", \"secondX\":" + pos[1].x + ", \"secondY\":" + pos[1].y +
            ", \"thirdX\":" + pos[2].x + ", \"thirdY\":" + pos[2].y +
            ", \"fourthX\":" + pos[3].x + ", \"fourthY\":" + pos[3].y + "}");
        socket.Emit("placeRequest", emitData);
    }

    private void turnStart(SocketIOEvent obj) // 턴 시작
    {
        JSONObject json = obj.data;
        int turn = int.Parse(json.GetField("data").GetField("turn").ToString());
        bool isMyturn = false;

        if (data.isFirst && turn == 2 || !data.isFirst && turn == 1) // 자신이 백이고 턴이 2(백) 이거나, 자신이 흑이고 턴이 1(흑) 일 때
            isMyturn = true;

        ingameM.TurnStart(isMyturn, json.GetField("data").GetField("chessboard"));
    }

    private void placeResponse(SocketIOEvent obj) // 턴 종료
    {
    }

    private void gameOver(SocketIOEvent obj)
    {
        ingameM.GameFinish(obj.data);
    }
}