using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeProjectile : MonoBehaviour
{
    //set variables for AI's (player) location & enemy/chaser speed
    private Transform playerTransform;
    private Transform enemyTransform;
    public float speed = 1f;
    public float approachThreashold = 12f;
    public float orbitThreashold = 12f;
    private bool isAtDistance = false;


    private void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
        enemyTransform = transform;
    }

    //update enemy by moving it towards the AI/client's location
    void Update()
    {
        if ((playerTransform.position - enemyTransform.position).magnitude > approachThreashold) enemyTransform.position += getDireciton() * (speed * Time.deltaTime);
        else if ((playerTransform.position - enemyTransform.position).magnitude < approachThreashold - 1) 
        { 
            enemyTransform.position += -getDireciton() * (speed * Time.deltaTime); 
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //player - enemy collisions
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthComponent>().ApplyHealthDelta(-10f);
        }
    }

    Vector3 getDireciton()
    {
        return (playerTransform.position - transform.position).normalized;
    }
}
