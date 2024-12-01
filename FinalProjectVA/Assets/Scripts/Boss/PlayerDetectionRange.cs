using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionRange : MonoBehaviour
{
    private Boss boss;

    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.isPlayerInRange = true;
        }
    }
}
