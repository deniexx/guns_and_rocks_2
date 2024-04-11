using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 dir;
    public float speed = 3;
    public int damage = 10;
    public int pierceAmount = 0;
    public Vector3 direction;

    //use this to send bullet towards player
    private Transform playerTransform; 
    [SerializeField] GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        //aim at player (bullet only needs to see player once, otherwise it tracks)
        playerTransform = GameManager.Instance.Player.transform;
        direction = getDireciton();

        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move bullet towards player
        transform.position += direction * speed * Time.deltaTime;
    }

    //apply any damage to player
    void OnTriggerEnter2D(Collider2D other)
    {
        //collision checks
        if (other.CompareTag("Player"))
        {
            //apply damage to player
            other.GetComponent<HealthComponent>().ApplyHealthDelta(damage);
        }
    }

    //get the direction of the player
    Vector3 getDireciton()
    {
        return (playerTransform.position - transform.position).normalized;
    }

}
