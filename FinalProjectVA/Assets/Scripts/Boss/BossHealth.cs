using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float health = 25;
    public Animator animator;
    public bool isInvulnerable = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;

        if (health <= 10)
        {
            animator.SetBool("isEnraged", true);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void Deactivate()
    {
        Destroy(gameObject);
    }
}
