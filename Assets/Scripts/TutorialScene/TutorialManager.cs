using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private enum TILE
    {
        NONE, WHITE, WINE, CANT
    }

    public Animator textAnim;
    public Animator screenAnim;
    public Text text;

    public GameObject[] playerPO = new GameObject[4];
    public GameObject[] opponentPO = new GameObject[4];

    private int clearedPiece = 0;
    private int step = 0;
    private float textTime = 0;
    private bool isAppearTime = false;

    private Vector2Int[] playerP = new Vector2Int[4];
    private Vector2Int[] opponentP = new Vector2Int[4];

    private TILE[, ] tiles = new TILE[9, 9];

    private void Start ()
    {
        StartCoroutine("FirstMessage");

        playerP[0] = new Vector2Int(0, 0);
        playerP[1] = new Vector2Int(0, 0);
        playerP[2] = new Vector2Int(0, 0);
        playerP[3] = new Vector2Int(0, 0);
        opponentP[0] = new Vector2Int(2, 5);
        opponentP[1] = new Vector2Int(3, 7);
        opponentP[2] = new Vector2Int(5, 5);
        opponentP[3] = new Vector2Int(6, 7);

        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                tiles[i, j] = TILE.NONE;
            }
        }

        tiles[2, 5] = TILE.WINE;
        tiles[3, 7] = TILE.WINE;
        tiles[5, 5] = TILE.WINE;
        tiles[6, 7] = TILE.WINE;
    }
	
	private void Update()
    {
		if (isAppearTime)
        {
            textTime += Time.deltaTime;
            if (textTime >= 3)
            {
                textTime = 0;
                isAppearTime = false;
                textAnim.SetBool("IsAppear", false);
            }
        }

        TileClick();
	}

    private void ShowText(string text)
    {
        textAnim.SetBool("IsAppear", true);
        textTime = 0;
        isAppearTime = true;
        this.text.text = text;
    }

    private void ClearPiece(int x, int y)
    {
        if (y <= 4)
        {
            bool canClear = true;
            foreach (var value in playerP)
            {
                if (value.x == x && value.y == y)
                    canClear = false;
            }

            if (canClear)
            {
                playerP[clearedPiece] = new Vector2Int(x, y);
                playerPO[clearedPiece].transform.localPosition = new Vector3(x, y);
                clearedPiece++;

                tiles[x, y] = TILE.WHITE;
            }
        }

        if (clearedPiece >= 4)
        {
            step = 2;
            screenAnim.SetBool("IsDisappear", true);
            for (int i = 0; i < 4; i++)
            {
                opponentPO[i].transform.localPosition = new Vector3(opponentP[i].x, opponentP[i].y);
            }

            StartCoroutine("SecondMessage");
        }
    }

    public void TileClick()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = (int)Mathf.Round(pos.x);
        int y = (int)Mathf.Round(pos.y);

        if (x <= 8 && x >= 1 && y <= 8 && y >= 1) // 체커판 안을 누름
        {
            if (step == 1)
                ClearPiece(x, y);
        }
        else // 체커판 안을 누르지 않음
        {
            Debug.Log("체커판 아님");
        }
    }

    IEnumerator FirstMessage()
    {
        yield return new WaitForSeconds(1.5f);
        ShowText("스테일메이트에 오신것을 환영합니다!");
        yield return new WaitForSeconds(3.5f);
        ShowText("지금부터 스테일메이트의 튜토리얼을 시작하겠습니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("스테일메이트는 상대방의 경로를 막아 승리하는 보드게임입니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("기물이 한번이라도 지나간 타일은 지나갈 수 없습니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("경기를 시작하기 전 원하는 위치에 4개의 기물을 배치하세요");
        step = 1;
    }

    IEnumerator SecondMessage()
    {
        yield return new WaitForSeconds(1.5f);
        ShowText("두 상대가 배치를 마치면 게임이 시작됩니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("게임은 두 플레이어 순서대로 하나의 기물을 옮기며 진행됩니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("기물의 이동 방법은 체스의 나이트와 동일합니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("추가로 이동 위치에 자신의 기물이 있다면 건너뛸 수 있습니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("원하는 기물을 원하는 위치로 옮기세요");
        step = 3;
    }
}
