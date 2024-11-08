using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private AppleManager appleManager;
    private void Start()
    {
        appleManager = AppleManager.instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            appleManager.ChangeApples(value);
            Destroy(gameObject);
        }
    }
}
