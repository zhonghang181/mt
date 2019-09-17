using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveController : MonoBehaviour
{
    enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    private bool m_isMoving = false;
    private float m_cellSize = 32.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log(horizontal);

        //Vector2 posPre = transform.position;
        //posPre.x += horizontal>0? 
    }

    void StepOnce(Direction direction)
    {
        if (!m_isMoving)
        {
            
        }
    }
}
