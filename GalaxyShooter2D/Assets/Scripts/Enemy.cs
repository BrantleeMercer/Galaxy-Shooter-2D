using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

#region Serialize Fields

    [SerializeField] private float _speed = 4f, _ramSpeed = 5f;
    [SerializeField] private GameObject _laserPrefab, _backLaserPrefab;

#endregion


#region Private Variables

    private Animator _enemyDeathAnim;
    private Player _player;
    private SpawnManager _spawnManager;
    private Transform _enemyShield;
    private float _rateOfFire = 3f, _rateOfFireOnPowerups = 2f;
    private float _canFire = -1, _canShootPowerup = -1;
    private bool _isAlive = true;
    private int _id;
    private bool _isShieldActive = false;
    private bool _isBehindPlayer = false;

#endregion


#region Unity Methods

   private void Start() 
   {
       _player = GameObject.Find("Player").GetComponent<Player>();
       if (_player == null)
       {
           Debug.LogError("_player :: Enemy == null");
       }

       _enemyDeathAnim = GetComponent<Animator>();
       if (_enemyDeathAnim == null)
       {
           Debug.LogError("_enemyDeathAnim :: Enemy == null");
       }

       _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
       if (_spawnManager == null)
       {
           Debug.LogError("_spawnManager :: Enemy == null");
       }

       _enemyShield = transform.GetChild(0);
       if (_enemyShield == null)
       {
           Debug.LogError("_enemyShield :: Enemy == null");
       }

       float chanceForShield = Random.value;

       if (chanceForShield <= .3f)
       {
           _isShieldActive = true;
           _enemyShield.gameObject.SetActive(true);
       }
       
   }

    private void Update()
    {
       EnemyMovement();

       if ((Time.time > _canFire) && _isAlive)
       {
           FireEnemyLaser();
       }
    }

    private void FixedUpdate()
    {
        if ((Time.time > _canShootPowerup) && _isAlive)
       {
           ShootPowerups();
       }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Player"))
        {
            if (_player != null)
            {
                _player.Damage(1);
            }

            PlayDeathAnimation();

        }
        else if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            
            if (_player != null && !_isShieldActive)
            {
                _player.AddToScore(10);
            }

            PlayDeathAnimation();
            
        }
    }
#endregion


#region Private Methods

    private void EnemyMovement()
    {
        if (transform.position.y < _player.transform.position.y)
        {
            _isBehindPlayer = true;
        }
        else 
        {
            _isBehindPlayer = false;
        }

        if (_id == 0 && _isAlive)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            
            if (transform.position.y < -5.4f)
            {
                float randX = Random.Range(-9.4f, 9.4f);
                transform.position = new Vector3(randX, 7.3f, 0);
            }

            if (!_isBehindPlayer)
            {
                RamPlayer();
            }

            AvoidShot();
        }

        if (_id == 1 && _isAlive)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        
            if (transform.position.x < -9.4f)
            {
                float randY = Random.Range(2.5f,5.3f);
                transform.position = new Vector3(9.4f, randY, 0);
            }
        }
        
    }

     private void RamPlayer()
    {
        float distanceBetweenPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceBetweenPlayer < 3f && !_isShieldActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _ramSpeed * Time.deltaTime);
        }
    }

    private void PlayDeathAnimation()
    {

        if (_isShieldActive)
        {
            _isShieldActive = false;
            _enemyShield.gameObject.SetActive(false);
            return;
        }
        else
        {
            _isAlive = false;
            _speed = 0;

            _enemyDeathAnim.SetTrigger("OnEnemyDeath");
            AudioManager.instance.PlaySoundEffect("explosion");

            _spawnManager.IncrementEnemyDeath();

            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }

    private void FireEnemyLaser()
    {
        _rateOfFire = Random.Range(3f,7f);
        _canFire = Time.time + _rateOfFire;
        if (!_isBehindPlayer)
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
        else 
        {
            Instantiate(_backLaserPrefab, transform.position, Quaternion.identity);
        }
        
    }

    private void ShootPowerups()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 5f), Vector2.down);

        if (hit.transform != null)
        {
            if (hit.transform.tag.Equals("Powerup") || hit.transform.tag.Equals("TripleShot"))
            {
                _canShootPowerup = Time.time + _rateOfFireOnPowerups;
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
        }

        
    }

    private void AvoidShot()
    {
        GameObject playerLaser = GameObject.FindGameObjectWithTag("Laser");
        if(playerLaser == null) 
        { 
            return; 
        }
        else
        {
            if (Vector3.Distance(playerLaser.transform.position, transform.position) < 3f)
            {
                transform.Translate(Vector3.right * (_speed + 1f) * Time.deltaTime);
            }
        }
        
    }

#endregion


#region Public Methods

    public void SetEnemyID(int id)
    {
        _id = id;
    }

#endregion

}
