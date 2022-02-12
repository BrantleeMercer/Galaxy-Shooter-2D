using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] private float _speed = 4f;
   private Animator _enemyDeathAnim;
   private Player _player;

   private void Start() 
   {
       _player = GameObject.Find("Player").GetComponent<Player>();
       if(_player == null)
       {
           Debug.LogError("_player :: Enemy == null");
       }

       _enemyDeathAnim = GetComponent<Animator>();
       if(_enemyDeathAnim == null)
       {
           Debug.LogError("_enemyDeathAnim :: Enemy == null");
       }
   }

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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("Player"))
        {
            if(_player != null)
            {
                _player.Damage(1);
            }

            PlayDeathAnimation();

        }
        else if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.AddToScore(10);
            }
            
            PlayDeathAnimation();
            
        }
    }

    private void PlayDeathAnimation()
    {
        _enemyDeathAnim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        Destroy(gameObject, 2.8f);
    }
}
