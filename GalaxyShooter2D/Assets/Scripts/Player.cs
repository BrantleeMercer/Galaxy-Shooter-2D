using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

#region Serialize Fields

    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _missilePrefab;
    [SerializeField] private GameObject[] _engineDamage;
    [SerializeField] private float _rateOfFire = 0.5f, _rateOfFireMissile = 1f, _speed = 5f, _boostedSpeed = 9f;
    [SerializeField] private int _playerHealth = 3, _score = 0;
    [SerializeField] private float thrusterSpeed = 7f;
    [SerializeField] private float thrustAndBoost = 12f;
    [SerializeField] private Text _shotCountText, _missileCountText;
    [SerializeField] private GameObject _circularShotPrefab;
    [SerializeField] private Image _thrusterChargeBar;
    [SerializeField] private Camera _mainCamera;
    
#endregion


#region Private Variables

    private bool _isThrusterActive = false;
    private int _shieldStrength = 0;
    private int _shotCount = 15;
    private int _missileCount = 0;
    private bool _circularShotActive = false;
    private bool _canColletPowerups = true;
    private float _maxCharge = 3f;
    private float _rateBeforeRecharge = 2f;
    private float _canRecharge = -1;
    private float _cameraShakeTime = .5f;
    private const int _MAXSHOTCOUNT = 15;
    private const int _MAXMISSILECOUNT = 5;
    private float _canFire = -1f, _canFireMissile = -1;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private bool _tripleShotActive = false, _isSpeedBoosted = false, _isShieldActive = false;
    private Transform _shieldVisualizer;
    
#endregion


#region Unity Methods

    private void Start()
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
        _shotCountText.text = $"Shots: {_shotCount}/{_MAXSHOTCOUNT}";
        _missileCountText.text = $"Missiles: {_missileCount}/{_MAXMISSILECOUNT}";
    }

    private void Update()
    {
        CalculateMovement();
        ColletPowerups();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.F) && Time.time > _canFireMissile)
        {
            FireHomingMissile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("EnemyLaser"))
        {
            Destroy(other.gameObject);
            Damage(1);
        }

        if (other.tag.Equals("LaserBeam"))
        {
            Damage(1);
        }
    }

#endregion


#region Private Methods
    private void CalculateMovement()
    {
        float userInputHorz = Input.GetAxis("Horizontal");
        float userInputVert = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(userInputHorz, userInputVert, 0);

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterChargeBar.fillAmount != 0)
        {
            _isThrusterActive = true;

            if (_thrusterChargeBar.fillAmount > 0)
            {
                _thrusterChargeBar.fillAmount -= 1f /_maxCharge * Time.deltaTime;
                _canRecharge = Time.time + _rateBeforeRecharge;
            }            
        }
        else
        {
            _isThrusterActive = false;

            if(_thrusterChargeBar.fillAmount < 1 && Time.time > _canRecharge)
            {
                _thrusterChargeBar.fillAmount += 1f /_maxCharge * Time.deltaTime;
            }
            
        }

        int boost = BoostSpeed(_isThrusterActive, _isSpeedBoosted);

        switch (boost)
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
        float newBound = 15f;

        if (transform.position.x > 15f)
        {
            transform.position = new Vector3(-newBound, transform.position.y, 0);
        }
        else if (transform.position.x < -15f)
        {
            transform.position = new Vector3(newBound, transform.position.y, 0);
        }
    }

    private int BoostSpeed(bool thruster, bool speedPowerup)
    {
        int speedBoostCombonation;

        if (thruster && !speedPowerup)
        {
            speedBoostCombonation = 1;
        }
        else if (speedPowerup && !thruster)
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
        if (_shotCount == 0)
        {
            return;
        }

        _shotCount--;
        _shotCountText.text = $"Shots: {_shotCount}/{_MAXSHOTCOUNT}";

        _canFire = Time.time + _rateOfFire;
        Vector3 frontOfShip = new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z);
        
        if (_tripleShotActive)
        {
            GameObject tripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            Destroy(tripleShot, 1.5f);
        }
        else if (_circularShotActive)
        {
            GameObject circleShot = Instantiate(_circularShotPrefab, transform.position, Quaternion.identity);
            Destroy(circleShot, 1.5f);
        }
        else
        {
            Instantiate(_laserPrefab, frontOfShip, Quaternion.identity);
        }       
    }

    private void FireHomingMissile()
    {
        if (_missileCount == 0)
        {
            return;
        }

        _missileCount--;
        _missileCountText.text = $"Missiles: {_missileCount}/{_MAXMISSILECOUNT}";

        _canFireMissile = Time.time + _rateOfFireMissile;
        GameObject missile = Instantiate(_missilePrefab, transform.position, Quaternion.identity);
        Destroy(missile, 5f);

    }

    private void ColletPowerups()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_canColletPowerups)
            {
                GameObject[] powerupsVisible = GameObject.FindGameObjectsWithTag("Powerup");
                foreach (var el in powerupsVisible)
                {
                    el.transform.position = transform.position;
                }

                _uiManager.UpdateMagnetImage(false);
                _canColletPowerups = false;
            }
        }
        
    }

