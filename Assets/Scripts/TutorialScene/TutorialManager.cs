using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TutorialManager : MonoBehaviour
{
    private enum TILE
    {
        NONE, WHITE, WINE, CANT, CAN
    }

    public BGMManager bgmM;
    public SoundManager soundM;
    public SettingManager setting;

    public Animator sceneAnim;
    public Animator objectAnim;
    public Animator textAnim;
    public Animator screenAnim;
    public Text text;
    public GameObject dust;
    public GameObject smallDust;

    public GameObject[] playerPO = new GameObject[4];
    public GameObject[] opponentPO = new GameObject[4];
    public GameObject canMove;
    public GameObject canMoveTile;
    public GameObject cantMove;
    public GameObject cantMoveTile;
    public GameObject movedTile;

    private int selectPiece = -1;
    private int clearedPiece = 0;
    private int step = 0;
    private float textTime = 0;
    private bool isAppearTime = false;

    private Vector2Int[] playerP = new Vector2Int[4];
    private Vector2Int[] opponentP = new Vector2Int[4];
    private List<GameObject> canMoveTiles = new List<GameObject>();
    private List<GameObject> cantMoveTiles = new List<GameObject>();

    private TILE[, ] tiles = new TILE[9, 9];
    private Vector2Int[] moveDir = { new Vector2Int(1, 2), new Vector2Int(1, -2), new Vector2Int(-1, 2), new Vector2Int(-1, -2),
        new Vector2Int(2, 1),new Vector2Int(2, -1), new Vector2Int(-2, 1), new Vector2Int(-2, -1) };

    private void Start()
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
            if (textTime >= 2.7f)
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
                soundM.PlaySound("place");

                playerP[clearedPiece] = new Vector2Int(x, y);
                playerPO[clearedPiece].transform.localPosition = new Vector3(x, y);
                clearedPiece++;

                tiles[x, y] = TILE.WHITE;
            }
            else
            {
                BoardTouch();
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

    private void DestroyCanMoveTile()
    {
        soundM.PlaySound("cancel");

        selectPiece = -1;
        foreach (var value in canMoveTiles)
        {
            GameObject.Destroy(value);
        }
        for (int x = 1; x <= 8; x++)
        {
            for (int y = 1; y <= 8; y++)
            {
                if (tiles[x, y] == TILE.CAN)
                    tiles[x, y] = TILE.NONE;
            }
        }
    }

    private void OutClick()
    {
        if (selectPiece != -1) // 선택된 기물 있음
        {
            DestroyCanMoveTile();
        }
    }

    private void TileClickGame(int x, int y)
    {
        if (selectPiece != -1) // 선택된 기물 있음
        {
            if (tiles[x, y] == TILE.WINE
                || tiles[x, y] == TILE.NONE) // 상대편 혹은 빈 칸을 선택
            {
                DestroyCanMoveTile();
            }
            else if (tiles[x, y] == TILE.WHITE)
            {
                if (playerP[selectPiece].x == x && playerP[selectPiece].y == y)
                {
                    DestroyCanMoveTile();
                    return;
                }

                DestroyCanMoveTile();
                soundM.PlaySound("select");

                GameObject newCanMove2 = GameObject.Instantiate(canMove, canMoveTile.transform);
                newCanMove2.transform.localPosition = new Vector3(x, y);
                canMoveTiles.Add(newCanMove2);
                var renderer = newCanMove2.GetComponent<SpriteRenderer>();

                if (x % 2 == y % 2)
                    renderer.color = new Color(1, 0, 0, 0.35f);
                else
                    renderer.color = new Color(1, 1, 1, 0.35f);

                bool[] myP = { false, false, false, false };
                int myP_count = 1;
                int myP_pre_count = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (playerP[i].x == x && playerP[i].y == y)
                    {
                        selectPiece = i; // 기물 선택
                        myP[i] = true;
                    }
                }

                // 이동 가능 위치 판정
                while (myP_count != myP_pre_count)
                {
                    myP_count = myP_pre_count;

                    for (int i = 0; i < 4; i++)
                    {
                        if (myP[i])
                        {
                            foreach (var moved in moveDir)
                            {
                                Vector2Int pos = playerP[i] + moved;

                                if (pos.x <= 8 && pos.x >= 1 && pos.y <= 8 && pos.y >= 1)
                                {
                                    if (tiles[pos.x, pos.y] == TILE.NONE)
                                    {
                                        GameObject newCanMove = GameObject.Instantiate(canMove, canMoveTile.transform);
                                        newCanMove.transform.localPosition = new Vector3(pos.x, pos.y);
                                        canMoveTiles.Add(newCanMove);
                                        tiles[pos.x, pos.y] = TILE.CAN;
                                    }
                                    else
                                    {
                                        if (tiles[pos.x, pos.y] == TILE.WHITE)
                                        {
                                            for (int j = 0; j < 4; j++)
                                            {
                                                if (playerP[j] == pos)
                                                {
                                                    if (myP[j] == false)
                                                    {
                                                        myP[j] = true;
                                                        myP_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (tiles[x, y] == TILE.CAN)
            {
                soundM.PlaySound("place");

                if (setting.Effect)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var newEffect = GameObject.Instantiate(dust);
                        newEffect.transform.position = new Vector3(x - 4.5f, y - 4.5f);
                        newEffect.name = "dust";
                    }
                }

                Vector2Int pos = new Vector2Int((int)playerPO[selectPiece].transform.localPosition.x, (int)playerPO[selectPiece].transform.localPosition.y);

                GameObject newCantMove = GameObject.Instantiate(cantMove, cantMoveTile.transform);
                newCantMove.transform.localPosition = new Vector3(pos.x, pos.y);
                cantMoveTiles.Add(newCantMove);
                tiles[pos.x, pos.y] = TILE.CANT;

                GameObject.Instantiate(movedTile).transform.position = playerPO[selectPiece].transform.position;
                playerPO[selectPiece].transform.localPosition = new Vector3(x, y);
                GameObject.Instantiate(movedTile).transform.position = playerPO[selectPiece].transform.position;

                DestroyCanMoveTile();
                step = 4;

                StartCoroutine("FinalMessage");
            }
            else
            {
                BoardTouch();
            }
        }
        else // 선택된 기물 없음
        {
            if (tiles[x, y] == TILE.WHITE) // 선택한 타일이 플레이어가 있는 타일
            {
                soundM.PlaySound("select");

                GameObject newCanMove2 = GameObject.Instantiate(canMove, canMoveTile.transform);
                newCanMove2.transform.localPosition = new Vector3(x, y);
                canMoveTiles.Add(newCanMove2);
                var renderer = newCanMove2.GetComponent<SpriteRenderer>();

                if (x % 2 == y % 2)
                    renderer.color = new Color(1, 0, 0, 0.35f);
                else
                    renderer.color = new Color(1, 1, 1, 0.35f);

                bool[] myP = { false, false, false, false };
                int myP_count = 1;
                int myP_pre_count = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (playerP[i].x == x && playerP[i].y == y)
                    {
                        selectPiece = i; // 기물 선택
                        myP[i] = true;
                    }
                }

                // 이동 가능 위치 판정
                while (myP_count != myP_pre_count)
                {
                    myP_count = myP_pre_count;

                    for (int i = 0; i < 4; i++)
                    {
                        if (myP[i])
                        {
                            foreach (var moved in moveDir)
                            {
                                Vector2Int pos = playerP[i] + moved;

                                if (pos.x <= 8 && pos.x >= 1 && pos.y <= 8 && pos.y >= 1)
                                {
                                    if (tiles[pos.x, pos.y] == TILE.NONE)
                                    {
                                        GameObject newCanMove = GameObject.Instantiate(canMove, canMoveTile.transform);
                                        newCanMove.transform.localPosition = new Vector3(pos.x, pos.y);
                                        canMoveTiles.Add(newCanMove);
                                        tiles[pos.x, pos.y] = TILE.CAN;
                                    }
                                    else
                                    {
                                        if (tiles[pos.x, pos.y] == TILE.WHITE)
                                        {
                                            for (int j = 0; j < 4; j++)
                                            {
                                                if (playerP[j] == pos)
                                                {
                                                    if (myP[j] == false)
                                                    {
                                                        myP[j] = true;
                                                        myP_count++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                BoardTouch();
            }
        }
    }    

    public void TileClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)Mathf.Round(pos.x + 4.5f);
            int y = (int)Mathf.Round(pos.y + 4.5f);

            if (x <= 8 && x >= 1 && y <= 8 && y >= 1) // 체커판 안을 누름
            {
                if (step == 1)
                    ClearPiece(x, y);
                else if (step == 3)
                    TileClickGame(x, y);
                else
                    BoardTouch();
            }
            else // 체커판 안을 누르지 않음
            {
                OutClick();
            }
        }
    }

    private void BoardTouch()
    {
        soundM.PlaySound("touch");

        if (setting.Effect)
        {
            for (int i = 0; i < 5; i++)
            {
                var newEffect = GameObject.Instantiate(smallDust);
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                newEffect.transform.position = pos;
                newEffect.name = "smallDust";
            }
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

    IEnumerator FinalMessage()
    {
        yield return new WaitForSeconds(1.5f);
        ShowText("기물이 이동한 자리는 더 이상 사용할 수 없습니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("두 플레이어가 게임을 진행하며 상대방의 이동을 완전히 막으면 승리합니다");
        yield return new WaitForSeconds(3.5f);
        ShowText("이제 실제 게임에서 다른 플레이어들과 겨뤄보세요!");
        yield return new WaitForSeconds(3.5f);
        sceneAnim.SetInteger("state", 1);
        objectAnim.SetInteger("state", 1);
        bgmM.SetState(BGMManager.BGMSTATE.FINISH);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
