using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class VampireAbility : MonoBehaviour
{
    [SerializeField] private float _healthEffect;
    [SerializeField] private float _distance;
    [SerializeField] private float _usingTime;
    [SerializeField] private float _cooldown;

    private Player _player;
    private List<Ghost> _ghosts = new List<Ghost>();
    private float _currentCooldown = 0;
    private Ghost _closestGhost;

    public float Cooldown => _cooldown;
    public float CurrentCooldown => _currentCooldown;

    private void Start()
    {
        _player = GetComponent<Player>();

        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            if (gameObject.TryGetComponent(out Ghost ghost))
                _ghosts.Add(ghost);
        }

        _currentCooldown = _cooldown;
    }

    private void Update()
    {
        _currentCooldown += Time.deltaTime;
    }
    public void UseVampireAbility()
    {
        if (_cooldown <= _currentCooldown && TryGetClosestGhost(out Ghost closestGhost))
        {
            _closestGhost = closestGhost;
            _currentCooldown = 0;
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

        if (distance <= _distance)
            return true;
        else
            return false;
    }

    private IEnumerator VampireAbilityUsing()
    {
        float currentUsingTime = 0;

        while (_usingTime >= currentUsingTime)
        {
            currentUsingTime += Time.deltaTime;

            _closestGhost.Health.TakeDamage(Time.deltaTime);

            _player.Health.Healing(Time.deltaTime);

            yield return null;
        }

        StopCoroutine(VampireAbilityUsing());
    }
}
