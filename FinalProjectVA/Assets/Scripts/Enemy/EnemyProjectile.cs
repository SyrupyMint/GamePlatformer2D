using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private BoxCollider2D coll;

    private bool hit;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WindArea")
            return;
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent script first
        coll.enabled = false;
        gameObject.SetActive(false); //When this hits any object deactivate arrow
    }
}