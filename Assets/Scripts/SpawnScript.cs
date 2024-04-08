using System.Collections;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private GameObject[] enemy;

    private void Start()
    {
        StartCoroutine(SpawnEnemy(minSpawnTime, maxSpawnTime));
    }

    private IEnumerator SpawnEnemy(float min, float max)
    {
        float t = Random.Range(min, max);
        yield return new WaitForSeconds(t);
        StartCoroutine(SpawnEnemy(min, max));
    }
}
