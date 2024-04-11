using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //set variables for AI's (player) location & enemy/chaser speed
    private Transform playerTransform;
    private Transform enemyTransform;
    public float speed = 1f;


    private void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
        enemyTransform = transform;
    }

    //update enemy by moving it towards the AI/client's location
    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        enemyTransform.position += direction * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //player - enemy collisions
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthComponent>().ApplyHealthDelta(-10f);
        }
    }
}
