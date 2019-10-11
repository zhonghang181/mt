using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData s_Instance;
    public static GameData Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new GameData();
            }
            return s_Instance;
        }
    }

    public PlayerData player;
    public bool IsPlayerAssigned { private set; get; }
    public LevelData level = new LevelData();

    public void InitPlayer(PlayerData player)
    {
        if (!IsPlayerAssigned)
        {
            this.player = player;
            IsPlayerAssigned = true;
        }
    }
}
