using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab;
    [SerializeField] private GameObject[] _engineDamage;
    [SerializeField] private float _rateOfFire = 0.5f, _speed = 5f, _boostedSpeed = 9f;
    [SerializeField] private int _playerHealth = 3, _score = 0;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _tripleShotActive = false, _isSpeedBoosted = false, _isShieldActive = false;
    private Transform _shieldVisualizer;

    private bool _isThrusterActive = false;
    [SerializeField] private float thrusterSpeed = 7f;
    [SerializeField] private float thrustAndBoost = 12f;
    

    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        
        _shieldVisualizer = transform.GetChild(0);
    }


    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isThrusterActive = true;
        }
        else
        {
            _isThrusterActive = false;
        }
    }


    private void CalculateMovement()
    {
        float userInputHorz = Input.GetAxis("Horizontal");
        float userInputVert = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(userInputHorz, userInputVert, 0);

        int boost = BoostSpeed(_isThrusterActive, _isSpeedBoosted);

        switch(boost)
        {
            case 1: //Thruster and no Speed powerup
                transform.Translate(direction * thrusterSpeed * Time.deltaTime);
            break;

            case 2: //Speed power up and no thruster
                transform.Translate(direction * _boostedSpeed * Time.deltaTime);
            break;

            case 3: //Both Speed powerup is active and thruster is pressed
                transform.Translate(direction * thrustAndBoost * Time.deltaTime);
            break;

            default: //Normal speed
                transform.Translate(direction * _speed * Time.deltaTime);
            break;
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

    private int BoostSpeed(bool thruster, bool speedPowerup)
    {
        int speedBoostCombonation;

        if(thruster && !speedPowerup)
        {
            speedBoostCombonation = 1;
        }
        else if(speedPowerup && !thruster)
        {
            speedBoostCombonation = 2;
        }
        else if (thruster && speedPowerup)
        {
            speedBoostCombonation = 3;
        }
        else
        {
            speedBoostCombonation = 0;
        }

        return speedBoostCombonation;
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
            Instantiate(_laserPrefab, frontOfShip, Quaternion.identity);
        }       
    }

    public void Damage(int damage)
    {
        if(_isShieldActive)
        {
            damage = 0;
            _isShieldActive = false;
            _shieldVisualizer.gameObject.SetActive(false);
        }

        _playerHealth -= damage;

        _uiManager.UpdateLivesImage(_playerHealth);

        switch(_playerHealth)
        {
            case 2:
                int engineSelection = Random.Range(0, _engineDamage.Length);
                _engineDamage[engineSelection].gameObject.SetActive(true);
            break;

            case 1:
                if (!_engineDamage[0].gameObject.activeInHierarchy)
                {
                    _engineDamage[0].gameObject.SetActive(true);
                }
                else if (!_engineDamage[1].gameObject.activeInHierarchy)
                {
                    _engineDamage[1].gameObject.SetActive(true);
                }
            break;

            case 0:
                _spawnManager.SetStopSpawing();
                Destroy(gameObject);
            break;

            default:
                Debug.LogWarning($"_playerHealth :: Player == Not accounted for: {_playerHealth}");
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("EnemyLaser"))
        {
            Destroy(other.gameObject);
            Damage(1);
        }
    }

    public void AddToScore(int scoreValue)
    {
        _score += scoreValue;
        _uiManager.UpdateScoreText(_score);
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
        _shieldVisualizer.gameObject.SetActive(true);
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

#endregion

}
