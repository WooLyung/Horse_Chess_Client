using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
	public enum GAME_STATE
    {
        SET, GAME, FINISH
    }

    private GAME_STATE gameState_ = GAME_STATE.SET;

    public UIManager uiM;
    public ServerManager serverM;
    public InGameData data;

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
            gameState_ = GAME_STATE.GAME;
            serverM.SettingDone(data);
        }
    }

    public void TurnStart()
    {
        data.turnCount++;
    }
}
