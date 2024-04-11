using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyGameObject;
    [SerializeField] Camera camera;
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] float spawnRate = 10f;
    [SerializeField] float spawnEnemy = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnRate)
        {
            spawnEnemy = Time.time + spawnRate;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = CalcSpawnPos();

        if (IsPosOutsideView(spawnPos))
        {
            Instantiate(enemyGameObject, spawnPos, Quaternion.identity);
        }
    }

    Vector3 CalcSpawnPos()
    {
        float spawnX = Random.Range(-spawnDistance, spawnDistance);
        float spawnY = Random.Range(-spawnDistance, spawnDistance);

        Vector3 pos = camera.transform.position + new Vector3(spawnX, spawnY, 0);
        return pos;
    }

    bool IsPosOutsideView(Vector3 pos)
    {
        Vector3 screenPos = camera.WorldToScreenPoint(pos);
        return screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
    }
}
