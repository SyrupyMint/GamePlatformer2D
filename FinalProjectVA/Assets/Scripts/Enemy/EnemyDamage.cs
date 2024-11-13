using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            PlayerRespawn playerRespawn = collision.GetComponent<PlayerRespawn>();

            if (playerHealth != null)
            {
                playerHealth.TakeDMG(damage);

                if (playerHealth.currentHealth <= 0 && playerRespawn != null)
                {
                    playerRespawn.Respawn();
                }
            }
        }
    }
}