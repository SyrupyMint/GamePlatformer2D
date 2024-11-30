using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collision col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LadderMovement lm;

    private bool isInsideTrigger = false;

    [Header("Wind Variables")]
    public float windValue = 0.0f;
    public float windDirection = 1.0f;
    public float windMultiplier = 10.0f;
    public float windPower = 2.0f;
    public float windDivider = 500.0f;

    [Space]
    [Header("Wind Direction")]
    public float timerAmount = 5.0f;
    public float startTimer = 0.0f;
    private float lastWindDirection;

    [Header("No Wind Direction")]
    public float noWindTimerAmount = 2.5f;
    public float noWindStartTimer = 0.0f;

    [Header("Particle")]
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private float lerpTimer = 1.0f;
    [SerializeField] private float xVelocityValue = 5.0f;
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    private void Awake()
    {
        velocityModule = particle.velocityOverLifetime;
    }

    private void Update()
    {
        ChangeWindDirection();
    }

    private void FixedUpdate()
    {
        if (isInsideTrigger && !col.onGround && !col.onWall && !lm.isClimbing)
        {
            ApplyWind();
        }
        else
        {
            windValue = 0.0f;
        }
    }

    private void ApplyWind()
    {
        float wind = Mathf.Pow(Mathf.Sin(windValue) * windMultiplier, windPower);

        windValue += Mathf.PI / windDivider;

        Vector2 forceToAdd = new Vector2(wind * windDirection, 0.0f);
        rb.AddForce(forceToAdd, ForceMode2D.Impulse);
    }

    private void ChangeWindDirection()
    {
        if (startTimer <= timerAmount)
        {
            startTimer += Time.deltaTime;
        }
        else
        {
            if (windDirection != 0.0f)
            {
                lastWindDirection = windDirection;
                StartCoroutine(Lerp(lerpTimer, xVelocityValue * lastWindDirection, 0.0f));
            }

            windDirection = 0.0f;
            NoWindDirection();
        }
    }

    private void NoWindDirection()
    {
        if (noWindStartTimer <= noWindTimerAmount)
            noWindStartTimer += Time.deltaTime;
        else
        {
            if (windDirection != lastWindDirection * -1)
                StartCoroutine(Lerp(lerpTimer, 0.0f, xVelocityValue * lastWindDirection * -1.0f));

            noWindStartTimer = 0.0f;
            startTimer = 0.0f;
            windDirection = lastWindDirection * -1;
        }
    }

    private IEnumerator Lerp(float time, float startDampAmount, float endDampAmount)
    {
        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / time));
            velocityModule.xMultiplier = lerpedAmount;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInsideTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInsideTrigger = false;
        }
    }
}
