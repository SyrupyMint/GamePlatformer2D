using System.Collections;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    [SerializeField] private float speedBoost = 4f; 
    [SerializeField] private float boostDuration = 15f; 

    private bool isCollected = false;

    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(ApplySpeedBoost(other.GetComponent<PlayerController>()));
        }
    }

    IEnumerator ApplySpeedBoost(PlayerController player)
    {
        if (player == null) yield break;

        player.walkSpeed += speedBoost;

        sr.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(boostDuration);

        player.walkSpeed -= speedBoost;
        sr.enabled = true;
        col.enabled = true;
        isCollected = false;
    }
}