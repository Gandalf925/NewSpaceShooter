using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    public float score = 0;

    public void UpdateLives()
    {
        lives -= 1;
    }

    public void UpdateScore(float point)
    {
        score += point;
    }
}
