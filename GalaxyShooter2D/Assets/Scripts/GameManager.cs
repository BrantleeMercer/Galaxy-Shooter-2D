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
    private int _waveIndex;
    private Player _player;

#endregion


#region Unity Methods

    private void Awake() 
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("_player :: GameManager == null");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
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
        Instantiate(_asteroidPrefab, _asteroidPrefab.transform.position, Quaternion.identity);
        _player.ResetShotCount();
        _waveIndex++;
    }

    public int GetWaveIndex()
    {
        return _waveIndex;
    }

#endregion

}
