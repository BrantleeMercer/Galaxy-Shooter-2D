using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField] private float _speed = 20f;
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager :: Asteroids == NULL");
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0,0,1) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            
            int waveIndex = GameManager.instance.GetWaveIndex();

            if (waveIndex < 4)
            {
                _spawnManager.SetStopSpawingEnemies(false);
                _spawnManager.SetStopSpawingPowerups(false);
                _spawnManager.StartSpawning();
            }
            else if (waveIndex == 4)
            {
                _spawnManager.SetStopSpawingPowerups(false);
                _spawnManager.StartSpawning();
            }
            
            
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.25f);
        }
    }

}
