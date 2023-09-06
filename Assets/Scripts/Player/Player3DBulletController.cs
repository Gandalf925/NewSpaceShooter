using System.Collections;
using UnityEngine;

public class Player3DBulletController : MonoBehaviour
{
    public int attackPower = 3;
    [SerializeField] GameObject explosionPrefab;
    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        DisableCollider();
        StartCoroutine(EnableColliderAfter(0.1f));
    }

    private void Update()
    {
        if (transform.position.z > 50)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.8f);
            Destroy(gameObject);
        }
    }

    void DisableCollider()
    {
        col.enabled = false;
    }

    void EnableCollider()
    {
        col.enabled = true;
    }

    IEnumerator EnableColliderAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        EnableCollider();
    }
}
