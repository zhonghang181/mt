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
        Notification.Instance.Emit(Const.Event_Player_Data_Reload);
    }

    public int GetKeyNum(int keyIndex)
    {
        return keys[keyIndex];
    }

    public void UpdateAtk(int numDelta)
    {
        atk += numDelta;
        Notification.Instance.Emit(Const.Event_Player_Data_Reload);
    }

    public void UpdateDef(int numDelta)
    {
        def += numDelta;
        Notification.Instance.Emit(Const.Event_Player_Data_Reload);
    }

    public void UpdateHp(int numDelta)
    {
        hp += numDelta;
        Notification.Instance.Emit(Const.Event_Player_Data_Reload);
    }
}
