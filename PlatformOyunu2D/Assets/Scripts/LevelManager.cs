﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Text scoreValueText;
    private void Start()
    {
        scoreValueText = GameObject.Find("Score Value").GetComponent<Text>();
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClosePanel(string parentName)
    {
        GameObject.Find(parentName).SetActive(false);
    }

    public void addScore(int score)
    {
        int scoreValue = int.Parse(scoreValueText.text);
        scoreValue += score;
        scoreValueText.text = scoreValue.ToString();
    }
}
