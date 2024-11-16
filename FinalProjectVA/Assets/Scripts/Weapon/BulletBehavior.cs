using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Header("General Bullet Stats")]
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;

    [Header("Normal Bullet Stats")]
    [SerializeField] private float normalBulletSpd = 15f;
    [SerializeField] private float normalBulletDmg = 1f;

    [Header("Physics Bullet Stats")]
    [SerializeField] private float physicsBulletSpd = 17.5f;
    [SerializeField] private float physicsBulletGravity = 3f;
    [SerializeField] private float physicsBulletDmg = 2f;

    private Rigidbody2D rb;
    private float dmg;

    public enum BulletType
    {
        Normal,
        Physics
    }
    public BulletType bulletType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetDestroyTime();

        //Change RigidBody Stats based on bullet type
        SetRigidBodyStats();

        //Set Velocity based on bullet type
        InitializeBulletStats();
    }

    private void FixedUpdate()
    {
        if (bulletType == BulletType.Physics)
        {
            //rotate bullet in dir of velocity
            transform.right = rb.velocity;
        }
    }

    private void SetRigidBodyStats()
    {
        if (bulletType == BulletType.Normal)
        {
            rb.gravityScale = 0f;
        }
        else if (bulletType == BulletType.Physics)
        {
            rb.gravityScale = physicsBulletGravity;
        }
    }

    private void InitializeBulletStats()
    {
        if (bulletType == BulletType.Normal)
        {
            SetStraightVelocity();
            dmg = normalBulletDmg;
        } 
        else if (bulletType == BulletType.Physics)
        {
            SetPhysicsVelocity();
            dmg = physicsBulletDmg;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((whatDestroysBullet.value & (1 << col.gameObject.layer)) > 0){
            Health enemyHealth = col.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDMG(dmg);
            }
            //Destroy the bullet when colliding walls/objects/enemies
            Destroy(gameObject);
        }
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }

    private void SetStraightVelocity()
    {
        rb.velocity = transform.right * normalBulletSpd;
    }

    private void SetPhysicsVelocity()
    {
        rb.velocity = transform.right * physicsBulletSpd;
    }
}
