using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    private Weapon currentWeapon;
    public int damage = 0;
    public int pierceAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            --pierceAmount;
            other.GetComponent<HealthComponent>().ApplyHealthDelta(damage);
            if (pierceAmount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
