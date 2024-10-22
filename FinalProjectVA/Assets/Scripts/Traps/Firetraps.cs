using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetraps : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _firetrapDMG;

    [Header("Firetrap Timer")]
    [SerializeField] private float _activationDelay;
    [SerializeField] private float _activationTime;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isTriggered; //Traps triggered
    private bool isActivated; //Traps activated hurt players

    private Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerHealth != null && isActivated) 
        {
            playerHealth.TakeDMG(_firetrapDMG);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            if (!isTriggered)
            {
                //Trigger the firetrap
                StartCoroutine(ActivateFiretrap());

            }
            if (isActivated)
            {
                collision.GetComponent<Health>().TakeDMG(_firetrapDMG);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = null;
        }
    }

    IEnumerator ActivateFiretrap()
    {
        //Trigger the trap
        isTriggered = true;
        sr.color = Color.red; //turn red to alert player

        //Delay to activatate trap
        yield return new WaitForSeconds(_activationDelay);
        sr.color = Color.white; //Back to normal
        isActivated = true;
        anim.SetBool("activated", true);

        //Deactivate trap after X seconds, reset bools
        yield return new WaitForSeconds(_activationTime);
        isActivated = false;
        isTriggered = false;
        anim.SetBool("activated", false);
    }
}
