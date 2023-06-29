using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public GameObject explosionEffect;

    private void Update()
    {
        transform.Rotate(0f, 0f, 100 * Time.deltaTime);
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(1f);
            Destroy(explosion);
        }
    }
}
