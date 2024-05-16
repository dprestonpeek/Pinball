using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    [SerializeField]
    GameObject tempWall, permWall, tempHigher, permBumper, permLower;
    [SerializeField]
    Slider spring;

    [SerializeField]
    float launchMultiplier = 10f;

    AudioSource audioSource;
    [SerializeField]
    AudioClip boing;

    public bool DrawingSpring = false;
    public bool LaunchingBall = false;
    public bool DoneStartup = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void DrawSpring()
    {
        spring.value -= .01f;
        DrawingSpring = true;
    }

    public void LaunchBall(Rigidbody2D target)
    {
        if (target)
        {
            target.simulated = true;
            target.AddForce(Vector2.up * (1 - spring.value) * launchMultiplier);
        }
        DrawingSpring = false;
        LaunchingBall = true;
        ResetSpring();
    }

    public void ResetSpring()
    {
        spring.value = 1;
        audioSource.PlayOneShot(boing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FinishStartup();
    }

    private void FinishStartup()
    {
        tempWall.SetActive(false);
        permWall.SetActive(true);
        permBumper.GetComponent<PolygonCollider2D>().enabled = true;
        permLower.GetComponent<PolygonCollider2D>().enabled = true;
        tempHigher.SetActive(false);
        DoneStartup = true;
    }

    public void SetupStartup()
    {
        tempWall.SetActive(true);
        permWall.SetActive(false);
        permBumper.GetComponent<PolygonCollider2D>().enabled = false;
        permLower.GetComponent<PolygonCollider2D>().enabled = false;
        tempHigher.SetActive(true);
        DoneStartup = false;
        DrawingSpring = false;
        LaunchingBall = false;
    }
}
