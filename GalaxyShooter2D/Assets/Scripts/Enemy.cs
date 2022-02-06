using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private float _speed = 4f;

    void Update()
    {
       SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        if (transform.position.y < -5.4f)
        {
            float randX = Random.Range(-9.4f, 9.4f);
            
            transform.position = new Vector3(randX, 7.3f, 0);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();

            if(player != null)
            {
                player.Damage(1);
            }

            Destroy(gameObject);
        }
        else if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }
}
