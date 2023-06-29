using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] float lifeTime = 1.0f;
    public bool laser;
    Renderer targetRenderer;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        targetRenderer = GetComponent<Renderer>();
    }


}
