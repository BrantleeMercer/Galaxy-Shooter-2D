using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Start()
    {        
        AudioManager.instance.PlaySoundEffect("explosion");

        Destroy(gameObject, 3f);
    }
}
