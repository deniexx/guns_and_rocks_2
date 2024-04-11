using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float fireRate = 1;

    public string name = "";

    public int magazineAmmo = 6;
    public float reloadDelay = 0.3f;
    public int currentAmmo = 32;
    public int currentAmmoInMagazine = 6;
    public int damage = 20;
    public int projectileSpreadAngle = 90;
    public int numOfProjectiles = 5;
    public int recoilImpulse = 0;
    public float projectileLifeSpan = 1f;
    public int pierceAmount = 0;

    public Quaternion attachRotation;

    private PlayerController _playerController;

    private bool bIsFiring = false;

    private float nextTimeOfFire = 0;

    private Coroutine _reloadCoroutine;

    public UnityEvent<float> onReloadDelayStarted;
    public UnityEvent<int, int> onAmmoUpdated;

    public void Equipped(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public void StartFiring()
    {
        bIsFiring = true;
    }

    void FixedUpdate()
    {
        if (bIsFiring == true)
        {
            if (Time.time >= nextTimeOfFire)
            {
                if (currentAmmoInMagazine > 0)
                {
                    Vector3 bulletDir = -GameplayStatics.GetDirectionFromMouseToLocation(transform.position);
                    Vector3 leftOfSpread = GameplayStatics.RotateVector(bulletDir, -projectileSpreadAngle / 2f);

                    float deltaSpread = projectileSpreadAngle / (float)(numOfProjectiles + 1);
                    for (int i = 0; i < numOfProjectiles; i++)
                    {
                        Vector3 dirActual = GameplayStatics.RotateVector(leftOfSpread, deltaSpread * i);

                        //Debug.DrawLine(transform.position, transform.position + dirActual * 20, Color.black, 5f);

                        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        Bullet bullet = bulletGO.GetComponent<Bullet>();
                        bullet.damage = damage;
                        bullet.dir = dirActual;
                        bullet.pierceAmount = pierceAmount;
                        Destroy(bulletGO, projectileLifeSpan);
                        nextTimeOfFire = Time.time + 1 / fireRate;
                        if (recoilImpulse > 1)
                        {
                            _playerController.ApplyImpulseAwayFromMousePos(recoilImpulse);
                        }
                    }
                    
                    currentAmmoInMagazine--;
                    onAmmoUpdated?.Invoke(currentAmmoInMagazine, currentAmmo);
                }
            }
        }
    }

    public void Reload()
    {
        if (_reloadCoroutine == null && currentAmmoInMagazine != magazineAmmo && currentAmmo != 0)
        {
            _reloadCoroutine = StartCoroutine(ReloadAfterDelay(reloadDelay));
            onReloadDelayStarted?.Invoke(reloadDelay);
        }
    }

    private IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int ammoNeeded = magazineAmmo - currentAmmoInMagazine;
        if (currentAmmo > ammoNeeded)
        {
            currentAmmoInMagazine = magazineAmmo;
            currentAmmo -= ammoNeeded;
        }
        else
        {
            currentAmmoInMagazine = currentAmmo;
            currentAmmo = 0;
        }
        
        onAmmoUpdated?.Invoke(currentAmmoInMagazine, currentAmmo);
        _reloadCoroutine = null;
    }

    public void EndFiring()
    {
        bIsFiring = false;
    }

}
