using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : MonoBehaviour
{
    public Sprite currentWeaponSpr;
    public GameObject bulletPrefab;

    public float fireRate = 1;

    public int magazineAmmo = 6;
    public int totalAmmo = 32;
    public int currentAmmo = 32;
    public int damage = 20;
    public int projectileSpreadAngle = 0;
    public int numOfProjectiles = 1;

    private bool bIsFiring = false;

    private float nextTimeOfFire = 0;
    public void StartFiring()
    {
        Debug.Log("Weapon firing!");
        bIsFiring = true;
    }

    void FixedUpdate()
    {
        while (bIsFiring == true)
        {
            if (Time.time >= nextTimeOfFire)
            {
                GameObject bullet = Instantiate(bulletPrefab, GameObject.Find("FirePoint").transform.position,
                    Quaternion.identity);
                nextTimeOfFire = Time.time + 1 / fireRate;
            }

        }
    }

    public void EndFiring()
    {
        Debug.Log("Weapon stopped firing!");
        bIsFiring = false;
    }

}
