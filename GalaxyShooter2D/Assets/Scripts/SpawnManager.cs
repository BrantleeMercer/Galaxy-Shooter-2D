using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    private bool _stopSpawning = false;
   

    void Start()
    {
        StartCoroutine(nameof(SpawnRoutine)); 
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    public void SetStopSpawing()
    {
        _stopSpawning = true;
    }
}
