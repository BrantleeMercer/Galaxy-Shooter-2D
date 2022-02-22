using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeingMissile : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private GameObject _target;
    private Rigidbody2D _rb;
    
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy");
        if (_target == null)
        {
            Debug.LogError("_target :: HomingMissile == null");
        }

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.Log("_rb :: HomingMissile == null");
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = _target.transform.position - transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rb.angularVelocity = -rotateAmount * 200f;

        _rb.velocity = transform.up * _speed;

    }

}
