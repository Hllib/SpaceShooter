using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManu : MonoBehaviour
{
    [SerializeField]
    private Button _soundOffButton;
    [SerializeField]
    private Button _soundOnButton;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // main menu scene
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // game scene
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
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
}
