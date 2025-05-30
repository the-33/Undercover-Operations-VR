using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval;
    private float spawnTime;
    public GameObject enemyPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime <= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            spawnTime = spawnInterval;
        }
        else spawnTime -= Time.deltaTime;
    }
}
