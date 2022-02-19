using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativePowerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _negativePowerupID; // Each powerup will have it's own ID

    void Update()
    {
        SpawnNegativePowerup();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag.Equals("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_negativePowerupID)
                {
                    case 0: // 0 = Reduce Ammo
                        player.ActivateReduceAmmo();
                    break;

                    default:
                        Debug.LogError("Negative Powerup Not Accounted For");
                    break;                 
                }
            }

            AudioManager.instance.PlaySoundEffect("powerup");
            
            Destroy(gameObject);
        }
    }

    private void SpawnNegativePowerup()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.8f)
        {
            Destroy(gameObject);
        }     
    }
}
