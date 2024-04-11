using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _manager;

    public static GameManager Instance => _manager;

    private GameObject _player;

    public GameObject Player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }

            return _player;
        }
    }

    private int _currency;

    public UnityEvent<int> onCurrencyChanged;
    
    public int Currency => _currency;

    [HideInInspector]
    public Camera mainCam;

    private void Awake()
    {
        if (!_manager)
        {
            _manager = this;
            InitManager();
            return;
        }
        
        Destroy(gameObject);
    }

    private void InitManager()
    {
        mainCam = Camera.main;
    }
    
    /// <summary>
    /// Checks if the player has enough currency
    /// </summary>
    /// <param name="amountToCheck">The amount of currency to check if the player has enough</param>
    /// <returns>TRUE if player has enough currency, FALSE if not</returns>
    public bool HasEnoughCurrency(int amountToCheck)
    {
        return _currency >= amountToCheck;
    }
    
    /// <summary>
    /// Adds to the currency of the player
    /// </summary>
    /// <param name="amountToAdd">How much currency to add</param>
    public void AddToCurrency(int amountToAdd)
    {
        _currency += amountToAdd;
        onCurrencyChanged?.Invoke(_currency);
    }

    /// <summary>
    /// Attempts to take away currency from the player
    /// </summary>
    /// <param name="amount">Amount to reduce the currency by</param>
    /// <returns>TRUE if transaction was successful, FALSE if transaction has failed (has not enough currency)</returns>
    public bool TakeAwayFromCurrency(int amount)
    {
        if (!HasEnoughCurrency(amount))
        {
            return false;
        }
        
        // Clamp currency to 0, although it should never go below 0, with the check above
        _currency = Mathf.Max(_currency - amount, 0);
        onCurrencyChanged?.Invoke(_currency);
        return true;
    }
}
