using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InGameManager ingameM;
    public InGameData data;

    private void Update()
    {
        TileClick();
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

    }

    private void InClick(int x, int y)
    {
        if (ingameM.GameState == InGameManager.GAME_STATE.SET) // 배치모드
        {
            if (y <= 4 && data.map[x, y] == InGameData.TILE.NONE) // 배치 가능한 위치를 눌렀을 때
            {
                ingameM.Setting(x, y);
            }
        }
    }
}
