using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _explosionSFX;
    [SerializeField] private AudioSource _laserSoundFX;
    [SerializeField] private AudioSource _powerupSFX;

    private void Awake() 
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void PlaySoundEffect(string soundName)
    {
        switch(soundName)
        {
            case "explosion":
                _explosionSFX.Play();
            break;

            case "laser":
                _laserSoundFX.Play();
            break;

            case "powerup":
                _powerupSFX.Play();
            break;

            default:
                Debug.LogError("PlaySoundEffect :: AudioManager == Sound not set up");
            break;
        }
    }



































    // [SerializeField] private AudioSource _laserSoundFX;
    // [SerializeField] private AudioSource _powerupSFX;

    // public void PlayLaserSoundFX()
    // {
    //     _laserSoundFX.PlayOneShot(_laserSoundFX.clip);
    // }

    // public void PlayPowerupSFX()
    // {
    //     _powerupSFX.PlayOneShot(_powerupSFX.clip);
    // }
}
