using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanon : MonoBehaviour
{
    [SerializeField] private GameObject _laserBeam;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _speed = 3f;
    
    private Player _player;
    private SpawnManager _spawnManager;
    private string _direction = "left";
    private bool _isLaserActive = false;
    private bool _isAlive = true;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager :: EnemyCanon == null");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("_player :: Enemy == null");
        }

        StartCoroutine(ShootLaserBeamRoutine());
    }

    void Update()
    {
        CanonMovement();

        ShootLaserRay();
    }

    private void CanonMovement()
    {
        //Due to the rotation of the sprite:
        // Vector3.left = down
        // Vector3.right = up
        // Vector3.down = left
        // Vector3.up = right

        if (transform.position.y > 3f)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else
        {
            MoveHorzontial();
        }

        if (transform.position.x <= -9.4)
        {
            _direction = "right";
        }
        else if (transform.position.x >= 9.4)
        {
            _direction = "left";
        }
        
    }

    private void MoveHorzontial()
    {
        if (_direction.Equals("left"))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (_direction.Equals("right"))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
    }

    private void ShootLaserRay()
    {
        if (_isLaserActive)
        {
            _laserBeam.SetActive(true);
        }
        else
        {
            _laserBeam.SetActive(false);
        }
    }

    private void LaserCanonDeath()
    {
        _isAlive = false;
        _spawnManager.IncrementEnemyDeath();

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            
            _player.AddToScore(10);

            LaserCanonDeath();
        }
    }

    public void SetLaserActive(bool isLaserActive)
    {
        _isLaserActive = isLaserActive;
    }

    IEnumerator ShootLaserBeamRoutine()
    {
        while(_isAlive)
        {
            yield return new WaitForSeconds(3f);
            SetLaserActive(true);
            yield return new WaitForSeconds(5f);
            SetLaserActive(false);
        }
    }
}
