using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int level;
    public int gold;
    public int exp;
    public int hp;
    public int atk;
    public int def;
    public int[] keys;

    public void UpdateKeys(int keyIndex, int numDelta)
    {
        keys[keyIndex] += numDelta;
        keys[keyIndex] = keys[keyIndex] < 0 ? 0 : keys[keyIndex];
        Notification.GetInstance().Emit(Const.Event_Key_Num_Changed);
    }

    public int GetKeyNum(int keyIndex)
    {
        return keys[keyIndex];
    }
}
