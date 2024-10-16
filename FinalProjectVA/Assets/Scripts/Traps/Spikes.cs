using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float dmg;
    public PlayerController _cont;

    private void Start()
    {
        _cont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _cont.KBCounter = _cont.KBTotalTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                _cont.KnockFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                _cont.KnockFromRight = false;
            }
            collision.GetComponent<PlayerHealth>().TakeDMG(dmg);
        }
    }
}
