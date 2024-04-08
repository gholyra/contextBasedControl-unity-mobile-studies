using System.Collections;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private GameObject enemy;

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minZ, maxZ;
    
    private void Start()
    {
        StartCoroutine(SpawnEnemy(minSpawnTime, maxSpawnTime));
    }

    private IEnumerator SpawnEnemy(float min, float max)
    {
        float t = Random.Range(min, max);
        yield return new WaitForSeconds(t);

        // DEFINE A POSIÇÃO DO SPAWN DO INIMIGO
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        Vector3 spawnPosition = new Vector3(x, transform.position.y, z);

        // SPAWN EFETIVO DO INIMIGO
        Instantiate(enemy, spawnPosition, transform.rotation);
        
        // SEGUE PARA O PRÓXIMO CICLO
        StartCoroutine(SpawnEnemy(min, max));
    }
}
