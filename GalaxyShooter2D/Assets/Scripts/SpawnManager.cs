using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

#region Serialize Fields

    [SerializeField] private GameObject _enemyCanonPrefab, _enemyPrefab, _enemyContainer;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private GameObject[] _listOfNegativePowerups;
    [SerializeField] private GameObject[] _listOfPowerupsRare, _listOfPowerupsCommon;

#endregion


#region Private Variables

    private bool _stopSpawningEnemies = false;
    private bool _stopSpawningPowerups = false;
    private int _currentEnemiesDefeated;
    private int _maxEnemiesInWave;  
    private int _totalEnemiesSpawned;

#endregion


#region Unity Methods

    private void Start()
    {
        _maxEnemiesInWave = 3;
        _currentEnemiesDefeated = 0;
    }

    private void Update() 
    {
        if (_totalEnemiesSpawned == _maxEnemiesInWave)
        {
            SetStopSpawingEnemies(true);
        }
    }

    private void FixedUpdate() 
    {
        if (_currentEnemiesDefeated == _maxEnemiesInWave)
        {
            SetStopSpawingPowerups(true);
            _currentEnemiesDefeated = 0;
            _totalEnemiesSpawned = 0;
            _maxEnemiesInWave += 1;
            GameManager.instance.StartNewWave();
        }
    }

#endregion


#region Public Methods

    public void StartSpawning()
    {
        int waveIndex = GameManager.instance.GetWaveIndex();

        StartCoroutine(SpawnBasicEnemyRoutine());
        StartCoroutine(SpawnHorzEnemyRoutine());

        if (waveIndex >= 1)
        {
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnNegativePowerupRoutine());
        }

        if (waveIndex >= 2)
        {
            StartCoroutine(SpawnEnemyCanonRoutine());
        }

        if (waveIndex == 4)
        {
            StartCoroutine(SpawnBossRoutine());
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
        _currentEnemiesDefeated++;
    }

#endregion


#region Coroutines

    IEnumerator SpawnBasicEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);        

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
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    IEnumerator SpawnHorzEnemyRoutine()
    {
        yield return new WaitForSeconds(4f);
        
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
            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    IEnumerator SpawnEnemyCanonRoutine()
    {
        yield return new WaitForSeconds(6f);

        while (_stopSpawningEnemies == false) 
        {
             Vector3 randomSpawnLocation = new Vector3(Random.Range(-8f, 8f), 7.3f, 0);

            GameObject spawnedEnemyCanon = Instantiate(_enemyCanonPrefab, randomSpawnLocation, _enemyCanonPrefab.transform.rotation);

            spawnedEnemyCanon.transform.parent = _enemyContainer.transform;

            _totalEnemiesSpawned++;
            yield return new WaitForSeconds(Random.Range(6f, 8f));
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        while (_stopSpawningPowerups == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject nextPowerup;

           float chanceToSpawn = Random.value;

            if (chanceToSpawn <= .1f)
            {
                nextPowerup = _listOfPowerupsRare[Random.Range(0, _listOfPowerupsRare.Length)];
            }
            else
            {
                nextPowerup = _listOfPowerupsCommon[Random.Range(0, _listOfPowerupsCommon.Length)];
            }

            Instantiate(nextPowerup, randomSpawnLocation, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    IEnumerator SpawnNegativePowerupRoutine()
    {
        yield return new WaitForSeconds(Random.Range(7f, 10f));

        while (_stopSpawningPowerups == false)
        {
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-9.4f, 9.4f), 7.3f, 0);

            GameObject nextNegativePowerup = _listOfNegativePowerups[Random.Range(0, _listOfNegativePowerups.Length)];

            Instantiate(nextNegativePowerup, randomSpawnLocation, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(7f, 10f));
        }
    }

    IEnumerator SpawnBossRoutine()
    {
        yield return null;
        Instantiate(_bossPrefab, new Vector3(0f, 7f, 0f), Quaternion.identity);
    }

#endregion
    
}
