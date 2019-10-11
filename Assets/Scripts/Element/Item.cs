using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;

    // =========== MonoBehavior ===========
    void Start()
    {
        if (!GameData.Instance.level.RegisterElement(gameObject))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Tips.Instance.ShowTips(itemType);
        switch (itemType)
        {
            case ItemType.KeyYellow:
                GameData.Instance.player.UpdateKeys((int)KeyType.Yellow, 1);
                break;
            case ItemType.KeyBlue:
                GameData.Instance.player.UpdateKeys((int)KeyType.Blue, 1);
                break;
            case ItemType.KeyRed:
                GameData.Instance.player.UpdateKeys((int)KeyType.Red, 1);
                break;
            case ItemType.GemRed:
                GameData.Instance.player.UpdateAtk(Const.GemAttrIncrease);
                break;
            case ItemType.GemBlue:
                GameData.Instance.player.UpdateDef(Const.GemAttrIncrease);
                break;
            case ItemType.MedicineHp1:
                GameData.Instance.player.UpdateHp(Const.MedicineHpIncrease1);
                break;
            case ItemType.MedicineHp2:
                GameData.Instance.player.UpdateHp(Const.MedicineHpIncrease2);
                break;
        }
        
        GameData.Instance.level.DisableElement(gameObject);
        Destroy(gameObject);
    }
}
