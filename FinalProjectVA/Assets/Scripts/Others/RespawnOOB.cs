using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOOB : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawnPoint.transform.position;
            col.GetComponent<Health>().TakeDMG(1);

            // Check if the player's health is zero
            if (col.GetComponent<Health>().currentHealth <= 0)
            {
                // Respawn the player
                col.GetComponent<PlayerRespawn>().Respawn();
                //When 0 health, make player return to the same spot
                player.transform.position = respawnPoint.transform.position;
            }
        }
    }
}
