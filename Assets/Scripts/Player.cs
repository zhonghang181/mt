using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // =========== Public ===========

    // =========== Private ===========
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private Vector2 _posNext;
    private Vector2 _posPrev;

    private const float DURATION = 0.2f;
    private float _moveDetalTime;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    Vector2 GetMoveVector()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        horizontal = horizontal > 0 ? 1 : (horizontal < 0 ? -1 : 0);
        vertical = vertical > 0 ? 1 : (vertical < 0 ? -1 : 0);
        vertical = Mathf.Approximately(horizontal, 0) ? vertical : 0;
        return new Vector2(horizontal, vertical);
    }

    void Update()
    {
        Vector2 move = GetMoveVector();

        if (!_animator.GetBool("Moving"))
        {
            if (Mathf.Abs(move.x) + Mathf.Abs(move.y) > 0)
            {
                Vector2 posCur = _rigidbody.position;
                _posNext = posCur + move;
                _posPrev = posCur;

                _collider.enabled = false;
                RaycastHit2D hit = Physics2D.Linecast(_posPrev, _posNext);
                _collider.enabled = true;
                if (hit.transform == null || hit.collider.isTrigger)
                {
                    _animator.SetFloat("MoveX", move.x);
                    _animator.SetFloat("MoveY", move.y);
                    _animator.SetBool("Moving", true);
                    _moveDetalTime = 0.0f;
                }
                else
                {
                    if (hit.collider.tag == "Door")
                    {
                        Door door = hit.collider.gameObject.GetComponent<Door>();
                        door.Open();
                    }
                }
            }
        }
        else
        {
            _moveDetalTime += Time.deltaTime;
            if (_moveDetalTime >= DURATION)
            {
                _moveDetalTime = DURATION;
                _animator.SetBool("Moving", false);
                _rigidbody.position = _posNext;
            } else
            {
                _rigidbody.MovePosition(Vector2.Lerp(_posPrev, _posNext, _moveDetalTime / DURATION));
            }
        }
    }
}
