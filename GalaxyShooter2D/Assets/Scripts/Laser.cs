using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    [SerializeField] private float _speed = 8f;

    // Update is called once per frame
    void Update()
    {
        MoveLaser();
    }

    private void MoveLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
