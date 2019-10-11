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
        Notification.Instance.On(Const.Event_Key_Num_Changed, OnKeyNumChanged);
        Notification.Instance.On(Const.Event_Player_Data_Reload, OnPlayerDataReload);
        
        Reload();
    }

    private void OnDestroy()
    {
        Notification.Instance.Off(Const.Event_Key_Num_Changed, OnKeyNumChanged);
        Notification.Instance.Off(Const.Event_Player_Data_Reload, OnPlayerDataReload);
    }

    void Update()
    {
        
    }

    // =========== Private Function ===========
    void UpdateKeys()
    {
        Keys[0].text = Player.Instance.playerData.GetKeyNum(0).ToString();
        Keys[1].text = Player.Instance.playerData.GetKeyNum(1).ToString();
        Keys[2].text = Player.Instance.playerData.GetKeyNum(2).ToString();
    }

    void Reload()
    {
        var data = Player.Instance.playerData;
        Atk.text = data.atk.ToString();
        Def.text = data.def.ToString();
        Hp.text = data.hp.ToString();
        Gold.text = data.gold.ToString();
        Exp.text = data.exp.ToString();
        Level.text = data.level.ToString();
        UpdateKeys();
    }

    // =========== Public Functions ===========
    public void OnKeyNumChanged(CustomEvent data)
    {
        UpdateKeys();
    }

    public void OnPlayerDataReload(CustomEvent data)
    {
        Reload();
    }
}
