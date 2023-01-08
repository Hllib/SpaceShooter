using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _soundOffButton;
    [SerializeField]
    private Button _soundOnButton;
    [SerializeField]
    private Text _aboutText;
    [SerializeField]
    private Button _backToMenuButton;
    [SerializeField]
    private Image _bgForAbout;

    private void Start()
    {
        _bgForAbout.gameObject.SetActive(false);
        _aboutText.gameObject.SetActive(false);
        _backToMenuButton.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // game scene
        AudioListener.volume = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SoundOff()
    {
        AudioListener.volume = 0;
        _soundOffButton.gameObject.SetActive(false);
        _soundOnButton.gameObject.SetActive(true);
    }

    public void SoundOn()
    {
        AudioListener.volume = 1;
        _soundOffButton.gameObject.SetActive(true);
        _soundOnButton.gameObject.SetActive(false);
    }

    public void ResetBestScore()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        Player.BestScore = 0;
    }

    public void About()
    {
        _aboutText.gameObject.SetActive(true);
        _backToMenuButton.gameObject.SetActive(true);
        _bgForAbout.gameObject.SetActive(true);
    }

    public void CloseAbout()
    {
        _aboutText.gameObject.SetActive(false);
        _backToMenuButton.gameObject.SetActive(false);
        _bgForAbout.gameObject.SetActive(false);
    }
}
