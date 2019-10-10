using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public enum StairsType
    {
        Up, Down,
    }

    public StairsType stairsType;
    SpriteRenderer m_SpriteRendererStairs;

    void Start()
    {
        m_SpriteRendererStairs = GetComponent<SpriteRenderer>();

        switch (stairsType)
        {
            case StairsType.Up:
                m_SpriteRendererStairs.sprite = AssetsManager.Instance.GetSprite("Terrains_24", "Terrains");
                break;
            case StairsType.Down:
                m_SpriteRendererStairs.sprite = AssetsManager.Instance.GetSprite("Terrains_23", "Terrains");
                break;
        }   
    }

    void Update()
    {
        
    }
}
