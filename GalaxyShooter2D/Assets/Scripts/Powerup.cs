using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    
    void Update()
    {
        SpawnPowerup();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.ActivateTripleShot();
            }
            
            Destroy(gameObject);
        }
    }

    private void SpawnPowerup()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.8f)
        {
            Destroy(gameObject);
        }     
    }
}
