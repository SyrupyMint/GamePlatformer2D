using System.Collections;
using UnityEngine;

public class ResetDashPowerUp : MonoBehaviour
{
    [SerializeField] private float respawnTime = 5f;
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
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ResetDash(player);
            }

            StartCoroutine(RespawnOrb());
        }
    }

    private void ResetDash(PlayerController player)
    {
        player.hasDashed = false;
        player.isDashing = false;
    }

    private IEnumerator RespawnOrb()
    {
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);
        sr.enabled = true;
        col.enabled = true;
        isCollected = false;
    }
}