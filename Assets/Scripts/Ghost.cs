using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]

public class Ghost : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private Player _target;
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _points;
    [Header("Attack")]
    [SerializeField] private int _damage;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _viewField;
    [SerializeField] private float _attackRange;

    private Health _health;
    private float _currentAttackDelay = 0;
    private int _currentPointNumber = 0;
    private float _targetDistance;
    private Transform _currentPoint;

    private void Start()
    {
        _currentPoint = _points[_currentPointNumber];
        transform.position = _currentPoint.position;
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        _targetDistance = Vector3.Distance(_target.transform.position, transform.position);

        _currentAttackDelay += Time.deltaTime;

        if (_targetDistance <= _attackRange)
        {
            if(_attackDelay <= _currentAttackDelay)
            {
                _target.Health.TakeDamage(_damage);
                _currentAttackDelay = 0;
            }
        }
        else if (_targetDistance <= _viewField)
        {
            GoToTarget();
        }
        else
        {
            PatrollingArea();
        }
    }

    private void GoToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    private void PatrollingArea()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position, _speed * Time.deltaTime);

        if (transform.position == _currentPoint.position)
        {
            _currentPointNumber++;

            if (_currentPointNumber >= _points.Length)
                _currentPointNumber = 0;

            _currentPoint = _points[_currentPointNumber];
        }
    }
}