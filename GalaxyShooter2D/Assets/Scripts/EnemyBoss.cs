using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [SerializeField] private GameObject _bossLaserBeamPrefb;
    [SerializeField] private GameObject _bossLaserShotPrefab;
    [SerializeField] private GameObject _bossShieldPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _speed = 1.2f;
    

    private int _bossHealth = 15;
    private int _shieldHealth = 2;
    private bool _isShielded = true;
    private bool _startLaserBeam = false;
    private Rigidbody2D _rb;
    private UIManager  _uiManager;
    private GameObject _target;

#region Unity Methods

    void Start()
    {
        _bossShieldPrefab.SetActive(true);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("_uiManager :: EnemyBoss == null");
        }

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("_rb :: EnemyBoss == null");
        }

        _target = GameObject.Find("Player");
        if (_target == null)
        {
            Debug.LogError("_target :: EnemyBoss == null");
        }

        StartCoroutine(ShootLaserRoutine());
        StartCoroutine(SweepingLaserRoutine());
    }

    void Update()
    {
        Movement();

        if (_startLaserBeam)
        {
            _bossLaserBeamPrefb.transform.RotateAround(transform.position, Vector3.forward, 90 * Time.deltaTime);
        }

        if (_isShielded)
        {
            _bossShieldPrefab.SetActive(true);
        }
        else
        {
            _bossShieldPrefab.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = _target.transform.position - transform.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, -transform.up).z;

            _rb.angularVelocity = -rotateAmount * 200f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Laser") || other.tag.Equals("Missile"))
        {
            Destroy(other.gameObject);

            if (_isShielded)
            {
                _shieldHealth--;
                if (_shieldHealth == 0)
                {
                    _isShielded = false;
                    StartCoroutine(RegenerateShieldRoutine());
                }
            }
            else
            {
                _bossHealth--;

                if (_bossHealth <= 0)
                {
                    GameOver();
                }
            }
        }
    }

#endregion


#region Private Methods

    private void Movement()
    {
        if (transform.position.y > 3.46f)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
    }

    private void GameOver()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _uiManager.ShowGameOver();
        Destroy(gameObject, .25f);
    }


#endregion


#region Coroutines

    private IEnumerator SweepingLaserRoutine()
    {
        while(_bossHealth > 0)
        {
            yield return new WaitForSeconds(Random.Range(7f, 10f));
            _startLaserBeam = true;
            _bossLaserBeamPrefb.SetActive(_startLaserBeam);

            yield return new WaitForSeconds(5f);
            _startLaserBeam = false;
            _bossLaserBeamPrefb.SetActive(_startLaserBeam);
            
        }
    }

    private IEnumerator ShootLaserRoutine()
    {
        while (_bossHealth > 0)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            Instantiate(_bossLaserShotPrefab, transform.position, transform.rotation);
        }
    }

    private IEnumerator RegenerateShieldRoutine()
    {
        yield return new WaitForSeconds(10f);
        _isShielded = true;
        _shieldHealth = 2;
    }

#endregion


}
