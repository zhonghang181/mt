using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    protected BoxCollider2D m_BoxCollider2D;
    protected Vector2 m_PreviousPosition;
    protected Vector2 m_CurrentPosition;
    protected Vector2 m_NextMovement;
    public Rigidbody2D Rigidbody2D { get; private set; }

    // =========== MonoBehaviours ===========
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();

        m_CurrentPosition = Rigidbody2D.position;
        m_PreviousPosition = Rigidbody2D.position;
    }

    void FixedUpdate()
    {
        m_PreviousPosition = Rigidbody2D.position;
        m_CurrentPosition = m_PreviousPosition + m_NextMovement;

        Rigidbody2D.MovePosition(m_CurrentPosition);
        m_NextMovement = Vector2.zero;
    }

    // =========== Public functions ===========
    public void Move(Vector2 movement)
    {
        m_NextMovement += movement;
    }

    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - m_CurrentPosition;
        m_PreviousPosition += delta;
        m_CurrentPosition = position;
        Rigidbody2D.MovePosition(position);
    }
}