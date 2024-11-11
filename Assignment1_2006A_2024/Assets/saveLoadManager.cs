using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveLoadManager : MonoBehaviour
{
    public static saveLoadManager Instance { get; set; }

    string highScoreKey = "BestWaveSavedValue";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public void saveHighScore(int score)
    {
        PlayerPrefs.SetInt(highScoreKey, score);
    }

    public int loadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
        }
    }
}
