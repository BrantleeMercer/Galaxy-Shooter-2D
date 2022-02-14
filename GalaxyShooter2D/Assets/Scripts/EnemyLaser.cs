using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    
    private void Start() 
    {
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        MoveLaser();
    }

    private void MoveLaser()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
