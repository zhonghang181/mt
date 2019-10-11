using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public enum KeyType
    {
        Yellow, Blue, Red,
    }
    public KeyType keyType;

    // =========== MonoBehavior ===========
    void Start()
    {
        
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tips.Instance.ShowTips(KeyType2ShowType());
        Player.Instance.playerData.UpdateKeys((int)keyType, 1);
        Destroy(gameObject);
    }

    // =========== Private Functions ===========
    Tips.ShowType KeyType2ShowType()
    {
        Tips.ShowType showType = Tips.ShowType.KeyYellow;
        switch (keyType)
        {
            case KeyType.Yellow:
                showType = Tips.ShowType.KeyYellow;
                break;
            case KeyType.Blue:
                showType = Tips.ShowType.KeyBlue;
                break;
            case KeyType.Red:
                showType = Tips.ShowType.KeyRed;
                break;

        }
        return showType;
    }
}
