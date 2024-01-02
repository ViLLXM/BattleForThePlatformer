using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _minHealth = 0;

    public event Action Render;

    private float _healthCount;

    public float MaxHealth => _maxHealth;
    public float HealthCount => _healthCount;

    private void Awake()
    {
        _healthCount = _maxHealth;
    }

    private void OnValidate()
    {
        if (_minHealth >= _maxHealth)
            _minHealth = _maxHealth - 1;
    }

    public void TakeDamage(float damage)
    {
        _healthCount = Mathf.Clamp(_healthCount - damage, _minHealth, _maxHealth);
        Render?.Invoke();
    }

    public void Healing(float heal)
    {
        _healthCount = Mathf.Clamp(_healthCount + heal, _minHealth, _maxHealth);
        Render?.Invoke();
    }
}