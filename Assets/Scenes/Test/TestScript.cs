using UnityEngine;

public class TestScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with " + other.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }
}