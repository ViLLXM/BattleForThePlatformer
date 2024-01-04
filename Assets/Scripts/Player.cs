using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]

public class Player : MonoBehaviour
{
    [SerializeField] private float _vampireAbilityHealthEffect;
    [SerializeField] private float _vampireAbilityDistance;
    [SerializeField] private float _vampireAbilityUsingTime;
    [SerializeField] private float _vampireAbilityCooldown;

    private Health _health;
    private Mover _mover;
    private Animator _animator;
    private int _turn = Animator.StringToHash("turn");
    private List<Ghost> _ghosts = new List<Ghost>();
    private float _currentVampireAbilityCooldown = 0;
    private Ghost _closestGhost;

    public Health Health => _health;
    public float VampireAbilityCooldown => _vampireAbilityCooldown;
    public float CurrentVampireAbilityCooldown => _currentVampireAbilityCooldown;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            if(gameObject.TryGetComponent(out Ghost ghost))
                _ghosts.Add(ghost);
        }

        _currentVampireAbilityCooldown = _vampireAbilityCooldown;
    }

    private void Update()
    {
        _currentVampireAbilityCooldown += Time.deltaTime;

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

    public void UseVampireAbility()
    {
        if (_vampireAbilityCooldown <= _currentVampireAbilityCooldown && TryGetClosestGhost(out Ghost closestGhost))
        {
            _closestGhost = closestGhost;
            _currentVampireAbilityCooldown = 0;
            StartCoroutine(VampireAbilityUsing());
        }
    }

    private bool TryGetClosestGhost(out Ghost closestGhost)
    {
        float distance = float.MaxValue;
        float currentDistance;

        closestGhost = null;

        foreach (Ghost ghost in _ghosts)
        {
            currentDistance = Vector3.Distance(ghost.transform.position, transform.position);

            if (currentDistance < distance)
            {
                closestGhost = ghost;
                distance = currentDistance;
            }
        }

        if (distance <= _vampireAbilityDistance)
            return true;
        else
            return false;
    }

    private IEnumerator VampireAbilityUsing()
    {
        float currentUsingTime = 0;

        while (_vampireAbilityUsingTime >= currentUsingTime)
        {
            currentUsingTime += Time.deltaTime;

            _closestGhost.Health.TakeDamage(Time.deltaTime);

            _health.Healing(Time.deltaTime);

            yield return null;
        }

        StopCoroutine(VampireAbilityUsing());
    }
}