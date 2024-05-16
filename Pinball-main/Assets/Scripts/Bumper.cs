using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    private float scaleIncrement = .025f;
    [SerializeField]
    private float scaleMax = 1.1f;
    [SerializeField]
    private float bumpForce = 1;

    [SerializeField]
    private Vector2 bumpDirection;
    [SerializeField]
    private bool useTarget = false;

    private bool growInSize = false;
    private Vector2 initScale = Vector2.one;

    AudioSource audioSource;
    [SerializeField]
    AudioClip soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        initScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (growInSize)
        {
            GrowInSize(scaleMax, scaleIncrement);
        }
    }

    public void Bump(Rigidbody2D target)
    {
        growInSize = true;
        Vector2 directionOfBump = bumpDirection;
        if (useTarget)
        {
            directionOfBump = Vector2.MoveTowards(transform.position, target.transform.position, 1);
        }
        target.AddForce(directionOfBump * bumpForce, ForceMode2D.Impulse);
        if (useTarget)
        {
            bumpDirection = Vector2.zero;
        }
        audioSource.PlayOneShot(soundEffect);
    }

    private void GrowInSize(float maxScale, float increment)
    {
        if (transform.localScale.x < maxScale)
        {
            Vector2 newScale = transform.localScale;
            newScale.x += increment;
            newScale.y += increment;
            transform.localScale = newScale;
        }
        else
        {
            transform.localScale = initScale;
            growInSize = false;
        }
    }
}
