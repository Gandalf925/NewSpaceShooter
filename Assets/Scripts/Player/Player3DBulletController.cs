using UnityEngine;

public class Player3DBulletController : MonoBehaviour
{
    public int attackPower = 3;
    [SerializeField] GameObject explosionPrefab;

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
}
