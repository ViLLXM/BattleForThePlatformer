using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]

public class Player : MonoBehaviour
{
    private Health _health;
    private Mover _mover;
    private Animator _animator;
    private int _turn = Animator.StringToHash("turn");

    public Health Health => _health;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _mover.Run(Vector3.left);
            _animator.SetFloat(_turn, -0.1f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _mover.Run(Vector3.right);
            _animator.SetFloat(_turn, 0.1f);
        }
        else
        {
            _animator.SetFloat(_turn, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _mover.Jump();
        }
    }
}