using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite currentWeaponSpr;
    public GameObject bulletPrefab;

    public float fireRate = 1;

    public int magazineAmmo = 6;
    public int totalAmmo = 32;
    public int currentAmmo = 32;
    public int damage = 20;
    public int projectileSpreadAngle = 90;
    public int numOfProjectiles = 5;
    public int recoilImpulse = 0;
    public float projectileLifeSpan = 1f;
    public int pierceAmount = 0;

    private PlayerController _playerController;

    private bool bIsFiring = false;

    private float nextTimeOfFire = 0;

    public void Equipped(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public void StartFiring()
    {
        Debug.Log("Weapon firing!");
        bIsFiring = true;
    }

    void FixedUpdate()
    {
        if (bIsFiring == true)
        {
            if (Time.time >= nextTimeOfFire)
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
                    _playerController.ApplyImpulseAwayFromMousePos(recoilImpulse);
                }
            }
        }
    }

    public void EndFiring()
    {
        Debug.Log("Weapon stopped firing!");
        bIsFiring = false;
    }

}
