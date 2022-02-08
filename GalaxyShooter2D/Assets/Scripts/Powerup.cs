using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID; // Each powerup will have it's own ID

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
                switch (_powerupID)
                {
                    case 0: // 0 = TripleShot
                        player.ActivateTripleShot();
                    break;

                    case 1: // 1 = Speed
                        player.ActivateSpeedBoost();
                    break;

                    case 2: // 2 = Shields
                        player.ActivateShields();
                    break;   

                    default:
                        Debug.LogError("Powerup Not Accounted For");
                    break;                 
                }
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
