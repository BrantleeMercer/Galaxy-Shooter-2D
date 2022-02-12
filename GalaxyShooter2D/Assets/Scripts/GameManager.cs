using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool _isGameOver = false;

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
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1); //Current game scene is scene 1
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        _isGameOver = isGameOver;
    }
}
