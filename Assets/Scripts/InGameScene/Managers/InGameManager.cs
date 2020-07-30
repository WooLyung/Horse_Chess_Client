using System.Collections;
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
    public InputManager inputM;
    public EffectManager effectM;
    public InGameData data;

    public Animator whoTurn;
    public Animator screen;

    public GameObject CanMoveParent;
    public GameObject CantMoveParent;
    public GameObject blackPieceParent;
    public GameObject whitePieceParent;
    public GameObject whitePiece;
    public GameObject blackPiece;
    public GameObject cantMoveTile;

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
            serverM.SettingDone();
        }
    }

    public void PieceMove(int x1, int y1, int x2, int y2)
    {
        gameState_ = GAME_STATE.WAIT_SERVER;

        if (data.isFirst)
            serverM.PieceMove(x1 - 1, y1 - 1, x2 - 1, y2 - 1);
        else
            serverM.PieceMove(x1 - 1, 8 - y1, x2 - 1, 8 - y2);
    }

    public void TurnStart(bool isMyturn, JSONObject map, bool isTurnBack)
    {
        gameState_ = GAME_STATE.GAME;

        if (!isTurnBack)
        {
            data.turnCount++;
            data.turnCount2++;
        }
        else // 무르기 함
        {
            data.turnCount -= 2;
            data.turnCount2 = 1;
            uiM.ShowText("턴을 물렀습니다.");
        }

        data.isClicked_addtime = false;
        data.isClicked_takeback = false;
        data.isSended_addtime = false;
        data.isSended_takeback = false;

        data.isMyTurn = isMyturn;
        uiM.StartTimer(60);
        uiM.SetButtonText();
        DestroyObjects();
        CreatePieces(map);
        whoTurn.SetBool("Start", true);
        whoTurn.SetBool("MyTurn", isMyturn);

        if (data.turnCount == 1)
            screen.SetBool("IsDisappear", true);
        if (isMyturn)
        {
            if (inputM.isStalemate())
            {
                data.turnCount--;
                Stalemate();
            }
            else
            {
                uiM.ShowText("당신의 차례입니다");
            }
        }
    }

    public void Surrender()
    {
        gameState_ = GAME_STATE.FINISH;
        serverM.Surrender();
    }

    public void Stalemate()
    {
        gameState_ = GAME_STATE.FINISH;
        serverM.Stalemate();
    }

    public void GameFinish(JSONObject json)
    {
        gameState_ = GAME_STATE.FINISH;
        uiM.GameFinish(json);
    }

    private void CreatePieces(JSONObject map)
    {
        if (data.turnCount > 1)
        {
            for (int x = 1; x <= 8; x++)
            {
                for (int y = 1; y <= 8; y++)
                {
                    data.preMap[x, y] = data.map[x, y];
                }
            }
        }

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                int tile = int.Parse(map[x][y].ToString());
                Vector2Int pos = new Vector2Int(x + 1, data.isFirst ? (y + 1) : (8 - y));
                GameObject newObj = null;

                if (tile == 0) // 아무것도 존재하지 않는 칸
                {
                    data.map[pos.x, pos.y] = InGameData.TILE.NONE;
                }
                else if (tile == 1) // 흑색 기물이 있는 칸
                {
                    if (data.isFirst) // 백색일 경우
                        data.map[pos.x, pos.y] = InGameData.TILE.OPPONENT;
                    else
                        data.map[pos.x, pos.y] = InGameData.TILE.PLAYER;

                    newObj = GameObject.Instantiate(blackPiece, blackPieceParent.transform);
                    newObj.name = "blackPiece";
                }
                else if (tile == 2) // 백색 기물이 있는 칸
                {
                    if (!data.isFirst) // 흑색일 경우
                        data.map[pos.x, pos.y] = InGameData.TILE.OPPONENT;
                    else
                        data.map[pos.x, pos.y] = InGameData.TILE.PLAYER;

                    newObj = GameObject.Instantiate(whitePiece, whitePieceParent.transform);
                    newObj.name = "whitePiece";
                }
                else if (tile == 3) // 지나갈 수 없는 칸
                {
                    data.map[pos.x, pos.y] = InGameData.TILE.CANT;
                    newObj = GameObject.Instantiate(cantMoveTile, CantMoveParent.transform);
                    newObj.name = "cantMoveTile";
                }

                if (newObj != null)
                {
                    newObj.transform.localPosition = new Vector3(pos.x, pos.y);
                }

                if (data.preMap[pos.x, pos.y] != data.map[pos.x, pos.y]
                    && data.turnCount > 1) // 기물이 달라진 타일인 경우
                {
                    if (data.map[pos.x, pos.y] == InGameData.TILE.PLAYER
                        || data.map[pos.x, pos.y] == InGameData.TILE.OPPONENT)
                        effectM.PieceMoveEffect(pos);
                }
            }
        }
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
