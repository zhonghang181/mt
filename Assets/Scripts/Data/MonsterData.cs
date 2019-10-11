using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MonsterData
{
    public int hp;
    public int atk;
    public int def;
    public int gold;
    public int exp;

    public void UpdateHp(int deltaNum)
    {
        hp += deltaNum;
        hp = hp < 0 ? 0 : hp;
    }
}
