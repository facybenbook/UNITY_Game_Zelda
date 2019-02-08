﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float _speed;

    Rigidbody2D _rigidbody;

    Vector3 _change;

    Animator _animator;

    PlayerState _currentState;

    enum PlayerState
    {
        WALK, ATTACK, INTERACT
    }

	void Start () {
        _currentState = PlayerState.WALK;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("moveX", 0);
        _animator.SetFloat("moveY", -1);
    }
	
	void Update () {
        _change = Vector2.zero;
        _change.x = Input.GetAxisRaw("Horizontal");
        _change.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Attack") && _currentState != PlayerState.ATTACK)
        {
            StartCoroutine(Attack());
        }
        else if (_currentState == PlayerState.WALK)
        {
            UpdateAnimationAndMove();
        }
	}

    IEnumerator Attack()
    {
        _animator.SetBool("attacking", true);
        _currentState = PlayerState.ATTACK;
        yield return null;
        _animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        _currentState = PlayerState.WALK;
    }

    void UpdateAnimationAndMove()
    {
        if (_change != Vector3.zero)
        {
            MoveCharacter();
        }
        else
        {
            _animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        _rigidbody.MovePosition(transform.position + _change.normalized * _speed * Time.deltaTime);
        _animator.SetFloat("moveX", _change.x);
        _animator.SetFloat("moveY", _change.y);
        _animator.SetBool("moving", true);
    }
}
