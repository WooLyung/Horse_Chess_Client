using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance_ = null;

    private string name_ = "샌즈";
    private int rate_ = 0;
    private int game_ = 0;
    private int winGame_ = 0;
    private int ranking_ = 0;

    private void Awake()
    {
        instance_ = this;
    }

    public static PlayerData Instance
    {
        get
        {
            return instance_;
        }
    }

    public string Name
    {
        get
        {
            return name_;
        }

        set
        {
            name_ = value;
        }
    }

    public int Rate
    {
        get
        {
            return rate_;
        }

        set
        {
            rate_ = value;
        }
    }

    public int Game
    {
        get
        {
            return game_;
        }

        set
        {
            game_ = value;
        }
    }

    public int WinGame
    {
        get
        {
            return winGame_;
        }

        set
        {
            winGame_ = value;
        }
    }

    public int Ranking
    {
        get
        {
            return ranking_;
        }

        set
        {
            ranking_ = value;
        }
    }
}