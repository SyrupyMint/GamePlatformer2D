using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformer : MonoBehaviour
{
    public float PlatformSpd;
    public int startingPoint; //position of the platform
    public Transform[] points; //Points that set for platform to move

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, PlatformSpd * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Ensure the player is unparented only when it's not on the platform anymore
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.transform.SetParent(null);
        }
    }
}
