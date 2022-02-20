using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

#region Serialize Fields

    [SerializeField] private GameObject _enemyCanonPrefab, _enemyPrefab, _enemyContainer;
    [SerializeField] private GameObject[] _listOfPowerups, _listOfNegativePowerups;

#endregion


#region Private Variables

    private bool _stopSpawningEnemies = false;
    private bool _stopSpawningPowerups = false;
    private int _currentEnemysDefeated;
    private int _maxEnemiesInWave = 3;  
    private int _totalEnemiesSpawned;  

#endregion


#region Unity Methods

 private void Update() 
    {
        if (_totalEnemiesSpawned == _maxEnemiesInWave)
        {
            SetStopSpawingEnemies(true);
        }

        if (_currentEnemysDefeated == _maxEnemiesInWave)
        {
            GameManager.instance.StartNewWave();
            _currentEnemysDefeated = 0;
            _totalEnemiesSpawned = 0;
            _maxEnemiesInWave += 3;

        }
    }

#endregion


#region Public Methods

    public void StartSpawning()
    {
        StartCoroutine(nameof(SpawnBasicEnemyRoutine));
        StartCoroutine(nameof(SpawnHorzEnemyRoutine));

        if (GameManager.instance.GetWaveIndex() == 1)
        {
            StartCoroutine(nameof(SpawnPowerupRoutine));
            StartCoroutine(nameof(SpawnNegativePowerupRoutine));
        }

        if (GameManager.instance.GetWaveIndex() >= 2)
        {
            StartCoroutine(nameof(SpawnEnemyCanonRoutine));
        }
        
    }

    public void SetStopSpawingEnemies(bool stopSpawn)
    {
        _stopSpawningEnemies = stopSpawn;
    }

    public void SetStopSpawingPowerups(bool stopSpawn)
    {
        _stopSpawningPowerups = stopSpawn;
    }

    public void IncrementEnemyDeath()
    {
        _currentEnemysDefeated++;
    }

#endregion


#region Coroutines

    IEnumerator SpawnBasicEnemyRoutine()
    {
        float randomSeconds = Random.Range(3f, 5f);
        yield return new WaitForSeconds(randomSeconds);
        
        randomSeconds = Random.Range(3f, 5f);

        while (_stopSpawningEnemies == false)
        {            
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.SetEnemyID(0);
            }

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            _totalEnemiesSpawned++;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnHorzEnemyRoutine()
    {
        float randomSeconds = Random.Range(3f, 5f);
        yield return new WaitForSeconds(randomSeconds);

        randomSeconds = Random.Range(3f, 5f);
        
        while (_stopSpawningEnemies == false) 
        {            
            Vector3 randomSpawnLocation = new Vector3(9.4f, Random.Range(2.5f,5.3f), 0);

            GameObject spawnedEnemy = Instantiate(_enemyPrefab, randomSpawnLocation, Quaternion.identity);
            Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.SetEnemyID(1);
            }

            spawnedEnemy.transform.parent = _enemyContainer.transform;

            _totalEnemiesSpawned++;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnEnemyCanonRoutine()
    {
        float randomSeconds = Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomSeconds);

        randomSeconds = Random.Range(5f, 10f);
        
        while (_stopSpawningEnemies == false) 
        {
            GameObject spawnedEnemyCanon = Instantiate(_enemyCanonPrefab, new Vector3(0f, 8f, 0), _enemyCanonPrefab.transform.rotation);

            spawnedEnemyCanon.transform.parent = _enemyContainer.transform;

            _totalEnemiesSpawned++;
            yield return new WaitForSeconds(randomSeconds);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawningPowerups == false)
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

    IEnumerator SpawnNegativePowerupRoutine()
    {
        while (_stopSpawningPowerups == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject nextNegativePowerup = _listOfNegativePowerups[Random.Range(0, _listOfNegativePowerups.Length)];

            GameObject spawnedPowerup = Instantiate(nextNegativePowerup, randomSpawnLocation, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

#endregion
    
}
