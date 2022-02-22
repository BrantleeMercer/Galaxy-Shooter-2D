using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText, _gameOverText, _restartInstructionsText, _waveCounterText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage, _magnetImage;
    private string _scoreString = "Score: ";

    void Start()
    {
        _scoreText.text = _scoreString + 0;
        _gameOverText.gameObject.SetActive(false);

        ShowWaveCounter();
    }

    public void UpdateLivesImage(int currentHealth)
    {
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        _livesImage.sprite = _livesSprites[currentHealth];

        if(currentHealth <= 0)
        {
            ShowGameOver();
        }
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = _scoreString + score;
    }

    public void UpdateMagnetImage(bool showImage)
    {
        _magnetImage.gameObject.SetActive(showImage);
    }

    public void ShowGameOver()
    {
        GameManager.instance.SetGameOver(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    public void ShowWaveCounter()
    {
        StartCoroutine(ShowWaveCounterRoutine());
    }

#region Coroutines

    public IEnumerator GameOverFlickerRoutine()
    {
        _gameOverText.gameObject.SetActive(true);

        while (true)
        {
            _gameOverText.text = "GAME OVER";
            _restartInstructionsText.text = "PRESS \"R\" TO RESTART";
            yield return new WaitForSeconds(1f);

            _gameOverText.text = "";
            _restartInstructionsText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }

    public IEnumerator ShowWaveCounterRoutine()
    {
        _waveCounterText.gameObject.SetActive(true);
        
        int waveIndex = GameManager.instance.GetWaveIndex();

        if(waveIndex == 4)
        {
            _waveCounterText.text = "Final Wave!";
        }
        else
        {
            _waveCounterText.text = $"Wave: {waveIndex}";
        }

        yield return new WaitForSeconds (3f);
        _waveCounterText.gameObject.SetActive(false);
        
    }

#endregion
}
