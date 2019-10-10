using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        Yellow,
        Blue,
        Red,
    }
    public DoorType _doorType;
    public AudioClip _audioClip;

    Animator _animator;

    // =========== MonoBehavior ===========
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    // =========== Public Functions ===========
    public void Open()
    {
        _animator.SetBool("IsOpen", true);
    }

    public bool IsOpened()
    {
        return _animator.GetBool("IsOpen");
    }

    public DoorType GetDoorType()
    {
        return _doorType;
    }

    // =========== Auto Called ===========
    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
