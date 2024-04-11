using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponUIUpdater : MonoBehaviour
{
    private TMP_Text _text;
    private Weapon _currentWeapon;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().onWeaponChanged.AddListener(OnWeaponChanged);
    }

    private void OnWeaponChanged(Weapon newWeapon)
    {
        if (_currentWeapon)
        {
            _currentWeapon.onAmmoUpdated.RemoveListener(OnAmmoUpdated);
        }
        
        _currentWeapon = newWeapon;
        _currentWeapon.onAmmoUpdated.AddListener(OnAmmoUpdated);
        _text.SetText($"{_currentWeapon.currentAmmoInMagazine}/{_currentWeapon.currentAmmo}");
    }

    private void OnAmmoUpdated(int ammo, int availableAmmo)
    {
        _text.SetText($"{ammo}/{availableAmmo}");
    }
}
