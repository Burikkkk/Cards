using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player
{
    private int score = 0;
    private TMP_Text scoreText;

    public void SetScoreText(TMP_Text text)
    {
        scoreText = text;
    }

    public int Score
    {
        get
        {
            return score;
        }
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
