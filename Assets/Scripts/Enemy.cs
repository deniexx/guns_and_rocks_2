using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //set variables for AI's (player) location & enemy/chaser speed
    public Transform playerTransform;
    public float speed = 2f;


    //set cubic area for spawning using min and max values
    private Vector3 spawnMin = new Vector3(-10.91f, 0, 0);
    private Vector3 spawnMax = new Vector3(10.58f, 16.35f, 0);


    [SerializeField] private float enemyHealth = 100;


    //funciton to spawn the enemy/chaser randomly within the predefined cubic area
    public void SetRandomSpawnPosition()
    {
        transform.localPosition = new Vector3(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );
    }

    //update enemy by moving it towards the AI/client's location
    void Update()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;
    }
}
