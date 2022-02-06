using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _rateOfFire = 0.5f;
    private float _canFire = -1f;


    void Start()
    {
        transform.position = new Vector3(0,0,0);
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
        
        transform.Translate(direction * _speed * Time.deltaTime);

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
        Vector3 frontOfShip = new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z);
        GameObject laser = Instantiate(_laserPrefab, frontOfShip, Quaternion.identity);

        Destroy(laser, 1.5f);
    }
}
