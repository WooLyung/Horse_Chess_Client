using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InGameManager ingameM;
    public EffectManager effectM;
    public InGameData data;
    public GameObject CanMoveParent;
    public GameObject canMoveTile;

    private Vector2Int selectTile = new Vector2Int(0, 0);
    private Vector2Int[] moveDir = { new Vector2Int(1, 2), new Vector2Int(1, -2), new Vector2Int(-1, 2), new Vector2Int(-1, -2),
        new Vector2Int(2, 1),new Vector2Int(2, -1), new Vector2Int(-2, 1), new Vector2Int(-2, -1) };

    private void Update()
    {
        TileClick();
    }

    public bool isStalemate()
    {
        int count = 0;
        for (int x = 1; x <= 8; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                if (data.map[x, y] == InGameData.TILE.PLAYER)
                {
                    foreach (var value in moveDir)
                    {
                        Vector2Int pos = new Vector2Int(x + value.x, y + value.y);

                        if (pos.x <= 8 && pos.x >= 1 && pos.y <= 8 && pos.y >= 1) // 맵 안일 경우
                        {
                            if (data.map[pos.x, pos.y] == InGameData.TILE.NONE) // 아무것도 없는 타일일 경우
                            {
                                count++;
                            }
                        }
                    }
                }
            }
        }

        if (count == 0)
            return true;
        else
            return false;
    }

    private void TileClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)Mathf.Round(pos.x + 4.5f);
            int y = (int)Mathf.Round(pos.y + 4.5f);

            if (x <= 8 && x >= 1 && y <= 8 && y >= 1) // 체커판 안을 누름
            {
                InClick(x, y);
            }
            else // 체커판 안을 누르지 않음
            {
                OutClick(x, y);
            }
        }
    }

    private void OutClick(int x, int y)
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.GAME
            && data.isMyTurn) // 게임중
        {
            if (selectTile.x != 0) // 선택된 타일이 있을 경우
            {
                DestroyCanTiles();
                selectTile = new Vector2Int(0, 0);
            }
        }
    }

    private void InClick(int x, int y)
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.SET) // 배치모드
        {
            if (y <= 4 && data.map[x, y] == InGameData.TILE.NONE) // 배치 가능한 위치를 눌렀을 때
            {
                ingameM.Setting(x, y);
                return;
            }
        }
        else if (ingameM.GameState == InGameManager.GAME_STATE.GAME
            && data.isMyTurn) // 게임중
        {
            if (selectTile.x == 0) // 선택된 타일이 없을 경우
            {
                if (data.map[x, y] == InGameData.TILE.PLAYER) // 플레이어 기물을 클릭했을 경우
                {
                    selectTile = new Vector2Int(x, y);
                    CreateCanTiles(x, y, new List<Vector2Int>());
                    return;
                }
            }
            else // 선택된 타일이 있을 경우
            {
                if (selectTile.x == x && selectTile.y == y) // 원래 위치를 클릭했을 경우
                {
                    DestroyCanTiles();
                    selectTile = new Vector2Int(0, 0);
                    return;
                }
                else if (data.map[x, y] == InGameData.TILE.PLAYER) // 다른 플레이어 기물을 클릭했을 경우
                {
                    DestroyCanTiles();
                    selectTile = new Vector2Int(x, y);
                    CreateCanTiles(x, y, new List<Vector2Int>());
                    return;
                }
                else if (data.map[x, y] == InGameData.TILE.CAN) // 이동 가능 위치를 눌렀을 경우
                {
                    DestroyCanTiles();
                    PieceMove(selectTile.x, selectTile.y, x, y);
                    return;
                }
                else // 이동 불가능 위치를 눌렀을 경우
                {
                    DestroyCanTiles();
                    selectTile = new Vector2Int(0, 0);
                    return;
                }
            }
        }

        effectM.BoardClickEffect(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void DestroyCanTiles()
    {
        foreach (var value in CanMoveParent.GetComponentsInChildren<Transform>())
        {
            if (value.gameObject != CanMoveParent)
            {
                GameObject.Destroy(value.gameObject);
            }
        }

        for (int x = 1; x <= 8; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                if (data.map[x, y] == InGameData.TILE.CAN)
                {
                    data.map[x, y] = InGameData.TILE.NONE;
                }
            }
        }
    }

    private void PieceMove(int x1, int y1, int x2, int y2)
    {
        ingameM.PieceMove(x1, y1, x2, y2);
    }

    private void CreateCanTiles(int x, int y, List<Vector2Int> mines)
    {
        if (mines.Count == 0) // 비어있는 것일 경우
        {
            CreateSelectTileObject(x, y);
        }

        mines.Add(new Vector2Int(x, y));

        foreach (var value in moveDir)
        {
            Vector2Int pos = new Vector2Int(x + value.x, y + value.y);

            if (pos.x <= 8 && pos.x >= 1 && pos.y <= 8 && pos.y >= 1) // 맵 안일 경우
            {
                if (data.map[pos.x, pos.y] == InGameData.TILE.NONE) // 아무것도 없는 타일일 경우
                {
                    CreateCanTileObject(pos.x, pos.y);
                }
            }
        }

        foreach (var value in moveDir)
        {
            Vector2Int pos = new Vector2Int(x + value.x, y + value.y);

            if (pos.x <= 8 && pos.x >= 1 && pos.y <= 8 && pos.y >= 1) // 맵 안일 경우
            {
                if (data.map[pos.x, pos.y] == InGameData.TILE.PLAYER) // 자신의 기물일 경우
                {
                    if (!mines.Contains(pos)) // 파악된 타일이 아닐 경우
                        CreateCanTiles(pos.x, pos.y, mines);
                }
            }
        }
    }

    private void CreateCanTileObject(int x, int y)
    {
        GameObject newCanMove = GameObject.Instantiate(canMoveTile, CanMoveParent.transform);
        newCanMove.transform.localPosition = new Vector3(x, y);
        data.map[x, y] = InGameData.TILE.CAN;
    }

    private void CreateSelectTileObject(int x, int y)
    {
        GameObject newCanMove = GameObject.Instantiate(canMoveTile, CanMoveParent.transform);
        newCanMove.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        var renderer = newCanMove.GetComponent<SpriteRenderer>();

        if (x % 2 == y % 2)
            renderer.color = new Color(1, 0, 0, 0.35f);
        else
            renderer.color = new Color(1, 1, 1, 0.35f);

        newCanMove.transform.localPosition = new Vector3(x, y);
    }
}