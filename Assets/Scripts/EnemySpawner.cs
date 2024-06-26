using System.Collections;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] float spawnRate = 10f;
    [SerializeField] float spawnEnemy = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //start timer - spawning of enemies
        StartCoroutine(EnemySpawnTimer(2.5f, RollEnemyType()));
    }
    
    //create the timer to spawn enemies
    private IEnumerator EnemySpawnTimer(float delay, GameObject enemyType)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy(enemyType);
        StartCoroutine(EnemySpawnTimer(spawnRate, RollEnemyType()));
    }

    //spawn enemy into world - off screen
    void SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnPos = CalcSpawnPos();

        if (IsPosOutsideView(spawnPos))
        {
            spawnPos.z = 0f;
            Instantiate(enemyType, spawnPos, Quaternion.identity);
        }
    }

    //calculate spawn position using screen and threasholds
    Vector3 CalcSpawnPos()
    {
        float spawnX = Random.Range(-spawnDistance, spawnDistance);
        float spawnY = Random.Range(-spawnDistance, spawnDistance);

        Vector3 pos = GameManager.Instance.mainCam.transform.position + new Vector3(spawnX, spawnY, 0);
        return pos;
    }

    //choose a random position off-screen
    bool IsPosOutsideView(Vector3 pos)
    {
        Vector3 screenPos = GameManager.Instance.mainCam.WorldToScreenPoint(pos);
        return screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
    }

    GameObject RollEnemyType()
    {
        int roll = Random.Range(0, enemyTypes.Length);

        return enemyTypes[roll];
    }
}
