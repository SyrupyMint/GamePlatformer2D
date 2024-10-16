using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _startingHealth;

    [Header("Iframes")]
    [SerializeField] private float _iFrameDuration;
    [SerializeField] private float _flashNumber;

    [Header("Dead Jump")]
    [SerializeField] private float _deadJumpHeight = 5f;

    private bool KnockFromRight;
    private bool invulnerable;
    private Animator _anim;
    public float currentHealth { get; private set; }
    private bool _dead;
    private Collider2D _col;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Awake()
    {
        currentHealth = _startingHealth;
        _anim = transform.Find("Visual").GetComponent<Animator>();
        _sr = transform.Find("Visual").GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
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
                _anim.SetTrigger("die");
                GetComponent<PlayerController>().enabled = false;
                _col.enabled = false;
                _rb.velocity = new Vector2(_rb.velocity.x, _deadJumpHeight);
                _dead = true;
            }
            
        }
    
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, _startingHealth);
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < _flashNumber; i++) {
            _sr.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(_iFrameDuration/ (_flashNumber * 2));
            _sr.color = Color.white;
            yield return new WaitForSeconds(_iFrameDuration / (_flashNumber * 2));
        }
        //Invulnerability
        Physics2D.IgnoreLayerCollision(6, 7, false);
        invulnerable = false;
    }
}
