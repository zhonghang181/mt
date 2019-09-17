using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveController : MonoBehaviour
{
    // =========== Public ===========
    public Animator _animator;
    public Rigidbody2D _rigidbody;

    // =========== Private ===========
    private float _cellSize = 32.0f;
    private float _duration = 0.5f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Mathf.Approximately(horizontal, 0.0f) ? Input.GetAxis("Vertical") : 0.0f;
        bool isInputExist = !Mathf.Approximately(horizontal, 0.0f) || !Mathf.Approximately(vertical, 0.0f);
        bool isMoving = _animator.GetBool("Moving");

        if (isInputExist)
        {
            if (isMoving)
            {
                // =========== Movement ===========
                Vector2 move = new Vector2(horizontal, vertical);
                Vector2 posPre = _rigidbody.position;
                Vector2 posCur = posPre + move * _cellSize / _duration * Time.deltaTime;
                _rigidbody.MovePosition(posCur);
            } else
            {
                // =========== Animation ===========
                _animator.SetFloat("MoveX", horizontal);
                _animator.SetFloat("MoveY", vertical);
                _animator.SetBool("Moving", true);
            }
        }
    }
}
