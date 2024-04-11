using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] 
    private float maxHealth = 100f;

    [SerializeField] 
    private float invincibilityDuration = 0.1f;
    
    private float _health;

    [SerializeField] 
    private bool applyInvincibility = false;

    public float InvincibilityDuration => invincibilityDuration;

    private bool _invincible = false;
    
    [Space(10f)]
    public UnityEvent<float /* newHealth */, float /* appliedDelta */> onHealthChanged;
    
    private void Awake()
    {
        _health = maxHealth;
    }
    
    /// <summary>
    /// Applies a delta to the health variable
    /// </summary>
    /// <param name="delta">How much to change the health by, negative numbers will deal damage</param>
    public void ApplyHealthDelta(float delta)
    {
        if (_invincible) return;
        
        float oldHealth = _health;
        _health = Mathf.Clamp(_health + delta, 0, maxHealth);

        float actualDelta = _health - oldHealth;
        onHealthChanged?.Invoke(_health, actualDelta);
        if (applyInvincibility)
        {
            StartCoroutine(ApplyInvincibilityForDuration(InvincibilityDuration));
        }
        Debug.Log("Max Health:" + _health);
    }

    private IEnumerator ApplyInvincibilityForDuration(float duration)
    {
        _invincible = true;
        yield return new WaitForSeconds(duration);
        _invincible = false;
    }
}
