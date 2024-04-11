using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public Sprite currentWeaponSpr;
    public GameObject bulletPrefab;
    public int damage = 20;
    public void Shoot(Vector3 firePoint)
    {
        Instantiate(bulletPrefab, firePoint, Quaternion.identity);
    }
    
}
