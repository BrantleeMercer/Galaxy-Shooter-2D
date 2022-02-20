using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

#region Serialize Fields

   [SerializeField] private GameObject _asteroidPrefab;

#endregion


#region Public Variables

 public static GameManager instance;

#endregion


#region Private Variables

    private bool _isGameOver = false;
    private int _waveIndex = 1;

#endregion


#region Unity Methods

    private void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            _waveIndex = 1;
            SceneManager.LoadScene(1); //Current game scene is scene 1
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); // Quits the Unity Application
        }
    }


#endregion


#region Public Methods

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }

    public void StartNewWave()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("_player :: GameManager == null");
        }

        Instantiate(_asteroidPrefab, _asteroidPrefab.transform.position, Quaternion.identity);
        player.ResetShotCount();
        _waveIndex++;
    }

    public int GetWaveIndex()
    {
        return _waveIndex;
    }

#endregion

}
