using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class mainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    public AudioClip main_music;
    public AudioSource main_channel;

    private void Start()
    {
        main_channel.PlayOneShot(main_music);
        int highScore = saveLoadManager.Instance.loadHighScore();
        highScoreUI.text = $"Highest Wave Reached: {highScore}";
    }
    public void startGame ()
    {
        main_channel.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
