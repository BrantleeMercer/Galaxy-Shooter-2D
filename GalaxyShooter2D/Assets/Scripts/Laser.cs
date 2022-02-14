using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private int _ID;

    private void Start() 
    {
        AudioManager.instance.PlaySoundEffect("laser");
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        MoveLaser();
    }

    private void MoveLaser()
    {
        switch(_ID)
        {
            case 0:
                transform.Translate(Vector3.up * _speed * Time.deltaTime); //Player Laser
            break;

            case 1:
                transform.Translate(Vector3.down * _speed * Time.deltaTime); //Enemy Laser
            break; 
        }
        
    }
}
