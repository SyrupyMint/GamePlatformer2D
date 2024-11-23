using System.Collections;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{
    [SerializeField] private float jumpBoost = 5f;
    [SerializeField] private float boostDuration = 10f;

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

        player.jumpPower += jumpBoost;

        sr.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(boostDuration);

        player.jumpPower -= jumpBoost;
        sr.enabled = true;
        col.enabled = true;
        isCollected = false;
    }
}