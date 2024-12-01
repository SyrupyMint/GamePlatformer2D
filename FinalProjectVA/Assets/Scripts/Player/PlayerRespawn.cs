using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private RespawnOOB respawn;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake()
    {
        GameObject respawnObject = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnObject == null)
        {
            Debug.LogError("No object tagged as 'Respawn' found in the scene.");
            return;
        }

        // Get the RespawnOOB component
        respawn = respawnObject.GetComponent<RespawnOOB>();
        if (respawn == null)
        {
            Debug.LogError("RespawnOOB component is missing on the Respawn object!");
            return;
        }

        // Get the Player's Health component
        playerHealth = GetComponent<Health>();
        if (playerHealth == null)
        {
            Debug.LogError("Player Health component is missing!");
            return;
        }
        currentCheckpoint = respawn.transform;
    }

    public void Respawn()
    {
        if (playerHealth != null && currentCheckpoint != null)
        {
            playerHealth.Respawn(); // Restore player health and reset animation
            transform.position = currentCheckpoint.position; // Move player to checkpoint location
        }
        else
        {
            Debug.LogWarning("Cannot respawn. PlayerHealth or CurrentCheckpoint is null.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");

            if (collision.CompareTag("Checkpoint"))
            {
                if (respawn == null)
                {
                    Debug.LogError("Respawn is null! Ensure the Respawn object exists and is tagged correctly.");
                    return;
                }

                respawn.respawnPoint = collision.gameObject;
                currentCheckpoint = collision.transform;

                Collider2D collider = collision.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"Collider2D is missing on Checkpoint: {collision.gameObject.name}");
                }

                Animator animator = collision.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("appear");
                }
                else
                {
                    Debug.LogWarning($"Animator is missing on Checkpoint: {collision.gameObject.name}");
                }
            }
        }
    }
}
