using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIUpdate : MonoBehaviour
{
    private Slider _slider;
    private float _maxHealth;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        HealthComponent comp = GameManager.Instance.Player.GetComponent<HealthComponent>();
        comp.onHealthChanged.AddListener(UpdateUI);
        _maxHealth = comp.maxHealth;
        _slider.value = 1f;
    }

    private void UpdateUI(float health, float delta)
    {
        float targetValue = health / _maxHealth;
        StartCoroutine(LerpValue(_slider.value, targetValue));
    }

    IEnumerator LerpValue(float starting, float end)
    {
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 3;
            float res = Mathf.Lerp(starting, end, alpha);
            yield return null;
        }
    }
}
