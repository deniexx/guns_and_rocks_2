using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyTypeProjectile : MonoBehaviour
{
    //set variables for AI's (player) location & enemy/chaser speed
    private Transform playerTransform;
    private Transform enemyTransform;
    public float speed = 3.5f;
    public float approachThreashold = 10f;


    GameObject healthComponentObject;
    [SerializeField] GameObject enemyBullet;
    [SerializeField] public float fireRate = 3;

    private void Start()
    {
        //link player transform to player game object   &   link up enemy transform to local transform
        playerTransform = GameManager.Instance.Player.transform;
        enemyTransform = transform;

        //shoot first bullet(s) 3 seconds after game starts
        StartCoroutine(EnemyShootTimer(3));
    }

    //update enemy by moving it towards the AI/client's location
    void Update()
    {
        //if the enemy is far enough away - move towards player  |  when enemy get close enough, it will try to keep its distance as it shoots
        if ((playerTransform.position - enemyTransform.position).magnitude > approachThreashold) enemyTransform.position += getDireciton() * (speed * Time.deltaTime);
        else if ((playerTransform.position - enemyTransform.position).magnitude < approachThreashold - 1) enemyTransform.position += -getDireciton() * (speed * Time.deltaTime); 


        //shoot
        EnemyShootTimer(fireRate);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //player - enemy collisions
        if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<HealthComponent>().ApplyHealthDelta(-10f);
    }

    Vector3 getDireciton()
    {
        return (playerTransform.position - transform.position).normalized;
    }

    private IEnumerator EnemyShootTimer(float fireRate)
    {
        yield return new WaitForSeconds(fireRate);

        Instantiate(enemyBullet, enemyTransform.position, quaternion.identity);
        StartCoroutine(EnemyShootTimer(fireRate));
    }

    void deathCheck()
    {
        HealthComponent healthComponent = healthComponentObject.GetComponent<HealthComponent>();

        float currentHealth = healthComponent._health;

        if (gameObject != null) if (currentHealth <= 0) Destroy(gameObject);
    }
}
