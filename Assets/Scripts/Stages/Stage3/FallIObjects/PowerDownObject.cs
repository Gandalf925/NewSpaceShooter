using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDownObject : MonoBehaviour
{
    PlayerController player;
    public GameObject explosionEffect;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, 100 * Time.deltaTime);
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.PowerDown();
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(1f);
            Destroy(explosion);
        }
    }
}
