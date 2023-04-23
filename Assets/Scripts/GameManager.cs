using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    public int score = 0;

    public void UpdateLives()
    {
        lives -= 1;
    }

    public void UpdateScore(int damage)
    {
        score += damage;
    }
}
