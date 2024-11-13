using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform weaponPoint;

    private GameObject bulletInst;

    private Vector2 worldPos;
    private Vector2 dir;
    private float angle;

    // Update is called once per frame
    void Update()
    {
        HandleWeaponRotation();
        HandleWeaponAttack();
    }

    private void HandleWeaponAttack()
    {
       if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            bulletInst = Instantiate(bullet, weaponPoint.position, weapon.transform.rotation);
        }
    }

    private void HandleWeaponRotation()
    {
        worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        dir = (worldPos - (Vector2)weapon.transform.position).normalized;
        weapon.transform.right = dir;

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Vector3 ls = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < -90)
        {
            ls.y = -1f;
        }
        else
        {
            ls.x = 1f;
        }
        weapon.transform.localScale = ls;
    }

}
