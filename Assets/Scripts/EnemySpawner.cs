using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float timeBeforeSpawn = 1.5f;

    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        for (float i = timeBeforeSpawn; i <= timeBeforeSpawn; i -= Time.deltaTime)
        {
            if (i <= 0f)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
                i = timeBeforeSpawn;
            }
            yield return null;
        }
    }
}
