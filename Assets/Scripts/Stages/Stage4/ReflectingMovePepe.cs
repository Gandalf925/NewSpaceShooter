using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ReflectingMovePepe : MonoBehaviour
{
    public Transform startPos;
    public Transform stopPos;
    public float moveSpeed = 3f;
    public float waitTime = 1f;
    public int maxReflections = 10;
    public float reflectionDelay = 3f;

    private int reflectionsCount = 0;
    private Vector2 direction;
    private Rigidbody2D rb;
    private bool hasHitWall = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;

        // Move from startPos to stopPos in 1 second
        transform.position = startPos.position;
        transform.DOMove(stopPos.position, 1f).OnComplete(StartMoving);
    }

    void StartMoving()
    {
        // Wait for 1 second
        Invoke(nameof(BeginReflection), waitTime);
    }

    void BeginReflection()
    {
        if (reflectionsCount >= maxReflections)
        {
            reflectionsCount = 0;
            // Wait for reflectionDelay seconds before starting again
            Invoke(nameof(StartMoving), reflectionDelay);
            return;
        }

        rb.simulated = true;
        // Random direction
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

    void FixedUpdate()
    {
        if (hasHitWall)
        {
            hasHitWall = false;
            if (reflectionsCount >= maxReflections)
            {
                rb.simulated = false;
                BeginReflection();
            }
        }
        else
        {
            rb.velocity = direction * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // Get the normal from the WallNormal component
            Vector2 normal = other.GetComponent<WallNormal>().normal;

            // Reflect off wall
            direction = Vector2.Reflect(direction, normal);
            reflectionsCount++;
            hasHitWall = true;
        }
    }
}