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

                    case 3: // 3 = Health
                        player.ActivateHealthPowerup();
                    break; 

                    case 4: // 4 = Refill Ammo
                        player.ActivateRefillPowerup();
                    break; 

                    case 5: // 5 = Circular shot powerup
                        player.ActivateCircularShot();
                    break; 

                    case 6: // 6 = Homing Missile powerup
                        player.ActivateMissileRefillPowerup();
                    break;

                    default:
                        Debug.LogError("Powerup Not Accounted For");
                    break;                 
                }
            }

            AudioManager.instance.PlaySoundEffect("powerup");
            
            Destroy(gameObject);
        }

        if (other.transform.tag.Equals("EnemyLaser"))
        {
            Destroy(other.gameObject);
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
