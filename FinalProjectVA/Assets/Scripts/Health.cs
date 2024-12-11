using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _startingHealth;

    [Header("Iframes")]
    [SerializeField] private float _iFrameDuration;
    [SerializeField] private float _flashNumber;

    [Header("Componenets")]
    [SerializeField] private Behaviour[] _comp;

    private bool KnockFromRight;
    private bool invulnerable;
    private Animator _anim;
    public float currentHealth { get; set; }
    private bool _dead;
    private SpriteRenderer _sr;

    private void Awake()
    {
        currentHealth = _startingHealth;
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    public void TakeDMG(float _dmg)
    {
        if (invulnerable)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - _dmg, 0, _startingHealth);
        
        if(currentHealth > 0) 
        {
            //Player getting DMG
            _anim.SetTrigger("hurt");
            //Iframe
            StartCoroutine(Invulnerability());
        }
        else
        {
            //Player dead
            if (!_dead)
            {
                Die();
            }
            
        }
    
    }

    public void Die()
    {
        //Deactivate all attached component classes
        foreach (Behaviour component in _comp)
            component.enabled = false;
        _anim.SetTrigger("die");
        _dead = true;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, _startingHealth);
    }

    public void Respawn()
    {
        _dead = false;
        AddHealth(_startingHealth);
        _anim.ResetTrigger("die");
        _anim.Play("Idle");
        StartCoroutine(Invulnerability());

        //Activate all attached component classes
        foreach (Behaviour component in _comp)
            component.enabled = true;
    }

    public IEnumerator Invulnerability()
    {
        invulnerable = true;

        // Ensure Layer 6 and 11 are colliding initially (adjust layer numbers if needed)
        if (!Physics2D.GetIgnoreLayerCollision(6, 11))
        {
            Physics2D.IgnoreLayerCollision(6, 11, true);
        }

        for (int i = 0; i < _flashNumber; i++)
        {
            _sr.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(_iFrameDuration / (_flashNumber * 2)); // Yield control between frames
            _sr.color = Color.white;
            yield return new WaitForSeconds(_iFrameDuration / (_flashNumber * 2));
        }

        // Re-enable collision after invulnerability (adjust layer numbers if needed)
        if (Physics2D.GetIgnoreLayerCollision(6, 11))
        {
            Physics2D.IgnoreLayerCollision(6, 11, false);
        }

        invulnerable = false;
    }

    //When enemies die
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
