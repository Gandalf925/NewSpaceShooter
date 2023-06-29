using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCandy : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, 100 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.AddPowerupPoint(50);

            gameManager.UpdateScore(20);

            Destroy(gameObject);
        }
    }
}
