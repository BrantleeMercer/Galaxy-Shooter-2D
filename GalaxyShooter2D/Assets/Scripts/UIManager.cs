using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText, _gameOverText, _restartInstructionsText;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage;
    private string _scoreString = "Score: ";

    void Start()
    {
        _scoreText.text = _scoreString + 0;
        _gameOverText.gameObject.SetActive(false);
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

    public void ShowGameOver()
    {
        GameManager.instance.SetGameOver(true);
        StartCoroutine(nameof(GameOverFlickerRoutine));
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

#endregion
}
