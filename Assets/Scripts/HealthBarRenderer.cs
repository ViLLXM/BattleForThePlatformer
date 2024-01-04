using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]

public class HealthBarRenderer : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _speed;

    private Slider _slider;
    private Coroutine _coroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        _slider.value = ConvertToSliderValue(_health.HealthCount, _health.MaxHealth);
    }

    private void OnEnable()
    {
        _health.Render += OnRender;
    }

    private void OnDisable()
    {
        _health.Render -= OnRender;
    }

    private void OnRender()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Updating());
    }

    private float ConvertToSliderValue(float value, float maxValue)
    {
        return value/maxValue;
    }

    private IEnumerator Updating()
    {
        while (ConvertToSliderValue(_health.HealthCount, _health.MaxHealth) != _slider.value)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, ConvertToSliderValue(_health.HealthCount, _health.MaxHealth), _speed * Time.deltaTime);
            yield return null;
        }
    }
}