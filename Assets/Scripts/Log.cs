﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy {

    Transform _target;
    Rigidbody2D _rigidbody;
    Animator _animator;

    [SerializeField] float _chaseRadius;
    [SerializeField] float _attackRadius;
    [SerializeField] Transform _homePosition;

    void Start()
    {
        CurrentState = CharacterState.IDLE;

        _target = GameObject.FindGameObjectWithTag("Player").transform;

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CheckTargetDistance();
    }

    void CheckTargetDistance()
    {
        if(CurrentState == CharacterState.STAGGER || CurrentState == CharacterState.ATTACK)
        {
            return;
        }

        if (Vector3.Distance(_target.position, transform.position) <= _chaseRadius && Vector3.Distance(_target.position, transform.position) > _attackRadius)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, _target.position, MoveSpeed * Time.deltaTime);
            ChangeState(CharacterState.WALK);
            ChangeAnimation(temp - transform.position);
            _rigidbody.MovePosition(temp);
            _animator.SetBool("wakeUp", true);
        }
        else if(Vector3.Distance(_target.position, transform.position) > _chaseRadius)
        {
            _animator.SetBool("wakeUp", false);
        }
    }

    void ChangeAnimation(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimatorFloat(Vector2.right);
            }
            else if(direction.x < 0)
            {
                SetAnimatorFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimatorFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimatorFloat(Vector2.down);
            }
        }
    }

    void SetAnimatorFloat(Vector2 setVector)
    {
        _animator.SetFloat("moveX", setVector.x);
        _animator.SetFloat("moveY", setVector.y);
    }
}
