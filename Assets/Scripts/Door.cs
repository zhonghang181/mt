﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // =========== Public ===========
    public enum DoorType
    {
        Yellow,
        Blue,
        Red,
    }
    public DoorType _doorType;

    // =========== Private ===========
    private Animator _animator;

    // =========== Functions ===========
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void Open()
    {
        _animator.SetBool("IsOpen", true);
    }

    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}