#endregion


#region Public Methods

    public void AddToScore(int scoreValue)
    {
        _score += scoreValue;
        _uiManager.UpdateScoreText(_score);
    }

    public void Damage(int damage)
    {
        if(_isShieldActive)
        {
            _shieldStrength -= damage;

            switch(_shieldStrength)
            {
                case 2:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .75f);
                break;

                case 1:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .40f);
                break;

                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.gameObject.SetActive(false);
                break;

                default:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    Debug.LogWarning("Damage :: Player == Shields have more damage than count");
                break;
            }
            
            damage = 0;
        }

        _playerHealth -= damage;

        StartCoroutine(nameof(ShakeCameraRoutine));

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
                _spawnManager.SetStopSpawingEnemies(true);
                _spawnManager.SetStopSpawingPowerups(true);
                Destroy(gameObject);
            break;

            default:
                Debug.LogWarning($"_playerHealth :: Player == Not accounted for: {_playerHealth}");
            break;
        }
    }

    public void ResetShotCount()
    {
        _shotCount = _MAXSHOTCOUNT;
        _shotCountText.text = $"Shots: {_shotCount}/{_MAXSHOTCOUNT}";

        _canColletPowerups = true;
        _uiManager.UpdateMagnetImage(true);
    }

#endregion


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
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        _shieldStrength = 3;
        _shieldVisualizer.gameObject.SetActive(true);
    }

    public void ActivateHealthPowerup()
    {
        if (_playerHealth >= 3)
        {
            return;
        }

        _playerHealth++;

        _uiManager.UpdateLivesImage(_playerHealth);

        if (_engineDamage[0].gameObject.activeInHierarchy)
        {
            _engineDamage[0].gameObject.SetActive(false);
        }
        else if (_engineDamage[1].gameObject.activeInHierarchy)
        {
            _engineDamage[1].gameObject.SetActive(false);
        }
    }

    public void ActivateRefillPowerup()
    {
        _shotCount = 15;
        _shotCountText.text = $"Shots: {_shotCount}/{_MAXSHOTCOUNT}";
    }

    public void ActivateCircularShot()
    {
        _circularShotActive = true;
        StartCoroutine(nameof(DeactivateCircularShot));
    }

    public void ActivateMissileRefillPowerup()
    {
        _missileCount = _MAXMISSILECOUNT;
        _missileCountText.text = $"Missiles: {_missileCount}/{_MAXMISSILECOUNT}";
    }

/************** Negative Effects ***************/

    public void ActivateReduceAmmo()
    {
        _shotCount -= 5;

        if(_shotCount < 0)
        {
            _shotCount = 0;
        }

         _shotCountText.text = $"Shots: {_shotCount}/{_MAXSHOTCOUNT}";
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

    IEnumerator DeactivateCircularShot()
    {
        yield return new WaitForSeconds(5f);
        _circularShotActive = false;
    }

    IEnumerator ShakeCameraRoutine()
    {
        float timeLeft = _cameraShakeTime;
        float cameraXPos = 0.02f;
        while(timeLeft > 0)
        {
            _mainCamera.transform.position = new Vector3(cameraXPos, _mainCamera.transform.position.y, _mainCamera.transform.position.z);
            yield return new WaitForSeconds(0.02f);
            cameraXPos *= -1;
            timeLeft -= Time.deltaTime;
        }

        _mainCamera.transform.position = new Vector3(0f, _mainCamera.transform.position.y, _mainCamera.transform.position.z);

    }

#endregion

}
