using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBoard : MonoBehaviour
{
    public Text Level;
    public Text Hp;
    public Text Atk;
    public Text Def;
    public Text Gold;
    public Text Exp;
    public Text[] Keys;

    void Start()
    {
        Notification.GetInstance().On(Const.Event_Key_Num_Changed, OnKeyNumChanged);
    }

    private void OnDestroy()
    {
        Notification.GetInstance().Off(Const.Event_Key_Num_Changed, OnKeyNumChanged);
    }

    void Update()
    {
        
    }

    // =========== Inner Function ===========
    void UpdateKeys()
    {
        Keys[0].text = UnitCenter.UnitPlayer._playerData.GetKeyNum(0).ToString();
        Keys[1].text = UnitCenter.UnitPlayer._playerData.GetKeyNum(1).ToString();
        Keys[2].text = UnitCenter.UnitPlayer._playerData.GetKeyNum(2).ToString();
    }

    // =========== Outter Api ===========
    public void OnKeyNumChanged(CustomEvent data)
    {
        UpdateKeys();
    }
}
