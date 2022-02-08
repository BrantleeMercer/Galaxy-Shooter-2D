using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField] private GameObject[] _listOfPowerups;
    private bool _stopSpawning = false;
    

    void Start()
    {
        StartCoroutine(nameof(SpawnEnemyRoutine)); 
        StartCoroutine(nameof(SpawnPowerupRoutine));
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject spawnedTripleShot = Instantiate(_listOfPowerups[Random.Range(0, _listOfPowerups.Length)], randomSpawnLocation, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void SetStopSpawing()
    {
        _stopSpawning = true;
    }
}
