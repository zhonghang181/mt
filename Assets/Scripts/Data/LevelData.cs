using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData
{
    public int stage;
    public Dictionary<int, bool> slots = new Dictionary<int, bool>();

    public bool RegisterElement(GameObject obj)
    {
        var posIndex = Utils.Pos2Index(obj.transform.position);
        if (!slots.ContainsKey(posIndex)) {
            slots.Add(posIndex, true);
        }

        return slots[posIndex];
    }

    public void DisableElement(GameObject obj)
    {
        var posIndex = Utils.Pos2Index(obj.transform.position);
        if (slots.ContainsKey(posIndex))
        {
            slots[posIndex] = false;
        }
    } 
}
