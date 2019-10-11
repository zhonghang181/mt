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
    public Text Stage;

    private void Awake()
    {
        GameData.Instance.level.stage = int.Parse(Stage.text);
    }

    void Start()
    {
        Notification.Instance.On(Const.Event_Player_Data_Reload, OnPlayerDataReload);
        
        Reload();
    }

    private void OnDestroy()
    {
        Notification.Instance.Off(Const.Event_Player_Data_Reload, OnPlayerDataReload);
    }

    void Update()
    {
        
    }

    // =========== Private Function ===========
    void Reload()
    {
        var data = GameData.Instance.player;
        Atk.text = data.atk.ToString();
        Def.text = data.def.ToString();
        Hp.text = data.hp.ToString();
        Gold.text = data.gold.ToString();
        Exp.text = data.exp.ToString();
        Level.text = data.level.ToString();
        Keys[0].text = GameData.Instance.player.GetKeyNum(0).ToString();
        Keys[1].text = GameData.Instance.player.GetKeyNum(1).ToString();
        Keys[2].text = GameData.Instance.player.GetKeyNum(2).ToString();
    }

    // =========== Public Functions ===========
    public void OnPlayerDataReload(CustomEvent data)
    {
        Reload();
    }
}
