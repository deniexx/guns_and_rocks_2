using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Transform transformTarget; //player pos
    [SerializeField] private GameObject enemyGameObject;

    public float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //randomly position each object at the beginning of each episode (client, targets & enemy/chaser)
        transform.localPosition = new Vector3(Random.Range(10.58f, -10.91f), 1, Random.Range(-9.9f, 9.61f));

        Enemy Enemy = enemyGameObject.GetComponent<Enemy>();
        if (Enemy != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //collision checks
    private void OnTriggerEnter(Collider other)
    {
        
    }

}
