﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
	public enum GAME_STATE
    {
        SET, GAME, FINISH, WAIT_SERVER
    }

    private GAME_STATE gameState_ = GAME_STATE.SET;

    public UIManager uiM;
    public ServerManager serverM;
    public InGameData data;

    public Animator whoTurn;
    public Animator screen;

    public GameObject CanMoveParent;
    public GameObject CantMoveParent;
    public GameObject blackPieceParent;
    public GameObject whitePieceParent;
    public GameObject whitePiece;
    public GameObject blackPiece;

    public GAME_STATE GameState
    {
        get
        {
            return gameState_;
        }
    }

    private void Start()
    {
        uiM.StartTimer(120);
    }

    public void Setting(int x, int y)
    {
        data.settedPieces++;
        GameObject newPiece;

        if (data.isFirst)
            newPiece = GameObject.Instantiate(whitePiece, whitePieceParent.transform);
        else
            newPiece = GameObject.Instantiate(blackPiece, blackPieceParent.transform);

        newPiece.transform.localPosition = new Vector3(x, y);
        data.map[x, y] = InGameData.TILE.PLAYER;

        if (data.settedPieces >= 4) // 배치 완료
        {
            gameState_ = GAME_STATE.WAIT_SERVER;
            serverM.SettingDone(data);
        }
    }

    public void PieceMove(int x, int y)
    {
        gameState_ = GAME_STATE.WAIT_SERVER;
        serverM.PieceMove(x, y);
    }

    public void TurnStart()
    {
        gameState_ = GAME_STATE.GAME;

        data.turnCount++;
        DestroyObjects();
        uiM.StartTimer(60);
        uiM.SetButtonText();
        whoTurn.SetBool("Start", true);
        CreatePieces();
        // 누구 턴인지 설정

        if (data.turnCount == 1)
            screen.SetBool("IsDisappear", true);
    }

    public void GameFinish()
    {
        gameState_ = GAME_STATE.FINISH;
        uiM.GameFinish();
    }

    private void CreatePieces()
    {
        // 맵 생성
    }

    private void DestroyObjects()
    {
        foreach (var value in CanMoveParent.GetComponentsInChildren<Transform>())
        {
            if (value.gameObject != CanMoveParent)
                GameObject.Destroy(value.gameObject);
        }
        foreach (var value in CantMoveParent.GetComponentsInChildren<Transform>())
        {
            if (value.gameObject != CantMoveParent)
                GameObject.Destroy(value.gameObject);
        }
        foreach (var value in blackPieceParent.GetComponentsInChildren<Transform>())
        {
            if (value.gameObject != blackPieceParent)
                GameObject.Destroy(value.gameObject);
        }
        foreach (var value in whitePieceParent.GetComponentsInChildren<Transform>())
        {
            if (value.gameObject != whitePieceParent)
                GameObject.Destroy(value.gameObject);
        }
    }
}
