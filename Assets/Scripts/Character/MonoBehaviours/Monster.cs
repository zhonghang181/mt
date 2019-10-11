using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData = new MonsterData();

    void Start()
    {
        if (!GameData.Instance.level.RegisterElement(gameObject))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (monsterData.hp <= 0)
        {
            GameData.Instance.level.DisableElement(gameObject);
            Destroy(gameObject);
        }
    }
}
