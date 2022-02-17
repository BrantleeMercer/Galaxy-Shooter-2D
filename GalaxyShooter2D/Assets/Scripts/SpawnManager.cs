using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField] private GameObject[] _listOfPowerups;
    private bool _stopSpawning = false;
    
    public void StartSpawning()
    {
        StartCoroutine(nameof(SpawnBasicEnemyRoutine));
        StartCoroutine(nameof(SpawnHorzEnemyRoutine));
        StartCoroutine(nameof(SpawnPowerupRoutine));
    }

    IEnumerator SpawnBasicEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        
        while (_stopSpawning == false)
        {            
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.SetEnemyID(0);
            }

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnHorzEnemyRoutine()
    {
        yield return new WaitForSeconds(4f);
        
        while (_stopSpawning == false) 
        {            
            Vector3 randomSpawnLocation = new Vector3(9.4f, Random.Range(2.5f,5.3f), 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.SetEnemyID(1);
            }

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject nextPowerup = _listOfPowerups[Random.Range(0, _listOfPowerups.Length)];

            if(nextPowerup == _listOfPowerups[_listOfPowerups.Length - 1]) //Make Circular shot rare, NEED TO KEEP CIRCULAR SHOT AS LAST POWERUP!
            {
                //Circular shot has 1 in however long the list is chance, and if it is selected, has a 50/50 chance of spawning
                float chance = Random.value;
                if(chance < .5f)
                {
                    nextPowerup = _listOfPowerups[Random.Range(0, _listOfPowerups.Length - 1)]; 
                }
            }

            GameObject spawnedPowerup = Instantiate(nextPowerup, randomSpawnLocation, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void SetStopSpawing()
    {
        _stopSpawning = true;
    }
}
