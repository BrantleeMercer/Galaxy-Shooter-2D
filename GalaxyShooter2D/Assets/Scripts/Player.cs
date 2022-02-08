using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab;
    [SerializeField] private float _rateOfFire = 0.5f, _speed = 5f, _boostedSpeed = 9f;
    [SerializeField] private int _playerHealth = 3;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private bool _tripleShotActive = false, _isSpeedBoosted = false, _isShieldActive = false;


    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
    }


    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }


    private void CalculateMovement()
    {
        float userInputHorz = Input.GetAxis("Horizontal");
        float userInputVert = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(userInputHorz, userInputVert, 0);
        
        if (_isSpeedBoosted)
        {
            transform.Translate(direction * _boostedSpeed * Time.deltaTime);
        }
        else 
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        

        VerticalBounds();
        HorizontalBounds();
    }

    private void VerticalBounds()
    {
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //Save this for later
        // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0, -3.8f),0);
    }

    private void HorizontalBounds()
    {
        float newBound = 10.8f;

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-newBound, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(newBound, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _rateOfFire;
        Vector3 frontOfShip = new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z);
        
        if(_tripleShotActive)
        {
            GameObject tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            Destroy(tripleShot, 1.5f);
        }
        else
        {
            GameObject laser = Instantiate(_laserPrefab, frontOfShip, Quaternion.identity);
            Destroy(laser, 1.5f);
        }
       
    }

    public void Damage(int damage)
    {
        _playerHealth -= damage;

        if (_playerHealth <= 0)
        {
            _spawnManager.SetStopSpawing();
            Destroy(gameObject);
        }
    }

#region Activate Powerups

    public void ActivateTripleShot()
    {
        _tripleShotActive = true;
        StartCoroutine(nameof(DeactivateTripleShotRoutine));
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoosted = true;
        StartCoroutine(nameof(DeactivateSpeedBoost));
    }

     public void ActivateShields()
    {
        _isShieldActive = true;
        StartCoroutine(nameof(DeactivateShields));
    }

#endregion

#region Coroutines

    IEnumerator DeactivateTripleShotRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    IEnumerator DeactivateSpeedBoost()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoosted = false;
    }

    IEnumerator DeactivateShields()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
    }

#endregion
}
