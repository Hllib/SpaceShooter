using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //TODO: fix score bug when it is over 999
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Text _tutorialText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImg;

    private GameManager _gameManager;
    private Player _player;

    [SerializeField]
    private Slider _sliderForBoost;
    [SerializeField]
    private Slider _sliderForTripleShot;

    private float _sliderDefaultValue = 100f;
    private float _sliderStep = 20.0f;

    void Start()
    {
        _tutorialText.text = "Use SPACE to shoot\nUse arrows OR wasd to move";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        FindGameManager();
        FindPlayer();
        StartCoroutine(TutTextFlickRoutine());

        _sliderForTripleShot.maxValue = _sliderDefaultValue;
        _sliderForBoost.maxValue = _sliderDefaultValue;
        _sliderForTripleShot.value = 0f;
        _sliderForBoost.value = 0f;
        Player.BestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreText.text = $"Best: {Player.BestScore}";
        _scoreText.text = $"Score: {0}";
    }

    void FindPlayer()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.Log("Player is NULL in UIManager");
        }
    }

    void FindGameManager()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is NULL in UIManager");
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void CheckForBestScore()
    {
        if (_player.Score > Player.BestScore)
        {
            Player.BestScore = _player.Score;
            PlayerPrefs.SetInt("BestScore", Player.BestScore);
            _bestScoreText.text = $"Best: {Player.BestScore}";
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(TutTextFlickRoutine());
            _tutorialText.gameObject.SetActive(false);
        }
    }

    public void SetBoostSlider()
    {
        _sliderForBoost.value = _sliderDefaultValue;
        StartCoroutine(BoostSliderRoutine());
    }

    public void StopBoostSlider()
    {
        StopCoroutine(BoostSliderRoutine());
        _sliderForBoost.value = 0f;
    }

    IEnumerator BoostSliderRoutine()
    {
        for (int i = 0; i < _player.PowerUpCoolDownRate; i++)
        {
            yield return new WaitForSeconds(1f);
            _sliderForBoost.value -= _sliderStep;
        }
    }

    public void SetTripleShotSlider()
    {
        _sliderForTripleShot.value = _sliderDefaultValue;
        StartCoroutine(TripleShotSliderRoutine());
    }

    public void StopTripleShotSlider()
    {
        StopCoroutine(TripleShotSliderRoutine());
        _sliderForTripleShot.value = 0f;
    }

    IEnumerator TripleShotSliderRoutine()
    {
        for (int i = 0; i < _player.PowerUpCoolDownRate; i++)
        {
            yield return new WaitForSeconds(1f);
            _sliderForTripleShot.value -= _sliderStep;
        }
    }

    public void UpdateLives(int currentLivesCount)
    {
        _livesImg.sprite = _liveSprites[currentLivesCount];

        if (currentLivesCount < 1)
        {
            StartCoroutine(GameOverFlickScreenRoutine());
            _gameManager.GameOver();
        }
    }

    IEnumerator GameOverFlickScreenRoutine()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        while (true)
        {
            _restartText.text = "Press 'R' to restart";
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            _restartText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator TutTextFlickRoutine()
    {
        while (true)
        {
            _tutorialText.text = "Use SPACE to shoot\nUse arrows OR wasd to move";
            yield return new WaitForSeconds(0.65f);
            _tutorialText.text = "";
            yield return new WaitForSeconds(0.65f);
        }
    }
}

