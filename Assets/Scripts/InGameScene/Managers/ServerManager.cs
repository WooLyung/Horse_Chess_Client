using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private SocketIOComponent socket;
    public InGameManager ingameM;
    public EmotionManager emotionM;
    public UIManager uiM;
    public InGameData data;

    private void Awake()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        socket.On("turnStart", turnStart);
        socket.On("gameOver", gameOver);
        socket.On("proposeExtendTimeLimits", proposeExtendTimeLimits);
        socket.On("updateTimeLimits", updateTimeLimits);
        socket.On("proposeTurnBack", proposeTurnBack);
        socket.On("emotion", emotion);
    }

    public void SendEmotion(int code)
    {
        JSONObject json = new JSONObject("{data:{number:" + code + "}}");
        socket.Emit("emotionRequest", json);
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

    public void RequestTakeBack() // 무르기 요청
    {
        socket.Emit("proposeTurnBackRequest");
        uiM.ShowText("상대방에게 무르기를 요청했습니다");
        uiM.takeBack.text = "요청중";
    }

    public void RequestAddTime() // 시간연장 요청
    {
        socket.Emit("proposeExtendTimeRequest");
        uiM.ShowText("상대방에게 시간연장을 요청했습니다");
        uiM.addTime.text = "요청중";
    }

    public void AcceptTakeBack() // 무르기 수락
    {
        socket.Emit("allowTurnBackRequest");
    }

    public void AcceptAddTime() // 시간연장 수락
    {
        socket.Emit("allowExtendTimeRequest");
    }

    private void turnStart(SocketIOEvent obj) // 턴 시작
    {
        JSONObject json = obj.data;
        int turn = int.Parse(json.GetField("data").GetField("room").GetField("turn").ToString());
        bool isTurnBack = bool.Parse(json.GetField("data").GetField("turnBack").ToString());
        bool isMyturn = false;

        if (data.isFirst && turn == 2 || !data.isFirst && turn == 1) // 자신이 백이고 턴이 2(백) 이거나, 자신이 흑이고 턴이 1(흑) 일 때
            isMyturn = true;

        ingameM.TurnStart(isMyturn, json.GetField("data").GetField("room").GetField("chessboard"), isTurnBack);
    }

    private void gameOver(SocketIOEvent obj) // 게임 오버
    {
        ingameM.GameFinish(obj.data);
    }

    private void proposeExtendTimeLimits(SocketIOEvent obj) // 상대방이 보낸 시간연장 요청 받음
    {
        data.isSended_addtime = true;
        uiM.AddTimeHighlight();
        uiM.ShowText("상대방이 시간연장을 요청했습니다");
        uiM.addTime.text = "수락";
    }

    private void updateTimeLimits(SocketIOEvent obj) // 시간이 연장됨
    {
        uiM.AddTimer(30);
        uiM.ShowText("30초가 연장되었습니다");
        uiM.addTime.text = "연장됨";
    }

    private void proposeTurnBack(SocketIOEvent obj) // 상대방이 보낸 무르기 요청 받음
    {
        data.isSended_takeback = true;
        uiM.TakeBackHighlight();
        uiM.ShowText("상대방이 무르기를 요청했습니다");
        uiM.takeBack.text = "수락";
    }

    private void emotion(SocketIOEvent obj) // 상대방이 보낸 무르기 요청 받음
    {
        emotionM.YourEmotion(int.Parse(obj.data.GetField("").GetField("").ToString()));
    }
}