using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float maxVelocity = 50;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        switch(tag)
        {
            case "Dead":
                //GameManager.instance.GameEnd();
                GameManager.instance.BallLoss();
                break;

            case "Bouncer":
                GameManager.instance.UpdateScore(10, 1);
                Bumper bumper1;
                if (bumper1 = collision.gameObject.GetComponent<Bumper>()) {
                    bumper1.Bump(rb);
                }
                break;

            case "Point":
                GameManager.instance.UpdateScore(20, 1);
                Bumper bumper2;
                if (bumper2 = collision.gameObject.GetComponent<Bumper>())
                {
                    bumper2.Bump(rb);
                }
                break;

            case "Side":
                GameManager.instance.UpdateScore(10, 0);
                break;

            case "Flipper":
                GameManager.instance.multiplier = 1;
                break;

            default:
                break;
        }
    }
}
