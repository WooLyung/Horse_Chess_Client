using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData : MonoBehaviour
{
    public enum TILE
    {
        NONE, PLAYER, OPPONENT, CANT, CAN
    }

    public bool isFirst = true;
    public bool isMyTurn = true;
    public int turnCount = 0;
    public int turnCount2 = 0;
    public string opponentName = "SANS";
    public int opponentRating = 0;
    public float opponentWinRate = 0;
    public int settedPieces = 0;
    public TILE[,] map = new TILE[9, 9];

    public bool isClicked_takeback = false;
    public bool isClicked_addtime = false;
    public bool isSended_takeback = false;
    public bool isSended_addtime = false;

    private void Awake()
    {
        DataSender sender = GameObject.Find("DataSender").GetComponent<DataSender>();

        isFirst = sender.isFirst;
        opponentRating = sender.opponentRating;
        opponentWinRate = sender.opponentWinRate;
        opponentName = sender.opponentName;

        GameObject.Destroy(sender.gameObject);
    }
}
