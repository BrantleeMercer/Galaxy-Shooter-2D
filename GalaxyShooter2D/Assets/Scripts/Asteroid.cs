using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager :: Asteroids == NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate on Z axis
        transform.Rotate(new Vector3(0,0,1) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            
            _spawnManager.StartSpawning();
            
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.25f);
        }
    }
}
