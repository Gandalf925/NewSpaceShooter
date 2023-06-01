using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScrollCollider : MonoBehaviour
{
    Stage2Manager stage2Manager;
    // Start is called before the first frame update
    void Start()
    {
        stage2Manager = FindObjectOfType<Stage2Manager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            stage2Manager.EndScrollStage();
        }
    }
}
